using System.Collections.Immutable;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Favor;

public class FavorService
{
    private readonly ApplicationContext _dbContext;

    public FavorService(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Core.Favor> Get()
    {
        return _dbContext.Favors;
    }

    public async Task<Core.Favor> Get(int id)
    {
        var favor = await _dbContext.Favors.FirstOrDefaultAsync(f => f.Id == id);

        if (favor is null)
        {
            throw new InvalidDataException($"favor with id {id} was not found");
        }

        return favor;
    }

    public async Task<ImmutableList<Core.Favor>> ReadonlyNoTrackingList()
    {
        return (await _dbContext.Favors
                .AsNoTracking()
                .ToListAsync())
            .ToImmutableList();
    }
}