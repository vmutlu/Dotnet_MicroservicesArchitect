using Microsoft.Extensions.Logging;
using OnlineAuctionApp.Core.Abstract;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;

namespace OnlineAuctionApp.Core.Concrete
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly int _retryCount;
        private readonly ILogger<RabbitMQConnection> _logger;
        private bool _disposed;

        public RabbitMQConnection(IConnectionFactory connectionFactory, int retryCount, ILogger<RabbitMQConnection> logger)
        {
            _connectionFactory = connectionFactory;
            _retryCount = retryCount;
            _logger = logger;
        }

        public bool IsConnected
        {
            get
            {
                return _connection is not null && _connection.IsOpen && !_disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                string logAndExceptionMessage = "No RabbitMQ connections ara available to perform this action";

                _logger.LogInformation($"{logAndExceptionMessage}");

                throw new InvalidOperationException($"{logAndExceptionMessage}");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (System.Exception e)
            {
                _logger.LogCritical(e.Message.ToString());
            }
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ is trying to connect");

            var policy = RetryPolicy.Handle<SocketException>().Or<BrokerUnreachableException>().WaitAndRetry(_retryCount, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)), (ex, time) =>
             {
                 _logger.LogWarning(ex, "RabbitMQ could not connect after {TimeOut}s ({ExxceptionMessage})", $"{ time.TotalSeconds:n1}", ex.Message);
             });

            policy.Execute(() =>
            {
                _connection = _connectionFactory.CreateConnection();
            });

            if (!IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                return true;
            }
            else
            {
                _logger.LogCritical("RabbitMQ connections could not be created");
                return false;
            }
        }

        #region Events

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs connectionBlockedEventArgs)
        {
            if (_disposed) return;

            _logger.LogInformation("RabbitMQ connection is shutdown. Try re-connect...");

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs callbackExceptionEventArgs)
        {
            if (_disposed) return;

            _logger.LogInformation("RabbitMQ connection is shutdown. Try re-connect...");

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs shutdownEventArgs)
        {
            if (_disposed) return;

            _logger.LogInformation("RabbitMQ connection is shutdown. Try re-connect...");

            TryConnect();
        }

        #endregion
    }
}
