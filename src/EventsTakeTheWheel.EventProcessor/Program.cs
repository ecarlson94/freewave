using EventsTakeTheWheel.Domain.Messages;
using EventsTakeTheWheel.EventProcessor.Data.Redis;
using EventsTakeTheWheel.EventProcessor.Features.ChargeController;
using EventsTakeTheWheel.Infrastructure.Data.Redis;
using MongoDB.Driver;
using MongoDB.Entities;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        (hostContext, services) =>
        {
            var configuration = hostContext.Configuration;

            // Configure Redis
            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            services.AddSingleton<IRedisConnection, RedisConnection>();

            // Configure MongoDb
            var dbName =
                configuration.GetValue<string>("MongoDbSettings:DatabaseName") ?? "default";
            var dbConnectionString = configuration.GetValue<string>(
                "MongoDbSettings:ConnectionString"
            );
            var dbSettings = MongoClientSettings.FromConnectionString(dbConnectionString);
            var dbContext = new DBContext(dbName, dbSettings);
            services.AddSingleton(x => dbContext);

            // Configure Charge Poll Processor
            services.AddHostedService<ChargePollProcessor>();
            services.AddSingleton<
                IPubSubServer<ChargeControllerChargePollMessage>,
                PubSubServer<ChargeControllerChargePollMessage>
            >();
            services.AddTransient<IChargePollCollection, ChargePollCollection>();
        }
    )
    .Build();

host.Run();
