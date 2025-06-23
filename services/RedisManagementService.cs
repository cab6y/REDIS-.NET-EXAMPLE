using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace REDIS_.NET_API.services
{
    public class RedisManagementService
    {

        public async Task<(IDatabase db, ConnectionMultiplexer redis, StackExchange.Redis.IServer server)> Connect()
        {
            var redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
            var db = redis.GetDatabase();
            var server = redis.GetServer("localhost", 6379);
            return (db, redis, server);
        }


        public async Task<bool> InsertValues(List<Dictionary<string, string>> values)
        {
            var (db, _redis, _server) = await Connect(); // sadece db'yi kullanacağız

            foreach (var value in values)
            {
                foreach (var kvp in value)
                {
                    await db.StringSetAsync(kvp.Key, kvp.Value);
                }
            }

            return true;
        }


        public async Task<List<Dictionary<string, string>>> GetListAsync()
        {
            var (db, _redis, _server) = await Connect();

            // Tüm anahtarları al
            var keys = _server.Keys().ToArray();

            var result = new List<Dictionary<string, string>>();

            foreach (var key in keys)
            {
                var value = await db.StringGetAsync(key);
                result.Add(new Dictionary<string, string>
        {
            { key, value }
        });
            }

            return result;
        }

        public async Task<Dictionary<string, string>> GetByKeyFilter(string keyPattern)
        {
            var (db, redis, server) = await Connect();

            // Prefix filtreleme (örnek: "user:*")
            var keys = server.Keys(pattern: $"{keyPattern}*").ToArray();

            var keyValuePairs = await Task.WhenAll(
                keys.Select(async k =>
                {
                    var v = await db.StringGetAsync(k);
                    return new { Key = (string)k, Value = (string)v };
                })
            );

            return keyValuePairs.ToDictionary(x => x.Key, x => x.Value);
        }
        public string userAgeSet = "users:age";

        public async Task<bool> PostUserAges(List<Dictionary<string, int>> list)
        {
            var (db, _redis, _server) = await Connect();
            foreach (var item in list)
            {
                foreach (var kvp in item)
                {
                    db.SortedSetAdd(userAgeSet, new SortedSetEntry[]
{
    new(kvp.Key, kvp.Value),
});
                }
            }
            return true;

        }
        public async Task<int> GetUserAge(string key)
        {
            var (db, _redis, _server) = await Connect();
            var age = await db.SortedSetScoreAsync(userAgeSet, key);
            if (age.HasValue)
            {
                return (int)age.Value;
            }
            return -1; // Eğer kullanıcı bulunamazsa -1 döndür
        }

        public async Task<bool> SendMessage(string field,string value)
        {
            var (db, redis, server) = await Connect();
            var pubSub = redis.GetSubscriber();
            await db.StreamAddAsync("mystream", field, value);
            return true;
        }
    }
}
