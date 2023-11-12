using Core;
using Infrustructure.DTOs;
using Infrustructure.DTOs.Barbers;
using Infrustructure.Extensions.Barbers;

namespace Infrustructure.Extensions.Appointments;

public static partial class AppointmentExtensions
{
    public static AppointmentDTO ToAppointmentDto(this Appointment appointment)
    {
        return new AppointmentDTO
        {
            Id = appointment.Id,
            At = appointment.At,
            Barber = appointment.Barber.ToBarberDto(),
            Favors = appointment.Favors?.Select(f => new FavorDTO
            {
                Id = f.Id,
                Name = f.Name,
                Cost = f.Cost
            }).ToList() ?? new()
        };
    }
}