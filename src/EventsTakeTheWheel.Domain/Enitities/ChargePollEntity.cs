using EventsTakeTheWheel.Domain.Values;
using MongoDB.Entities;

namespace EventsTakeTheWheel.Domain.Enitities;

public class ChargePollEntity : Entity
{
    public SerialNumber DeviceSerialNumber { get; set; } = new();
    public Kilowatts Kilowatts { get; set; } = new();
    public DateTimeOffset TriggeredAt { get; set; } = DateTimeOffset.UtcNow;
}
