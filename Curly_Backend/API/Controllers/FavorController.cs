using API.Global;
using API.Models.Favors;
using BLL.Services.Favor;
using Infrustructure.ErrorHandling.Exceptions.Favors;
using Infrustructure.Extensions;
using Microsoft.AspNetCore.Authorization;
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

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var memberEmail = Request.GetMemberEmail();
        
        var favor = await _favorService.Get(id);

        if (favor is null) return NotFound();
        
        await _favorService.Delete(id);

        return Ok("Favor has been deleted successfully");
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateFavorRequest request)
    {
        var memberEmail = Request.GetMemberEmail();
        try
        {
            var created = await _favorService.Create(request.Name, request.Description, request.Cost);

            return Created($"favors/{created.Id}", "Favor created successfully");
        }
        catch (FavorNameTakenException ex)
        {
            _logger.LogWarning(
                "{Role} {Email} could not create favor, reason: {Message}", 
                Roles.Admin, memberEmail, ex.Message);

            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateFavorRequest request)
    {
        var memberEmail = Request.GetMemberEmail();
        try
        {
            await _favorService.Update(id, request.Name, request.Description, request.Cost);

            return Ok($"Favor with id {id} updated successfully");
        }
        catch (FavorNotFoundException ex)
        {
            _logger.LogWarning(
                "{Role} {Email} could not update favor, reason: {Message}", 
                Roles.Admin, memberEmail, ex.Message);

            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get()
    {
        var favors = await _favorService.ReadonlyNoTrackingList();

        if (favors.Count == 0) return NoContent();
        
        return Ok(favors);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
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