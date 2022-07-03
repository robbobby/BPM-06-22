using Api.Interfaces;
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

    [HttpGet]
    public async Task<IActionResult> User() {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRequest user) {
        await _userService.CreateUser(user);
        return Ok();
    }
}

public class UserRequest {
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
}