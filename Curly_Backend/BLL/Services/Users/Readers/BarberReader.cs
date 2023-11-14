using Core;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Users.Readers;

public class BarberReader : IUserReader<Barber>
{
    private readonly ApplicationContext _dbContext;
    
    public BarberReader(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Barber> GetPagedAsNoTracking(int pageNumber = 0, int pageCount = int.MaxValue)
    {
        return _dbContext.Barbers
            .Skip((pageNumber == 0 ? pageNumber : pageNumber - 1) * pageCount)
            .Take(pageCount)
            .OrderBy(b => b.Id)
            .AsNoTracking();
    }
}