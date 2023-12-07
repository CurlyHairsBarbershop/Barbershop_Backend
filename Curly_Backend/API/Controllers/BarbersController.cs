using API.Extensions.DTOs.Barbers;
using API.Global;
using API.Models.Barbers;
using API.Models.PageHandling;
using API.Services.AuthService;
using BLL.Services.Users;
using BLL.Services.Users.Barbers;
using Core;
using Infrustructure.ErrorHandling.Exceptions.Barbers;
using Infrustructure.Extensions;
using Infrustructure.Helpers;
using Infrustructure.Services;
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
    private readonly UserManager<Barber> _barberManager;
    private readonly UserManager<Client> _clientManager;
    private readonly BarberService _barberService;
    private readonly IAuthService<Barber> _barberAuthService;
    private readonly ILogger<BarbersController> _logger;
    private readonly BarberMediaService _barberMediaService;

    public BarbersController(
        IUserReader<Barber> barberReader,
        UserManager<Barber> barberManager,
        BarberService barberService,
        ILogger<BarbersController> logger, 
        IAuthService<Barber> barberAuthService, 
        UserManager<Client> clientManager)
    {
        _barberReader = barberReader;
        _barberManager = barberManager;
        _barberService = barberService;
        _logger = logger;
        _barberAuthService = barberAuthService;
        _clientManager = clientManager;
    }

    [HttpPatch]
    [Route("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> EditBarber([FromRoute] int id, [FromBody] EditBarberRequest request)
    {
        var memberEmail = Request.GetMemberEmail();
        
        try
        {
            var updatedBarber = await _barberService.Update(id, request.Name, request.LastName, request.Email, request.Image);
            
            _logger.LogInformation("{Role} {Email} has edited barber with id {BarberId}", Roles.Admin, memberEmail, updatedBarber.Id);

            return Ok($"Barber with id {id} updated");
        }
        catch (BarberNotFoundException ex)
        {
            _logger.LogError("{Role} {Email} could not edited barber with id {BarberId}", Roles.Admin, memberEmail, id);
            
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    [Route("favourite/{id:int}")]
    [Authorize(Roles = Roles.Client)]
    public async Task<IActionResult> DeleteFavouriteBarber(int id)
    {
        var memberEmail = Request.GetMemberEmail();
        var client = await _clientManager.FindByEmailAsync(memberEmail);

        try
        {
            await _barberService.DeleteFavouriteBarber(client, id);

            _logger.LogInformation(
                "{Role} {Email} deleted favourite barber with id {barberId}", 
                nameof(Client), memberEmail, id);
            return Ok("barber has been deleted from favourites");
        }
        catch (BarberNotFoundException ex)
        {
            _logger.LogError(
                "{Role} {Email} could not delete favourite barber, reason: {Message}", 
                nameof(Client), memberEmail, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch]
    [Route("favourite/{id:int}")]
    [Authorize(Roles = Roles.Client)]
    public async Task<IActionResult> AddFavouriteBarber(int id)
    {
        var memberEmail = Request.GetMemberEmail();
        var client = await _clientManager.FindByEmailAsync(memberEmail);

        try
        {
            await _barberService.AddFavouriteBarber(client, id);

            _logger.LogInformation(
                "{Role} {Email} added favourite barber with id {barberId}", 
                nameof(Client), memberEmail, id);
            return Ok("barber has been added to favourites");
        }
        catch (Exception ex) when(ex is BarberNotFoundException or FavouriteBarberAlreadyAddedException)
        {
            _logger.LogError(
                "{Role} {Email} could not add favourite barber, reason: {Message}", 
                nameof(Client), memberEmail, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
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
    
    [HttpPost]
    [Route("")]
    [Authorize(Roles = Roles.Admin)]
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
    [Authorize(Roles = Roles.Client)]
    public async Task<IActionResult> PostReview([FromBody] BarberCommentary commentary)
    {
        var memberEmail = Request.GetMemberEmail();
        var memberRole = Request.GetMemberRole();
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
                memberRole, memberEmail, commentary.BarberEmail);

            return Ok(barber.ToBarberWithReviewsDto());
        }
        catch (InvalidDataException ex)
        {
            _logger.LogError(
                "{Role} {Email} could not post a commentary to barber {BarberEmail}", 
                memberRole, memberEmail, commentary.BarberEmail);
            
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
    [Route("reply/{id:int}")]
    [Authorize]
    public async Task<IActionResult> PostReply(
        [FromRoute] int id, 
        [FromBody] PostReplyModel replyModel)
    {
        var memberEmail = Request.GetMemberEmail();
        var memberRole = Request.GetMemberRole();
        
        try
        {
            var reply = await _barberService.PostReply(
                parentId: id,
                content: replyModel.Content,
                memberId: id);
            _logger.LogInformation(
                "{Role} {Email} posted a reply {ReplyId} to comment with id {CommentId}", 
                memberRole, memberEmail, reply.Id, id);
            return Ok($"reply {reply.Id} has been posted successfully");
        }
        catch (Exception ex) when(ex is InvalidDataException or InvalidOperationException)
        {
            _logger.LogError("{Role} {Email} could not post a reply to comment with id {CommentId}, reason: {Message}", 
                memberRole, memberEmail, id, ex.Message);
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

        if (!barbers.Any()) return NoContent();

        return Ok(barbers
            .Select(b => b.ToBarberWithReviewsDto())
            .OrderBy(b => b.Rating));
    }

    [HttpGet]
    [Route("{id}/schedule")]
    public async Task<IActionResult> Get(
        int id,
        [FromQuery] int daysAhead = 1) 
    {
        var currentUserEmail = Request.GetMemberEmail();

        try
        {
            var busyHours = await _barberService.GetVacantHours(id, daysAhead);

            if (busyHours.Count == 0) return NoContent();

            return Ok(busyHours);
        }
        catch (BarberNotFoundException ex)
        {
            _logger.LogError("{Email} could not fetch vacant dates: {Message}",
                currentUserEmail, ex.Message);

            return BadRequest(ex.Message);
        }
    }
}