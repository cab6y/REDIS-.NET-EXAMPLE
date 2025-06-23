using StackExchange.Redis;

namespace REDIS_.NET_API.services
{
    public class RedisStreamListener : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisStreamListener(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var db = _redis.GetDatabase();
            string lastId = "0-0";

            while (!stoppingToken.IsCancellationRequested)
            {
                var entries = await db.StreamReadAsync("mystream", lastId);
                foreach (var entry in entries)
                {
                    Console.WriteLine($"Yeni Mesaj Geldi: ID: {entry.Id}");

                    foreach (var item in entry.Values)
                        Console.WriteLine($"{item.Name}: {item.Value}");

                    lastId = entry.Id;
                }

                await Task.Delay(500); // 0.5 saniyede bir kontrol et
            }
        }
    }


}
