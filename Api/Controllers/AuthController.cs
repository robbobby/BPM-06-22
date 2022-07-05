using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers; 

public class AuthController : ControllerBase {
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;
    private readonly IConfiguration _config;

    public AuthController(ILogger<AuthController> logger, IAuthService authService, ITokenService tokenService) {
        _tokenService = tokenService;
        _logger = logger;
        _authService = authService;
    }
    
    [HttpPost("api/auth/login")]
    public async Task<IActionResult> Login([FromBody] ILoginRequestModel loginRequestModel) {
        try {
            var user = await _authService.Login(loginRequestModel.Email, loginRequestModel.Password);
            _logger.LogInformation($"User {user?.EmailAddress} logged in");
            
            if (user == null) 
                return Unauthorized();
            
            
            string token = await _tokenService.GenerateToken(user);
            _logger.LogInformation($"Token generated for user {user?.EmailAddress}");
            _logger.LogInformation($"Token: {token}");
            return Ok(user);
        } catch (Exception ex) {
            _logger.LogError($"Exception: {ex}");
            return BadRequest();
        }
    }
}

public class ILoginRequestModel {
    public string Email { get; set; }
    public string Password { get; set; }
}