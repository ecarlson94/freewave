using EventsTakeTheWheel.Domain.Values;

namespace EventsTakeTheWheel.Domain.Messages;

public class ChargePollMessage {
  public required SerialNumber Device { get; set; }
  public required Kilowatts CurrentCharge { get; set; }
}
