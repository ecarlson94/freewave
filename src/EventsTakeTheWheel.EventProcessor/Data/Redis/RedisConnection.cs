using EventsTakeTheWheel.Infrastructure.Data.Redis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace EventsTakeTheWheel.EventProcessor.Data.Redis;

public class RedisConnection : IRedisConnection
{
    public ConnectionMultiplexer Connection { get; }

    public RedisConnection(IOptions<RedisSettings> settings)
    {
        Connection = ConnectionMultiplexer.Connect(settings.Value.ConnectionString);
    }

    public void Dispose()
    {
        Connection.Dispose();
    }
}
