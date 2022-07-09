using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers; 

[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase {
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService, ITokenService tokenService) {
        _tokenService = tokenService;
        _logger = logger;
        _authService = authService;
    }
    
    [HttpPost()]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel) {
        try {
            var user = await _authService.Login(loginRequestModel.EmailAddress, loginRequestModel.Password);
            if (user == null) 
                return Unauthorized();

            var token = await _tokenService.GetToken<TokenDto>(user);
            return Ok(token);
        } catch (Exception ex) {
            _logger.LogError("Exception: {Ex}", ex);
            return BadRequest();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel refreshTokenRequestModel) {
        try {
            var token = await _tokenService.GetRefreshedToken(refreshTokenRequestModel);
            return Ok(token);
        } catch (Exception ex) {
            _logger.LogError("Exception: {Ex}", ex);
            return BadRequest();
        }
    }
}

public class RefreshTokenRequestModel {
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

public class LoginRequestModel {
    public string EmailAddress { get; set; }
    public string Password { get; set; }
}