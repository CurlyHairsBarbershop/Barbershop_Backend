using Core;

namespace BLL.Services.Appointments;

public interface IAppointmentService
{
    IQueryable<Appointment> Get(int clientId);

    Task<Appointment> GetById(int id);

    Task<Appointment> Create(int barberId, int clientId, DateTime at, params int[] serviceIds);
}