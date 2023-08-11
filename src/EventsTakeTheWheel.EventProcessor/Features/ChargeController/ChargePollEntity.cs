using EventsTakeTheWheel.Domain.Values;
using MongoDB.Entities;

namespace EventsTakeTheWheel.EventProcessor.Features.ChargeController;

public class ChargePollEntity : Entity
{
    public SerialNumber DeviceSerialNumber { get; set; } = new();
    public Kilowatts Kilowatts { get; set; } = new();
    public DateTime TriggeredAt { get; set; } = DateTime.UtcNow;
}
