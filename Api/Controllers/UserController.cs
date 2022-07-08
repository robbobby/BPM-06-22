using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers; 

[Route("api/[controller]/[action]")]
public class UserController : ControllerBase {
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService) {
        _logger = logger;
        _userService = userService;
    }
    
    [Authorize(Policy = "User")]
    [HttpGet]
    public async Task<IActionResult> User() {
        var token = Request.Headers.Authorization;
        _logger.LogInformation($"User token: {token}");
        return Ok();
    }
    
    [Authorize(Policy = "Admin")]
    [HttpGet]
    public async Task<IActionResult> AdminProtectedUser() {
        var token = Request.Headers.Authorization;
        _logger.LogInformation($"User token: {token}");
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create(UserRequest user) {
        var token = await _userService.CreateUser(user);
        return Ok(token);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUser(string token) {
        var user = _userService.ValidateToken(token);
        return Ok(user);
    }
}

public class UserRequest {
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
}

