using Core;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Users;

public class BarberService
{
    private readonly ApplicationContext _dbContext;
    
    public BarberService(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Barber> AddComment(string barberEmail, string clientEmail, string title, string content, int rating)
    {
       var barber = await _dbContext.Barbers.FirstOrDefaultAsync(b => b.Email == barberEmail);
       
       if (barber is null)
       {
           throw new InvalidDataException("barber email was invalid");
       }

       var member = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Email == clientEmail);
       if (member is null)
       {
           throw new InvalidDataException("member email was invalid");
       }
       
       barber.Reviews ??= new List<Review>();
       barber.Reviews.Add(new Review
       {
           Rating = rating,
           Title = title,
           Content = content,
           Publisher = member,
           Barber = barber
       });

       await _dbContext.SaveChangesAsync();

       return barber;
    }
}