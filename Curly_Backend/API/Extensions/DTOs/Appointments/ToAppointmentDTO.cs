using API.Extensions.DTOs.Barbers;
using Core;
using Infrustructure.DTOs;

namespace API.Extensions.DTOs.Appointments;

public static partial class ToDtoExtensions
{
    public static AppointmentDTO ToAppointmentDto(this Appointment appointment)
    {
        return new AppointmentDTO
        {
            Id = appointment.Id,
            At = appointment.At,
            Barber = appointment.Barber.ToBarberDto(),
            Favors = appointment.Favors.Select(f => new FavorDTO
            {
                Cost = f.Cost,
                Name = f.Name,
                Description = f.Description,
                Id = f.Id
            }).ToList() ?? new()
        };
    }
}