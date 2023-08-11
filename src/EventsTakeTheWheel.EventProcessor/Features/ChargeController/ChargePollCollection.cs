using MongoDB.Entities;

namespace EventsTakeTheWheel.EventProcessor.Features.ChargeController;

public interface IChargePollCollection
{
    Task SaveAsync(ChargePollEntity poll);
}

public class ChargePollCollection : IChargePollCollection
{
    private readonly DBContext _dbContext;

    public ChargePollCollection(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(ChargePollEntity poll) => await _dbContext.SaveAsync(poll);
}
