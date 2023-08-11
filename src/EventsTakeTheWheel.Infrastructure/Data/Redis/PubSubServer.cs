namespace EventsTakeTheWheel.Infrastructure.Data.Redis;

using System.Text.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;

public class RedisMessage<T> {
    public DateTime TriggeredAt { get; set; } = DateTime.UtcNow;
    public required T Data { get; set; }
}

public interface IPubSubServer<T> : IDisposable
{
    void RegisterHandler(Func<T, DateTime, Task> handler);
    void DeregisterHanlder(Func<T, DateTime, Task> handler);
    Task PublishAsync(T message, CommandFlags flags = CommandFlags.FireAndForget);
}

public class PubSubServer<T> : IPubSubServer<T>
{
    private readonly string _channel;
    private readonly ILogger _logger;
    private readonly IList<HandlerScope> _handlers = new List<HandlerScope>();
    private readonly IConnectionMultiplexer _redis;

    public PubSubServer(IRedisConnection redisConnection, ILogger<PubSubServer<T>> logger)
    {
        _channel = typeof(T).Name;
        _redis = redisConnection.Connection;
        _logger = logger;
        Listen();
    }

    public void Listen() {
        var sub = _redis.GetSubscriber();
        sub.Subscribe(
            RedisChannel.Literal(_channel),
            async (channel, message) =>
            {
                if (channel != _channel)
                    return;
                if (message.IsNullOrEmpty)
                    return;
                _logger.LogDebug($"Receivieved message on {_channel}: {message}");
                RedisMessage<T>? eventMessage = default;

                try
                {
                    _logger.LogDebug($"Deserializing message: {message}");
                    eventMessage = JsonSerializer.Deserialize<RedisMessage<T>>(message!);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Could not parse {typeof(T).Name} message: {message}");
                }

                if (eventMessage == null)
                    return;

                foreach (var scope in _handlers)
                {
                    _logger.LogDebug($"Invoking handler for message: {message}");
                    await scope.SemaphoreGate.WaitAsync();
                    try
                    {
                        await scope.Handler(eventMessage.Data, eventMessage.TriggeredAt);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Could not handle message: {message}");
                    }
                    finally
                    {
                        scope.SemaphoreGate.Release();
                    }
                }
            }
        );
    }

    public void RegisterHandler(Func<T, DateTime, Task> handler) =>
        _handlers.Add(new HandlerScope { Handler = handler });

    public void DeregisterHanlder(Func<T, DateTime, Task> handler) =>
        _handlers.Remove(_handlers.Where(x => x.Handler == handler).First());

    public async Task PublishAsync(T message, CommandFlags flags = CommandFlags.FireAndForget)
    {
        var sub = _redis.GetSubscriber();

        var status = sub.IsConnected() ? "Connected" : "Disconnected";
        _logger.LogDebug($"{_channel} connection status: {status}");

        var ping = await sub.PingAsync();
        _logger.LogDebug($"{_channel} ping time: {ping.TotalMilliseconds}");

        var wrappedMessage = new RedisMessage<T>{ Data = message };
        await sub.PublishAsync(RedisChannel.Literal(_channel), JsonSerializer.Serialize(wrappedMessage), flags);
    }

    public void Dispose()
    {
        _redis.GetSubscriber().UnsubscribeAll();
    }

    private class HandlerScope
    {
        public Func<T, DateTime, Task> Handler { get; set; } = (_, __) => Task.CompletedTask;
        public SemaphoreSlim SemaphoreGate { get; } = new SemaphoreSlim(1);
    }
}
