using API.Extensions.DTOs.Barbers;
using API.Models.Admin;
using API.Models.Barbers;
using API.Models.PageHandling;
using API.Services.AuthService;
using BLL.Services.Users;
using Core;
using Infrustructure.ErrorHandling.Exceptions.Barbers;
using Infrustructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exception = System.Exception;
using InvalidDataException = System.IO.InvalidDataException;

namespace API.Controllers;

[Route("barbers")]
public class BarbersController : ControllerBase
{
    private readonly IUserReader<Barber> _barberReader;
    private readonly BarberService _barberService;
    private readonly IAuthService<Barber> _barberAuthService;
    private readonly ILogger<BarbersController> _logger;

    public BarbersController(
        IUserReader<Barber> barberReader,
        BarberService barberService,
        ILogger<BarbersController> logger, 
        IAuthService<Barber> barberAuthService)
    {
        _barberReader = barberReader;
        _barberService = barberService;
        _logger = logger;
        _barberAuthService = barberAuthService;
    }

    [Route("{id:int}")]
    [HttpDelete]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Delete(int id)
    {
        var memberEmail = Request.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty;
        
        try
        {
            var result = await _barberService.Delete(id);

            return result == false
                ? StatusCode(StatusCodes.Status500InternalServerError, "could not delete barber")
                : Ok("barber has been deleted successfully");
        }
        catch (BarberNotFoundException ex)
        {
            _logger.LogError(
                "{Role} {Email} could not delete barber with error: {Message}",
                nameof(Admin), memberEmail, ex.Message);

            return BadRequest(ex.Message);
        }
    }
    
    [Route("")]
    [HttpPost]
    [Authorize(Roles = nameof(Admin))]
    public async Task<IActionResult> Create([FromBody] CreateBarberRequest request)
    {
        var newBarber = new Barber
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Image = request.Image,
        };

        var register = await _barberAuthService.Register(newBarber, IdentityHelper.GeneratePassword());

        return register.Result.Succeeded
            ? Created("/unsupported", "barber has been created successfully")
            : !string.IsNullOrWhiteSpace(register.Error)
                ? BadRequest(register.Error)
                : StatusCode(StatusCodes.Status500InternalServerError, "could not create barber");
    }

    [HttpPost]
    [Route("comment")]
    [Authorize(Roles = nameof(Client))]
    public async Task<IActionResult> AddComment([FromBody] BarberCommentary commentary)
    {
        var memberEmail = Request.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty;
        try
        {
            var barber = await _barberService.AddComment(
                barberEmail: commentary.BarberEmail,
                clientEmail: memberEmail,
                title: commentary.Title,
                content: commentary.Content,
                rating: commentary.Rating);

            _logger.LogInformation(
                "{Role} {Email} posted a commentary to barber {BarberEmail}", 
                nameof(Client), memberEmail, commentary.BarberEmail);

            return Ok(barber.ToBarberWithReviewsDto());
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
        
        if (!barbers.Any()) return NoContent();

        return Ok(barbers.Select(b => b.ToBarberWithReviewsDto()));
    }

    [HttpPost]
    [Route("reply")]
    [Authorize(Roles = nameof(Client))]
    public async Task<IActionResult> PostReply([FromBody] PostReplyModel replyModel)
    {
        try
        {
            var review = await _barberService.PostReplyTo(
                parentId: replyModel.ReviewId,
                content: replyModel.Content,
                clientEmail: Request.HttpContext.User.Claims
                    .FirstOrDefault(c => c.Type == "Email")?.Value ?? string.Empty);
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
            return BadRequest(ex.Message);
        }
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

        return Ok(barbers
            .Select(b =>
                b.ToBarberWithReviewsDto())
            .OrderBy(b =>
                b.Rating));
    }

    [HttpGet]
    [Route("{id}/schedule")]
    public async Task<IActionResult> Get(
        int id,
        [FromQuery] int daysAhead = 1)
    {
        var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value ?? "Anonymous";

        try
        {
            var busyHours = await _barberService.GetVacantHours(id, daysAhead);

            if (busyHours.Count == 0) return NoContent();

            return Ok(busyHours);
        }
        catch (BarberNotFoundException ex)
        {
            _logger.LogError(
                ex,
                "{Email} could not fetch vacant dates: {Message}",
                currentUserEmail, ex.Message);

            return BadRequest(ex.Message);
        }
    }
}