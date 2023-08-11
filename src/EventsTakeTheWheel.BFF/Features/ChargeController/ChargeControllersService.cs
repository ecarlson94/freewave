using EventsTakeTheWheel.Domain.Data;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EventsTakeTheWheel.BFF.Features.ChargeController;

public class ChargeControllersService : ChargeControllers.ChargeControllersBase
{
    private readonly ILogger<ChargeControllersService> _logger;
    private readonly IChargePollCollection _chargePollCollection;

    public ChargeControllersService(
        ILogger<ChargeControllersService> logger,
        IChargePollCollection chargePollCollection
    )
    {
        _logger = logger;
        _chargePollCollection = chargePollCollection;
    }

    public override async Task StreamChargeInfo(
        StreamChargeInfoRequest request,
        IServerStreamWriter<StreamChargeInfoResponse> responseStream,
        ServerCallContext context
    )
    {
        // TODO: Filter by controller ids "associated" with the user
        // TODO: Use mongodb $bucket stage of aggregation pipeline to get averages grouped by time windows
        while (!context.CancellationToken.IsCancellationRequested)
        {
            var aggregationWindow = request.AggregationWindow.ToTimeSpan();
            var collectAfterDate = DateTimeOffset.UtcNow - aggregationWindow;
            var chargePolls = await _chargePollCollection.GetAllAfter(collectAfterDate);
            var averageCharge = chargePolls.Average(p => p.Kilowatts.Value);

            var halfWindowDate = DateTimeOffset.UtcNow - (aggregationWindow / 2);
            var response = new StreamChargeInfoResponse();
            response.AverageChargeInfos.Add(
                new AverageChargeInfo
                {
                    Time = Timestamp.FromDateTimeOffset(halfWindowDate),
                    Kilowatts = new Kilowatt { Value = averageCharge }
                }
            );
            await responseStream.WriteAsync(new());

            Thread.Sleep(request.RefreshInterval.ToTimeSpan().Milliseconds);
        }
    }
}
