using RabbitMQ.Client;
using System;

namespace OnlineAuctionApp.Core.Abstract
{
   public interface IRabbitMQConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}
