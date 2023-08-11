using EventsTakeTheWheel.Infrastructure.Data.Redis;
using StackExchange.Redis;

namespace EventsTakeTheWheel.ChargeController.Data;

public class RedisConnection : IRedisConnection
{
    public ConnectionMultiplexer Connection { get; }

    public RedisConnection(RedisSettings settings)
    {
        Connection = ConnectionMultiplexer.Connect(settings.ConnectionString);
    }

    public void Dispose()
    {
        Connection.Dispose();
    }
}


