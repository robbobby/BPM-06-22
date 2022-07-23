using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Exceptions;
using Server.Core.Interfaces.Service;
using Server.Core.Models;

namespace Api.Controllers; 

[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase {
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService, ITokenService tokenService, IMapper mapper) {
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
        _authService = authService;
    }
    
    [HttpPost()]
    public Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel) {
        try {
            var user = _authService.Login(loginRequestModel.EmailAddress, loginRequestModel.Password);
            if (user.Result == null) 
                return Task.FromResult<IActionResult>(Unauthorized());

            var token = _tokenService.GetToken<TokenDto>(user.Result);
            return Task.FromResult<IActionResult>(Ok(token));
        } catch (Exception ex) {
            _logger.LogError("Exception: {Ex}", ex);
            return Task.FromResult<IActionResult>(BadRequest());
        }
    }
    
    [HttpPost]
    public Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel tokenRefresh) {
        try {
            var tokenMapped = _mapper.Map<TokenDto>(tokenRefresh);
            var token = _tokenService.RefreshToken(tokenMapped);
            return Task.FromResult<IActionResult>(Ok(token));
        } catch (InvalidTokenException) {
            return Task.FromResult<IActionResult>(Unauthorized("Invalid token"));
        }
        catch (TokenStillValidException) {
            return Task.FromResult<IActionResult>(Ok("Token still valid"));
        }
        catch (Exception ex) {
            _logger.LogError("Exception: {Ex}", ex);
            return Task.FromResult<IActionResult>(BadRequest());
        }
    }
}