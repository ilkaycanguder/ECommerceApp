using System.Collections.Concurrent;

namespace ECommerceApp.Gateway.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private static readonly ConcurrentDictionary<string, (int Count, DateTime ResetTime)> _requestCounts = new();
        private const int MaxRequests = 100;
        private static readonly TimeSpan TimeWindow = TimeSpan.FromMinutes(1);

        public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var now = DateTime.UtcNow;
            var entry = _requestCounts.GetOrAdd(clientIp, _ => (0, now.Add(TimeWindow)));

            if (now > entry.ResetTime)
                entry = (0, now.Add(TimeWindow));

            if (entry.Count >= MaxRequests)
            {
                _logger.LogWarning("Rate limit exceeded for IP: {ClientIp}", clientIp);
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            _requestCounts[clientIp] = (entry.Count + 1, entry.ResetTime);
            await _next(context);
        }
    }
}
