namespace EventsTakeTheWheel.Infrastructure.Data.Redis;

using StackExchange.Redis;

public interface IRedisConnection : IDisposable
{
    ConnectionMultiplexer Connection { get; }
}
