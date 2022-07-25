using Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Aggregates;
using Server.Core.Exceptions;
using Server.Core.Interfaces.Service;

namespace Api.Controllers;

[Route("api/[controller]/[action]")]
public class UserController : ControllerBase {
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(IUserService userService, ILogger<UserController> logger) {
        _logger = logger;
        _userService = userService;
    }

    [Authorize(Policy = "User")]
    [HttpGet]
    public Task<IActionResult> GetUserAccounts() {
        try {
            _logger.LogError("GetUserAccounts");
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = TokenHelper.GetUserIdFromToken(token);
            var accountIds = _userService.GetUserAccounts(userId);
            return Task.FromResult<IActionResult>(Ok(accountIds.Result));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error in GetAllUserAccountIds");
            return Task.FromResult<IActionResult>(BadRequest("Internal server error"));
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public Task<IActionResult> Register([FromBody] UserRequest user) {
        try {
            var token = _userService.CreateUser(user);
            return Task.FromResult<IActionResult>(Ok(token.Result));
        } catch (UserAlreadyExistsException ex) {
            return Task.FromResult<IActionResult>(Conflict("Email already registered"));
        } catch (Exception ex) {
            _logger.LogError(ex.Message);
            return Task.FromResult<IActionResult>(StatusCode(500, "Internal server error"));
        }
    }
}