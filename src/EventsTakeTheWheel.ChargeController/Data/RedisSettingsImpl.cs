using EventsTakeTheWheel.Infrastructure.Data.Redis;

namespace EventsTakeTheWheel.ChargeController.Data;

public class RedisSettingsImpl : RedisSettings
{
    public override string Endpoint => "localhost";

    public override string Port => "6379";

    public override string Ssl => "false";

    public override string Password => "";
}
