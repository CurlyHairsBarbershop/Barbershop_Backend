using System.Collections.Immutable;
using Core;
using DAL.Context;
using Infrustructure.ErrorHandling.Exceptions.Barbers;
using Infrustructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Users.Barbers;

public class BarberService
{
    private readonly ApplicationContext _dbContext;
    private readonly UserManager<Barber> _barberManager;
    private readonly BarberMediaService _barberMediaService;

    public BarberService(ApplicationContext dbContext, UserManager<Barber> barberManager, BarberMediaService barberMediaService)
    {
        _dbContext = dbContext;
        _barberManager = barberManager;
        _barberMediaService = barberMediaService;
    }

    public async Task<Barber> Update(int id, string? name = null, string? lastName = null, string? email = null, string? image = null)
    {
        var barber = await Get(id);
    
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            barber.LastName = lastName;
        }
    
        if (!string.IsNullOrWhiteSpace(name))
        {
            barber.FirstName = name;
        }
    
        if (!string.IsNullOrWhiteSpace(email))
        {
            barber.Email = email;
        }
    
        if (!string.IsNullOrWhiteSpace(image))
        {
            if (image.IsValidBase64(out var bytes))
            {
                await _barberMediaService.SaveProfileImage(bytes, barber.Id.ToString());
            }
            else
            {
                if (_barberMediaService.MediaExists(image))
                {
                    barber.Image = _barberMediaService.GetUrl(image);
                }
            }
        }

        _dbContext.Update(barber);
        await _dbContext.SaveChangesAsync();

        return barber;
    }
    
    public async Task<Barber> Get(int id)
    {
        var barber =  await _dbContext.Barbers.FirstOrDefaultAsync(b => b.Id == id);

        if (barber is null) throw new BarberNotFoundException(id);

        return barber;
    }
    
    public async Task<bool> Delete(int id)
    {
        var barber = await _dbContext.Barbers.FirstOrDefaultAsync(b => b.Id == id);

        if (barber is null)
        {
            throw new BarberNotFoundException(id);
        }

        var result = await _barberManager.DeleteAsync(barber);

        return result.Succeeded;
    }

    
    /// <summary>
    /// Posts a reply to 
    /// </summary>
    /// <param name="to"></param>
    /// <param name="parentId"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public async Task<Review> PostReply(int memberId, int parentId, string content)
    {
        var review = await _dbContext
            .Reviews
            .Include(r => r.Barber)
            .FirstOrDefaultAsync(r => r.Id == parentId);

        var publisher = await _dbContext.Users
            .FirstOrDefaultAsync(c => c.Id == memberId);

        if (publisher is null)
        {
            throw new InvalidOperationException("publisher was invalid");
        }

        if (review is null)
        {
            throw new InvalidDataException("parent was not found");
        }

        _dbContext.Reviews.Add(new Review
        {
            Content = content,
            Rating = -1,
            Title = "REPLY",
            Publisher = publisher,
            Barber = review.Barber,
            ReplyTo = review.Id
        });

        await _dbContext.SaveChangesAsync();

        return review;
    }

    public async Task<Barber> AddComment(string barberEmail, string clientEmail, string title, string content,
        int rating)
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

    public async Task<ImmutableList<DateTime>> GetVacantHours(
        int barberId,
        int daysAhead)
    {
        var barber = await _dbContext.Barbers
            .Include(barber => barber.Appointments)
            .FirstOrDefaultAsync(b => b.Id == barberId);

        if (barber is null)
        {
            throw new BarberNotFoundException(barberId);
        }

        var hoursGroups = barber.Appointments?
            .GroupBy(a => a.At.Date);
        var hours = hoursGroups.Where(day =>
                day.Key >= DateTime.Now.Date &&
                day.Any(appointment => appointment.At.AddHours(1) > DateTime.Now))
            .Take(daysAhead)
            .SelectMany(d => d.Select(v => v.At))
            .ToImmutableList();

        return hours;
    }
    
    public async Task<bool> AddFavouriteBarber(Client client, int favouriteBarberId)
    {
        var barber = await _dbContext.Barbers.FirstOrDefaultAsync(b => b.Id == favouriteBarberId);

        if (barber is null) throw new BarberNotFoundException(favouriteBarberId);

        client.FavouriteBarbers ??= new List<Barber>();

        if (client.FavouriteBarbers.Any(fb => fb.Id == favouriteBarberId))
            throw new FavouriteBarberAlreadyAddedException(favouriteBarberId);
        
        client.FavouriteBarbers.Add(barber);

        var saveResult = await _dbContext.SaveChangesAsync();

        return saveResult > 0;
    }
    
    public async Task<ImmutableList<Barber>> GetFavouriteBarbers(int clientId)
    {
        var barbers = (await _dbContext.Clients
            .Include(c => c.FavouriteBarbers)
            .FirstOrDefaultAsync(c => c.Id == clientId))
            .FavouriteBarbers
            .ToImmutableList();

        return barbers;
    }

    public async Task<bool> DeleteFavouriteBarber(Client client, int id)
    {
        var dbClient = await _dbContext.Clients
            .Include(c => c.FavouriteBarbers)
            .FirstOrDefaultAsync(c => c.Id == client.Id);
        var barber = dbClient.FavouriteBarbers.FirstOrDefault(b => b.Id == id);
        dbClient.FavouriteBarbers.Remove(barber);

        var result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }
}