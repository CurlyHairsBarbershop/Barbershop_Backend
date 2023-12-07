using System.ComponentModel.DataAnnotations;
using Infrustructure.Attributes.Validation;
using Microsoft.AspNetCore.Mvc;

namespace API.Models.Appointments;

public class CreateAppointmentRequest
{
    [Required]
    [FutureTime(ErrorMessage = "Must be future time")]
    [DataType(DataType.Date)]
    public required string At { get; set; }
    
    [Required]
    public required int BarberId { get; set; }
    
    [NotEmptyCollection(ErrorMessage = "Appointment must have at least one service")]
    [Required]
    public required List<int> ServiceIds { get; set; }
}