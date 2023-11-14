using BLL.Services.Favor;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("favors")]
public class FavorController : ControllerBase
{
    private readonly FavorService _favorService;
    private readonly ILogger<FavorController> _logger;
    
    public FavorController(
        FavorService favorService, 
        ILogger<FavorController> logger)
    {
        _favorService = favorService;
        _logger = logger;
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        var favors = await _favorService.ReadonlyNoTrackingList();

        if (favors.Count == 0)
        {
            return NoContent();
        }
        
        return Ok(favors);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var currentUserEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value ?? "Anonymous";
        try
        {
            var favor = await _favorService.Get(id);

            return Ok(favor);
        }
        catch (InvalidDataException ex)
        {
            _logger.LogInformation(ex, "error on fetching favor by {Email} {Message}", currentUserEmail, ex.Message);

            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogCritical("unhandled error occured: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }
    }
}