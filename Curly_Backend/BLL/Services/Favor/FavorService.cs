using System.Collections.Immutable;
using DAL.Context;
using Infrustructure.ErrorHandling.Exceptions.Favors;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Favor;

public class FavorService
{
    private readonly ApplicationContext _dbContext;

    public FavorService(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Core.Favor> Create(string name, string description, double cost)
    {
        var nameTaken =
            _dbContext.Favors.ToList().Any(f => f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        if (nameTaken) throw new FavorNameTakenException(name);
        
        var favor = _dbContext.Favors.Add(new Core.Favor
        {
            Name = name,
            Description = description,
            Cost = cost
        }).Entity;

        var created = await _dbContext.SaveChangesAsync();

        return favor;
    }

    public async Task<bool> Update(
        int id, 
        string? newName = null, 
        string? newDescription = null, 
        double? newCost = null)
    {
        var favor = await _dbContext.Favors.FirstOrDefaultAsync(f => f.Id == id);

        if (favor is null) throw new FavorNotFoundException(id);

        if (!string.IsNullOrWhiteSpace(favor.Name)) favor.Name = newName;
        if (newDescription is not null) favor.Description = newDescription;
        if (newCost is not null) favor.Cost = (double)newCost;
        
        _dbContext.Update(favor);
        var saved = await _dbContext.SaveChangesAsync();

        return saved > 0;
    }

    public IQueryable<Core.Favor> Get()
    {
        return _dbContext.Favors;
    }

    public async Task<Core.Favor> Get(int id)
    {
        var favor = await _dbContext.Favors.FirstOrDefaultAsync(f => f.Id == id);

        if (favor is null) throw new InvalidDataException($"favor with id {id} was not found");

        return favor;
    }

    public async Task<bool> Delete(int id)
    {
        var delete = await _dbContext.Favors.Where(f => f.Id == id).ExecuteDeleteAsync();

        return delete > 0;
    }

    public async Task<ImmutableList<Core.Favor>> ReadonlyNoTrackingList()
    {
        return (await _dbContext.Favors
                .AsNoTracking()
                .ToListAsync())
            .ToImmutableList();
    }
}