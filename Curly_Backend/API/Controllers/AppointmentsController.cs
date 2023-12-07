using API.Extensions.DTOs.Appointments;
using API.Global;
using API.Models.Appointments;
using BLL.Services.Appointments;
using Core;
using Infrustructure.DTOs;
using Infrustructure.ErrorHandling.Exceptions.Appointments;
using Infrustructure.Extensions;
using Infrustructure.Services.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvalidDataException = System.IO.InvalidDataException;

namespace API.Controllers;

[Route("appointments")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentsController> _logger;
    private readonly UserManager<Client> _clientManager;
    private readonly EmailSender _emailSender;

    public AppointmentsController(
        IAppointmentService appointmentService, 
        ILogger<AppointmentsController> logger,
        UserManager<Client> clientManager, EmailSender emailSender)
    {
        _appointmentService = appointmentService;
        _logger = logger;
        _clientManager = clientManager;
        _emailSender = emailSender;
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        var email = Request.GetMemberEmail();
        var role = Request.GetMemberRole();

        var appointments=        Enumerable.Empty<Appointment>();
        var appointmentDtos=  Enumerable.Empty<AppointmentDTO>();

        if (role == Roles.Admin)
        {
            appointments = await _appointmentService.GetAll().ToListAsync();
            appointmentDtos = appointments.Select(app => app.ToAppointmentDto());
        }
        else
        {
            var client = await _clientManager.FindByEmailAsync(email);
            appointments = await _appointmentService.GetOfClient(client.Id).ToListAsync();
            appointmentDtos = appointments.Select(app => app.ToAppointmentDto());
        }
        
        _logger.LogInformation("{Role} {Email} has retrieved {AppointmentCount} appointments successfully", role, email, appointmentDtos.Count());
        
        return Ok(appointmentDtos);
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Client}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var memberEmail = Request.GetMemberEmail();
        var memberRole = Request.GetMemberRole();
        Appointment? appointment = null;

        try
        {
            if (memberRole == Roles.Admin)
            {
                appointment = await _appointmentService.Get(id);
            }
            else
            {
                var client = await _clientManager.FindByEmailAsync(memberEmail);
                appointment = await _appointmentService.GetOfClientSpecific(client.Id, id);
            }

            _logger.LogInformation(
                "{Role} {Email} has retrieved appointment with id {AppointmentId}", 
                memberRole, memberEmail, appointment.Id);

            return Ok(appointment);
        }
        catch (AppointmentNotFoundException ex)
        {
            _logger.LogError(
                "{Role} {Email} could not fetch appointment with id {AppointmentId}, reason: {Message}", 
                memberRole, memberEmail, id, ex.Message);
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = Roles.Client)]
    public async Task<IActionResult> Post([FromBody] CreateAppointmentRequest request)
    {
        var email = Request.GetMemberEmail();
        var client = await _clientManager.FindByEmailAsync(email);

        try
        {
            var newAppointment = await _appointmentService.Create(
                barberId: request.BarberId,
                clientId: client.Id,
                at: DateTime.Parse(request.At) >= DateTime.Now
                    ? DateTime.Parse(request.At)
                    : throw new InvalidOperationException("invalid date and time"),
                serviceIds: request.ServiceIds.ToArray());
            
            _logger.LogInformation(
                "Client {Email} has created an appointment with barber: {BarberEmail}", 
                client.Email, newAppointment.Barber.Email);

            var body =
                $"<!DOCTYPE html>\n<html lang=\"en\">\n\n<head>\n    <meta charset=\"UTF-8\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <link href=\"https://fonts.googleapis.com/css2?family=Nunito:wght@400;700&display=swap\" rel=\"stylesheet\">\n    <title>BarberShop Appointment Confirmation</title>\n    <style>\n        body {{\n            font-family: 'Nunito', sans-serif;\n            background-color: #363636;\n            margin: 0;\n            padding: 0;\n        }}\n\n        .closing-message {{\n            color: #007bff;\n            font-size: 14px;\n            margin-top: 20px;\n            font-style: italic;\n        }}\n\n        .container {{\n            max-width: 600px;\n            margin: 40px auto;\n            padding: 30px;\n            background-color: #1c1c1c;\n            border-radius: 10px;\n            box-shadow: 0 0 15px rgba(0, 0, 0, 0.3);\n            color: #007bff;\n        }}\n\n        h1 {{\n            color: #007bff;\n            text-align: center;\n        }}\n\n        p {{\n            color: whitesmoke;\n            text-align: justify;\n            margin-bottom: 20px;\n        }}\n\n        .reset-button {{\n            display: inline-block;\n            margin-top: 30px;\n            margin-bottom: 30px;\n            padding: 15px 25px;\n            font-size: 18px;\n            text-align: center;\n            text-decoration: none;\n            border-radius: 8px;\n            background-color: #007bff;\n            color: #ffffff;\n            cursor: pointer;\n        }}\n\n        .reset-button:hover {{\n            background-color: #0056b3;\n        }}\n    </style>\n</head>\n\n<body>\n    <div class=\"container\">\n        <h1>Appointment Confirmation</h1>\n        <p>\n            Dear {client.FirstName} {client.LastName},\n            <br><br>\n            We are excited to confirm your upcoming appointment with CurlyHairs! Your satisfaction is our top priority,\n            and we can't wait for your visit!.\n            <br><br>\n            Appointment Details:\n            <br>\n            Date: {newAppointment.At.Date:dddd, dd MMMM yyyy}\n            <br>\n            Time: {newAppointment.At:hh:mm tt}\n            <br>\n            Barber: {newAppointment.Barber.FirstName} {newAppointment.Barber.LastName}\n        </p>\n        <div style=\"text-align: center;\">\n            <a class=\"reset-button\" href=\"#\">View in account</a>\n        </div>\n        <p>\n            If you need to reschedule or have any questions, please contact our support team. Your experience matters to\n            us, and we're here to ensure your visit is seamless.\n            <br><br>\n        </p>\n        <p>Best regards,<br>The CurlyHairs Team</p>\n\n        <p class=\"closing-message\">\n            <i>\n                Thank you for choosing CurlyHairs – where style meets relaxation. If you encounter any issues or have\n                questions, feel free to reach out to our support team at curlysupport@gmail.com.\n            </i>\n        </p>\n    </div>\n</body>\n\n</html>\n";
            await _emailSender.SendHtmlEmail(client.Email, body);
            return Created($"appointments/{newAppointment.Id}", newAppointment.ToAppointmentDto());
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                "{Email} failed to create appointment: {Message}", 
                client.Email, ex.Message);
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPatch]
    [Route("cancel/{id}")]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Cancel(int id)
    {
        var memberRole = Request.GetMemberRole();
        var memberEmail = Request.GetMemberEmail();
        try
        {
            await _appointmentService.Cancel(id);

            return Ok();
        }
        catch (InvalidDataException e)
        {
            _logger.LogError(
                "{Role} {Email} could not cancel appointment with id {AppointmentId}, reason: {Message}", 
                memberRole, memberEmail, e.Message);
            return BadRequest(e.Message);
        }
    }
}