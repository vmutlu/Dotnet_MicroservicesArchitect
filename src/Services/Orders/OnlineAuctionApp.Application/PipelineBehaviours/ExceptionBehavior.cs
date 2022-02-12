using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineAuctionApp.Application.PipelineBehaviours
{
    public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public ExceptionBehavior(ILogger<TRequest> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next().ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, $" Uygulama da beklenmedik bir hata ile karşılaşıldı. {requestName} {request}");

                throw;
            }
        }
    }
}
