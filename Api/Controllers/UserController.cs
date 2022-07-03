using Api.Models;
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
    public async Task<IActionResult> Create() {
        return Ok();
    }
}

public interface IUserService {
    User GetUser();
}
public class UserService : IUserService {
    public User GetUser() { return new User(); }
}
