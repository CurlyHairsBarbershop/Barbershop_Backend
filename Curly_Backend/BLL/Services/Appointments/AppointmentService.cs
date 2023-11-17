using Core;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Appointments;

public class AppointmentService : IAppointmentService
{
    private readonly ApplicationContext _dbContext;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(
        ApplicationContext dbContext, 
        ILogger<AppointmentService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IQueryable<Appointment> Get(int clientId)
    {
        return _dbContext.Appointments
            .Include(a => a.Client)
            .Include(a => a.Barber)
            .Where(a => a.Client.Id == clientId);
    }

    public async Task<Appointment> GetById(int id)
    {
        return await _dbContext.Appointments.FirstOrDefaultAsync(a => a.Id == id) 
               ?? throw new InvalidDataException($"Appointment with id {id} does not exist");
    }

    public async Task<Appointment> Create(int barberId, int clientId, DateTime at, params int[] serviceIds)
    {
        var barber = await _dbContext.Barbers
                         .Include(barber => barber.Appointments)
                         .FirstOrDefaultAsync(b => b.Id == barberId) 
                     ?? throw new InvalidOperationException($"barber with id {barberId} does not exist");
        var client = await _dbContext.Clients
                         .FirstOrDefaultAsync(c => c.Id == clientId) 
                     ?? throw new InvalidOperationException($"client with id {barberId} does not exist");
        var favours = await _dbContext.Favors
            .Where(f => serviceIds.Contains(f.Id)).ToListAsync();
        
        var timeTaken = (barber.Appointments ?? new List<Appointment>())
            .Any(a => 
                a.At >= DateTime.Now && 
                at > a.At && 
                at < a.At.AddHours(1));
        
        if (timeTaken)
        {
            throw new InvalidOperationException("Appointment date and time is already taken");
        }

        var appointment = new Appointment
        {
            Client = client,
            Barber = barber,
            At = at,
            Favors = favours,
        };

        var newAppointment = (await _dbContext.AddAsync(appointment)).Entity;

        await _dbContext.SaveChangesAsync();

        return newAppointment;
    }
}