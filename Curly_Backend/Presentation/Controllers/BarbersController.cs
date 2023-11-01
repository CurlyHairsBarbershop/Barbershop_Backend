using BLL.Services.Users;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models.Barbers;
using Presentation.Models.PageHandling;

namespace Presentation.Controllers;

[Route("barbers")]
public class BarbersController : ControllerBase
{
    private readonly IUserReader<Barber> _barberReader;
    private readonly BarberService _barberService;
    private readonly ILogger<BarbersController> _logger;
    
    public BarbersController(
        IUserReader<Barber> barberReader, 
        BarberService barberService, 
        ILogger<BarbersController> logger)
    {
        _barberReader = barberReader;
        _barberService = barberService;
        _logger = logger;
    }

    [HttpPost]
    [Route("comment")]
    [Authorize(Roles = nameof(Client))]
    public async Task<IActionResult> AddComment([FromBody] BarberCommentary commmentary)
    {
        var memberEmail = Request.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty;
        try
        {
            var barber = await _barberService.AddComment(
                barberEmail: commmentary.BarberEmail,
                clientEmail: memberEmail,
                title: commmentary.Title,
                content: commmentary.Content,
                rating: commmentary.Rating);
            
            _logger.LogInformation("{Role} {Email} posted a commentary to barber {BarberEmail}", nameof(Client), memberEmail, commmentary.BarberEmail);
            
            return Accepted(new
            {
                barber.Email,
                barber.FirstName,
                barber.LastName,
                barber.PhoneNumber,
                barber.Earnings,
                barber.Rating,
                barber.Image,
                Reviews = barber.Reviews?.Select(r => new
                {
                    r.Content,
                    r.Rating,
                    Publisher = new { r.Publisher.FirstName, r.Publisher.LastName, r.Publisher.Email }
                })
            });
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet]
    [Route("page")]
    public async Task<IActionResult> GetPaged([FromQuery] QueryPageModel pageReader)
    {
        var barbers = await _barberReader
            .GetPagedAsNoTracking(pageReader.PageNumber, pageReader.PageCount)
            .Include(b => b.Reviews)!
            .ThenInclude(r => r.Publisher)
            .ToListAsync();
        if (!barbers.Any())
        {
            return NoContent();
        }
        
        return Ok(barbers.Select(b => new
        {
            b.Email,
            b.FirstName,
            b.LastName,
            b.PhoneNumber,
            b.Earnings,
            b.Rating,
            b.Image,
            Reviews = b.Reviews?.Select(r => new
            {
                r.Content,
                r.Rating,
                Publisher = new { r.Publisher.FirstName, r.Publisher.LastName, r.Publisher.Email }
            })
        }));
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        var barbers = await _barberReader
            .GetPagedAsNoTracking()
            .Include(b => b.Reviews)!
            .ThenInclude(r => r.Publisher)
            .ToListAsync();

        if (!barbers.Any())
        {
            return NoContent();
        }

        return Ok(barbers.Select(b => new
        {
            b.Email,
            b.FirstName,
            b.LastName,
            b.PhoneNumber,
            b.Earnings,
            b.Rating,
            b.Image,
            Reviews = b.Reviews?.Select(r => new
            {
                r.Content,
                r.Rating,
                Publisher = new { r.Publisher.FirstName, r.Publisher.LastName, r.Publisher.Email }
            })
        }));
    }
}