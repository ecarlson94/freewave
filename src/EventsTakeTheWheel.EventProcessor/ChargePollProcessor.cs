using EventsTakeTheWheel.Domain.Messages;
using EventsTakeTheWheel.Infrastructure.Data.Redis;

namespace EventsTakeTheWheel.EventProcessor;

public class ChargePollProcessor : BackgroundService
{
    private readonly ILogger<ChargePollProcessor> _logger;
    private readonly IPubSubServer<ChargePollMessage> _pubSub;

    public ChargePollProcessor(
        ILogger<ChargePollProcessor> logger,
        IPubSubServer<ChargePollMessage> pubSub
    )
    {
        _logger = logger;
        _pubSub = pubSub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ChargePollProcessor is starting.");

        _pubSub.RegisterHandler(OnNewMessageReceived);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        _pubSub.DeregisterHanlder(OnNewMessageReceived);

        _logger.LogInformation("ChargePollProcessor is stopping.");
    }

    private Task OnNewMessageReceived(ChargePollMessage message, DateTime triggerredAt)
    {
        _logger.LogInformation(
            $"Received {message.CurrentCharge.Value} kilowatts for: {message.Device.Value}"
        );
        return Task.CompletedTask;
    }
}
