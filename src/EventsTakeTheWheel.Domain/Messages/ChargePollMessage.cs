using EventsTakeTheWheel.Domain.Values;

namespace EventsTakeTheWheel.Domain.Messages;

public class ChargeControllerChargePollMessage
{
    public required SerialNumber Device { get; set; }
    public required Kilowatts CurrentCharge { get; set; }
}
