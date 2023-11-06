using BLL.Services.Reviews;
using BLL.Services.Users;
using Core;
using Infrustructure.DTOs;
using Infrustructure.DTOs.Barbers;
using Infrustructure.Extensions.Barbers;
using Infrustructure.Extensions.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models.Barbers;
using Presentation.Models.PageHandling;
using Exception = System.Exception;

namespace Presentation.Controllers;

[Route("barbers")]
public class BarbersController : ControllerBase
{
    private readonly IUserReader<Barber> _barberReader;
    private readonly ReviewService _reviewService;
    private readonly BarberService _barberService;
    private readonly ILogger<BarbersController> _logger;

    public BarbersController(
        IUserReader<Barber> barberReader,
        BarberService barberService,
        ILogger<BarbersController> logger, ReviewService reviewService)
    {
        _barberReader = barberReader;
        _barberService = barberService;
        _logger = logger;
        _reviewService = reviewService;
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

            _logger.LogInformation("{Role} {Email} posted a commentary to barber {BarberEmail}", nameof(Client),
                memberEmail, commmentary.BarberEmail);

            return Ok(barber.ToBarberWithReviewsDto(
                barber.Reviews?.ToList() ?? new()));
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

        return Ok(barbers.Select(b => 
            b.ToBarberWithReviewsDto(b.Reviews?.ToList() ?? new())));
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

        return Ok(barbers.Select(b => 
            b.ToBarberWithReviewsDto(b.Reviews?.ToList() ?? new())));
    }
    
}