using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineAuctionApp.Application.PipelineBehaviours
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next().ConfigureAwait(false);

            _timer.Stop();

            var elapsed = _timer.ElapsedMilliseconds; //process for milliseconds

            if(elapsed > 500)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogWarning($"{request} tipindeki istek çalışma sınır değerini geçti. Metod çalışma süresi: {requestName}");
            }

            return response;
        }
    }
}
