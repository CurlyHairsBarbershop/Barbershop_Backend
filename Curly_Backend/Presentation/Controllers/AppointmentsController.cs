using BLL.Services.Appointments;
using Core;
using Infrustructure.DTOs.Barbers;
using Infrustructure.Extensions.Appointments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models.Appointments;

namespace Presentation.Controllers;

[Route("appointments")]
[Authorize(Roles = nameof(Client))]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentsController> _logger;
    private readonly UserManager<Client> _clientManager;

    public AppointmentsController(
        IAppointmentService appointmentService, 
        ILogger<AppointmentsController> logger,
        UserManager<Client> clientManager)
    {
        _appointmentService = appointmentService;
        _logger = logger;
        _clientManager = clientManager;
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        var email = HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty;
        
        var client = await _clientManager.FindByEmailAsync(email);

        if (client is null)
        {
            return Unauthorized();
        }

        var appointments = await _appointmentService.Get(client.Id).ToListAsync();
        var appointmentDtos = appointments.Select(app => app.ToAppointmentDto());
        
        return Ok(appointmentDtos);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var email = HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty;
        
        var client = await _clientManager.FindByEmailAsync(email);

        if (client is null)
        {
            return Unauthorized();
        }

        var appointment = await _appointmentService.GetById(id);

        return Ok(appointment.ToAppointmentDto());
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Post([FromBody] CreateAppointmentRequest request)
    {
        var email = HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty;
        
        var client = await _clientManager.FindByEmailAsync(email);

        if (client is null)
        {
            return Unauthorized();
        }

        try
        {
            var newAppointment = await _appointmentService.Create(
                barberId: request.BarberId,
                clientId: client.Id,
                at: request.At,
                serviceIds: request.ServiceIds.ToArray());
            
            _logger.LogInformation(
                "Client {Email} has created an appointment with barber: {BarberEmail}", 
                client.Email, newAppointment.Barber.Email);
            return Accepted(newAppointment.ToAppointmentDto());
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                "{Email} failed to create appointment: {Message}", 
                client.Email, ex.Message);
            return BadRequest(ex.Message);
        }
    }
}