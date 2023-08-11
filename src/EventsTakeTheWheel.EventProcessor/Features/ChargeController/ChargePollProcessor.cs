using EventsTakeTheWheel.Domain.Data;
using EventsTakeTheWheel.Domain.Enitities;
using EventsTakeTheWheel.Domain.Messages;
using EventsTakeTheWheel.Infrastructure.Data.Redis;

namespace EventsTakeTheWheel.EventProcessor.Features.ChargeController;

public class ChargePollProcessor : BackgroundService
{
    private readonly ILogger<ChargePollProcessor> _logger;
    private readonly IPubSubServer<ChargeControllerChargePollMessage> _pubSub;
    private readonly IChargePollCollection _chargePollCollection;

    public ChargePollProcessor(
        ILogger<ChargePollProcessor> logger,
        IPubSubServer<ChargeControllerChargePollMessage> pubSub,
        IChargePollCollection chargePollCollection
    )
    {
        _logger = logger;
        _pubSub = pubSub;
        _chargePollCollection = chargePollCollection;
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

    private async Task OnNewMessageReceived(
        ChargeControllerChargePollMessage message,
        DateTimeOffset triggerredAt
    )
    {
        _logger.LogInformation(
            $"Received {message.CurrentCharge.Value} kilowatts for: {message.Device.Value}"
        );

        var model = new ChargePollEntity
        {
            DeviceSerialNumber = message.Device,
            Kilowatts = message.CurrentCharge,
            TriggeredAt = triggerredAt
        };
        await _chargePollCollection.SaveAsync(model);

        _logger.LogInformation($"Saved to database with id: {model.ID}");
    }
}
