using System.Collections.Concurrent;

namespace WomensWiki.Infrastructure.Services;


public sealed class RateLimitExceededHostedService(ILoggerFactory loggerFactory) : BackgroundService {
	private readonly ILogger logger = loggerFactory.CreateLogger("RateLimitExceeded");
	private readonly ConcurrentDictionary<string, int> exceededRequestCounts = new();

	public void Record(string partition) {
		var count = exceededRequestCounts.AddOrUpdate(partition, 1, (_, currentCount) => currentCount + 1);
		if (count == 1) {
			logger.LogWarning("Rate limit exceeded for {Partition}", partition);
		}
	}
	
	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
		
		while (await timer.WaitForNextTickAsync(stoppingToken)) {
			foreach (var partition in exceededRequestCounts.Keys) {
				if (exceededRequestCounts.TryRemove(partition, out var count)) {
					if (count > 1) {
						logger.LogWarning("Rate limit exceeded for {Partition}: {Count} requests in the last minute", partition, count);
					}
				}
			}
		}
	}
}