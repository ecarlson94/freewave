using EventsTakeTheWheel.Domain.Messages;
using EventsTakeTheWheel.EventProcessor;
using EventsTakeTheWheel.EventProcessor.Data.Redis;
using EventsTakeTheWheel.Infrastructure.Data.Redis;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        (hostContext, services) =>
        {
            var configuration = hostContext.Configuration;
            services.AddHostedService<ChargePollProcessor>();
            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            services.AddSingleton<IRedisConnection, RedisConnection>();
            services.AddSingleton<
                IPubSubServer<ChargePollMessage>,
                PubSubServer<ChargePollMessage>
            >();
        }
    )
    .Build();

host.Run();
