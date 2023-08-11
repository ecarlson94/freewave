using EventsTakeTheWheel.Domain.Values;

namespace EventsTakeTheWheel.Domain;

public class ChargePollMessage {
  public required SerialNumber Device { get; set; }
  public required Kilowatts CurrentCharge { get; set; }
}
