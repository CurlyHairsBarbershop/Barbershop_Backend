using Core;

namespace BLL.Services.Appointments;

public interface IAppointmentService
{
    /// <summary>
    /// Get all appointments
    /// </summary>
    /// <returns></returns>
    IQueryable<Appointment> GetAll();

    /// <summary>
    /// Get appointment with id within all appointments without sticking to the client scope
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Appointment> Get(int id);
    
    /// <summary>
    /// Get all appointments of a client with id clientId
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    IQueryable<Appointment> GetOfClient(int clientId);

    /// <summary>
    /// Get appointment with id of client with clientId
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    Task<Appointment> GetOfClientSpecific(int clientId, int id);

    Task<Appointment> Create(int barberId, int clientId, DateTime at, params int[] serviceIds);

    Task<bool> Cancel(int id);
}