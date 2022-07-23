using System.Reflection;
using System.Threading.Tasks;
using Api.Controllers;
using Api.MapperProfiles;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Server.Core.Interfaces.Repository;
using Server.Core.Interfaces.Service;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;
using ServerTests.Helpers;
using Xunit;

namespace ServerTests.Controllers;

public class AuthControllerTests {
    
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => {
        cfg.AddProfile<RefreshTokenRequestModelMap>();
    }));
    
    private readonly Mock<ITokenService> _mockTokenService = new();
    private readonly Mock<IUserService> _mockUserService = new();
    private readonly Mock<IAuthService> _mockAuthService = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly Mock<ILogger<AuthController>> _mockLogger = new();

    private AuthController _sut;

    private MockHelper _mockHelper = MockHelper.Instance;

    // before each test
    private void ResetMocks() {
        _mockTokenService.Reset();
        _mockUserService.Reset();
        _mockAuthService.Reset();
        _mockLogger.Reset();
    }

    private void SetSut() {
        _sut = new AuthController(_mockLogger.Object, _mockAuthService.Object, _mockTokenService.Object, _mapper);
    }
    
    [Fact(DisplayName = "Login() - Success"), Trait("AuthController", "Login")]
    public async Task Login_ReturnsOkWithToken() {
        ResetMocks();
        
        MockHelper.AuthService.Login.ReturnsUser(_mockAuthService);
        MockHelper.TokenService.GetToken.ReturnsToken(_mockTokenService);

        SetSut();

        var loginRequest = TestModelHelper.LoginRequest;
        var result = await _sut.Login(loginRequest);

        _mockAuthService.Verify(x => x.Login(
                It.Is<string>(emailParam => emailParam.Equals(TestModelHelper.LoginRequest.EmailAddress)),
                It.Is<string>(passwordParam => passwordParam.Equals(TestModelHelper.LoginRequest.Password))),
            Times.Once);
        _mockTokenService.Verify(x => x.GetToken<TokenDto>(It.IsAny<User>(), It.IsAny<string?>()), Times.Once);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var token = okResult.Value.Should().BeOfType<Task<TokenDto>>().Subject;
        token.Result.AccessToken.Should().NotBeNullOrEmpty();
        token.Result.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact(DisplayName = "Login() - Failure"), Trait("AuthController", "Login")]
    public async Task Login_ReturnsUnauthorized_WhenUserDoesNotExist() {
        ResetMocks();
        
        MockHelper.AuthService.Login.ReturnsNull(_mockAuthService);

        SetSut();
        var result = await _sut.Login(TestModelHelper.LoginRequest);

        _mockAuthService.Verify(x => x.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact(DisplayName = "Login() - Exception"), Trait("AuthController", "Login")]
    public async Task Login_ReturnsBadRequest_WhenErrorIsThrown() {
        ResetMocks();
        
        MockHelper.AuthService.Login.ThrowsException(_mockAuthService);

        SetSut();
        var result = await _sut.Login(TestModelHelper.LoginRequest);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact(DisplayName = "RefreshToken() - Success"), Trait("AuthController", "RefreshToken")]
    public async Task RefreshToken_ReturnsOkWithToken() {
        ResetMocks();
        
        MockHelper.TokenService.RefreshToken.ReturnsToken(_mockTokenService);

        SetSut();
        var result = await _sut.RefreshToken(TestModelHelper.RefreshTokenRequest);

        _mockTokenService.Verify(x => x.RefreshToken(It.Is<TokenDto>(token =>
                token.AccessToken.Equals(TestModelHelper.RefreshTokenRequest.AccessToken) &&
                token.RefreshToken.Equals(TestModelHelper.RefreshTokenRequest.RefreshToken))),
            Times.Once);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var token = Assert.IsType<Task<TokenDto>>(okResult.Value);
        token.Result.AccessToken.Should().Be(TestModelHelper.RefreshedToken.AccessToken);
        token.Result.RefreshToken.Should().Be(TestModelHelper.RefreshedToken.RefreshToken);
    }
    
    [Fact(DisplayName = "RefreshToken() - Exception - InvalidToken"), Trait("AuthController", "RefreshToken")]
    public async Task RefreshToken_ReturnsUnauthorized_WhenTokenIsInvalid() {
        ResetMocks();
        
        MockHelper.TokenService.RefreshToken.ThrowsInvalidTokenException(_mockTokenService);

        SetSut();
        var result = await _sut.RefreshToken(TestModelHelper.RefreshTokenRequest);

        _mockTokenService.Verify(x => x.RefreshToken(It.IsAny<TokenDto>()), Times.Once);
        var response = result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
        response.Value.Should().BeEquivalentTo("Invalid token");
    }
    
    [Fact(DisplayName = "RefreshToken() - Exception - TokenStillValid"), Trait("AuthController", "RefreshToken")]
    public async Task RefreshToken_Ok_WhenTokenIsExpired() {
        ResetMocks();
        
        MockHelper.TokenService.RefreshToken.ThrowsTokenStillValid(_mockTokenService);

        SetSut();
        var result = await _sut.RefreshToken(TestModelHelper.RefreshTokenRequest);

        _mockTokenService.Verify(x => x.RefreshToken(It.IsAny<TokenDto>()), Times.Once);
        var response = Assert.IsType<OkObjectResult>(result);
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Value.Should().BeEquivalentTo("Token still valid");
    }
    
    [Fact(DisplayName = "RefreshToken() - Exception - ExceptionCatchAll"), Trait("AuthController", "RefreshToken")]
    public async Task RefreshToken_ReturnsBadRequest_WhenExceptionIsThrown() {
        ResetMocks();
        
        MockHelper.TokenService.RefreshToken.ThrowsException(_mockTokenService);

        SetSut();
        var result = await _sut.RefreshToken(TestModelHelper.RefreshTokenRequest);

        _mockTokenService.Verify(x => x.RefreshToken(It.IsAny<TokenDto>()), Times.Once);
        result.Should().BeOfType<BadRequestResult>();
    }
}
