using Microsoft.AspNetCore.Mvc;
using UMS.Application.Common.Interfaces;
using UMS.Application.Common.Models;

namespace UMS.Web.Controllers;

public class UserController(IUserService userService, Serilog.ILogger logger) : BaseController
{
    private readonly IUserService _userSvc = userService;
    private readonly Serilog.ILogger _logger = logger;

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userSvc.GetAll();
            if (!users.Any())
            {
                _logger.Warning("No users found in the database.");
                return NotFound();
            }
            _logger.Information("Returned all users from database.");
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred during the GetAllUsers operation while retrieving users.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        try
        {
            var user = await _userSvc.GetById(id);
            if (user is null)
            {
                _logger.Warning("User with ID {UserId} was not found.", id);
                return NotFound();
            }
            _logger.Information("User with ID {UserId} was retrieved successfully.", id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred during the GetUserById operation while retrieving the user with ID {UserId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] UserDto request)
    {
        try
        {
            await _userSvc.Create(request);
            _logger.Information("User with ID {UserId} created successfully.", request.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while creating a user with ID {UserId}.", request.Id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserDto request)
    {
        try
        {
            var user = await _userSvc.GetById(id);
            if (user is null) return NotFound();
            else
                await _userSvc.Update(request);
            _logger.Information("User with ID {UserId} updated successfully.", request.Id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while updating a user with ID {UserId}.", request.Id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        try
        {
            var response = await _userSvc.GetById(id);
            if (response is null) return NotFound();
            else
                await _userSvc.Delete(id);
            _logger.Information("User with ID {UserId} deleted successfully.", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while deleting a user with ID {UserId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}
