using EventsTakeTheWheel.ChargeController.Data;
using EventsTakeTheWheel.Domain;
using EventsTakeTheWheel.Domain.Values;
using EventsTakeTheWheel.Infrastructure.Data.Redis;
using Microsoft.Extensions.Logging;

const double maxKilowatts = 200.0;
const double minKilowatts = 20.0;
SerialNumber deviceSerialNumber = new()
{
  Value = new("5b82bff6-7f70-454d-a3ef-c980c4d30b31")
};

var loggerFactory = LoggerFactory.Create(builder =>
{
  builder.AddConsole();
});

var redisConnection = new RedisConnection(new RedisSettingsImpl());
var logger = loggerFactory.CreateLogger<PubSubServer<ChargePollMessage>>();

var pubSub = new PubSubServer<ChargePollMessage>(redisConnection, logger);

pubSub.RegisterHandler((message, triggeredAt) => {
  Console.WriteLine($"Recieved {message.CurrentCharge.Value} kilowatts from event bus for device: {message.Device.Value}");
  return Task.CompletedTask;
});

var cancellationTokenSource = new CancellationTokenSource();
var cancellationToken = cancellationTokenSource.Token;
var poll = new Thread(async () => {
  var random = new Random();
  while (!cancellationToken.IsCancellationRequested) {
    Thread.Sleep(500);

    var kilowatts = new Kilowatts
    {
        Value = random.NextDouble() * (maxKilowatts - minKilowatts) + minKilowatts
    };
    kilowatts.Value = Math.Round(kilowatts.Value, 2);

    await pubSub.PublishAsync(new ChargePollMessage
    {
      Device = deviceSerialNumber,
      CurrentCharge = kilowatts
    });
  }
});
poll.Start();

Console.WriteLine("Press any key to stop");
Console.ReadKey();

cancellationTokenSource.Dispose();
pubSub.Dispose();
loggerFactory.Dispose();
