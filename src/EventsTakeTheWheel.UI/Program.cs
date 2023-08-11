using EventsTakeTheWheel.BFF;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("http://localhost:5244");
var client = new ChargeControllers.ChargeControllersClient(channel);

var cancellationTokenSource = new CancellationTokenSource();
var cancellationToken = cancellationTokenSource.Token;

var listenToStream = new Thread(async () =>
{
    var window = TimeSpan.FromSeconds(10);
    var request = new StreamChargeInfoRequest
    {
        AggregationWindow = Duration.FromTimeSpan(window),
        AggregationCount = 1,
        RefreshInterval = Duration.FromTimeSpan(TimeSpan.FromSeconds(2))
    };

    using var call = client.StreamChargeInfo(request);
    while (await call.ResponseStream.MoveNext(cancellationToken))
    {
        var response = call.ResponseStream.Current;
        if (response.AverageChargeInfos.Count == 0)
        {
            continue;
        }
        Console.WriteLine(
            $"Average charge rate over the last {window.TotalSeconds} seconds: {response.AverageChargeInfos[0].Kilowatts.Value} kilowatts"
        );
    }
});
listenToStream.Start();

Console.WriteLine("Press any key to stop listening to the stream");
Console.ReadKey();
cancellationTokenSource.Cancel();

cancellationTokenSource.Dispose();
