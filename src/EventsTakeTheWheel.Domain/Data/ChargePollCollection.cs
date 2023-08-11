using EventsTakeTheWheel.Domain.Enitities;
using MongoDB.Entities;
using MongoDB.Driver;

namespace EventsTakeTheWheel.Domain.Data;

public interface IChargePollCollection
{
    Task SaveAsync(ChargePollEntity poll);
    Task<IEnumerable<ChargePollEntity>> GetAllAfter(DateTimeOffset dateTime);
}

public class ChargePollCollection : IChargePollCollection
{
    private readonly DBContext _dbContext;

    public ChargePollCollection(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(ChargePollEntity poll) => await _dbContext.SaveAsync(poll);

    public async Task<IEnumerable<ChargePollEntity>> GetAllAfter(DateTimeOffset dateTime)
    {
        var filter = Builders<ChargePollEntity>.Filter.Gt(p => p.TriggeredAt, dateTime);
        return await _dbContext.Find<ChargePollEntity>().Match(filter).ExecuteAsync();
    }
}
