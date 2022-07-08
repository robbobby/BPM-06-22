using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers; 

[Route("api/[controller]/[action]")]
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
    
    [HttpPost()]
    public async Task<IActionResult> Login([FromBody] ILoginRequestModel loginRequestModel) {
        try {
            var user = await _authService.Login(loginRequestModel.Email, loginRequestModel.Password);
            _logger.LogInformation("User {UserEmailAddress} logged in", user?.EmailAddress);
            
            if (user == null) 
                return Unauthorized();

            string token = await _tokenService.GenerateToken(user);
            _logger.LogInformation("Token generated for user {UserEmailAddress}", user?.EmailAddress);
            _logger.LogInformation("Token: {Token}", token);
            return Ok(token);
        } catch (Exception ex) {
            _logger.LogError("Exception: {Ex}", ex);
            return BadRequest();
        }
    }
}

public class ILoginRequestModel {
    public string Email { get; set; }
    public string Password { get; set; }
}