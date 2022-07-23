using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Api.MapperProfiles;
using Api.Services;
using AutoMapper;
using Domain.Repository;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Server.Core.Exceptions;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;
using ServerTests.Helpers;
using Xunit;

namespace ServerTests.Services;

public class TokenServiceTests {
    private readonly Mock<ILogger<TokenService>> _logger = new Mock<ILogger<TokenService>>();
    private readonly Mock<ITokenRepository> _mockTokenRepository = new Mock<ITokenRepository>();
    private readonly IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

    private readonly Mock<IAccountUserRepository> _mockAccountUserRepository = new Mock<IAccountUserRepository>();

    private TokenService _sut;

    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => {
        cfg.AddProfile<TokenMap>();
        cfg.AddProfile<AccountUserMap>();
    }));

    private void SetSut() {
        _sut = new TokenService(configuration, _mockTokenRepository.Object, _mockAccountUserRepository.Object, _mapper, _logger.Object);
    }

    private void ResetMocks() {
        _logger.Reset();
        _mockTokenRepository.Reset();
    }

    [Fact(DisplayName = "GenerateToken - Success - Should return token"), Trait("TokenService", "GenerateToken")]
    public void GenerateToken_ReturnsValidToken() {
        ResetMocks();
        MockHelper.TokenRepository.Create.Void(_mockTokenRepository);
        MockHelper.TokenRepository.SaveChanges.Void(_mockTokenRepository);

        SetSut();

        var user = TestModelHelper.User;
        user.AccountUsers = TestModelHelper.AccountUsers;
        var result = _sut.GenerateToken(user, TestModelHelper.User.DefaultAccount.ToString());

        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.ReadJwtToken(result.Result.AccessToken);
        token.Issuer.Should().BeEquivalentTo("bmp", "Token issuer should be bmp as this is set in the appsettings.json" );
        token.Audiences.Should().BeEquivalentTo("bmp");

        token.Claims.Should().Contain(c => c.Type == "UserId" && c.Value == user.Id.ToString(), "Token should contain UserId");
        token.Claims.Should().Contain(c => c.Type == "AccountId" && c.Value == user.DefaultAccount.ToString(), "Token should contain AccountId");
        token.Claims.Should().Contain(c => c.Type == "role" && c.Value == "User", "Token should contain a user role for the account");

        var expiryTime = token.Claims.SingleOrDefault(c => c.Type == "exp")?.Value;
        DateTime.UtcNow.AddMinutes(15).Should().BeCloseTo(token.ValidTo, TimeSpan.FromSeconds(3), "Token should expire in 15 minutes");

        _mockTokenRepository.Verify(r => r.Create(It.IsAny<Token>()), Times.Once, "TokenRepository.Create should be called once to create a token");
        _mockTokenRepository.Verify(r => r.SaveChanges(), Times.Once, "TokenRepository.SaveChanges should be called once to save the token");
    }
    
    [Fact(DisplayName = "GetToken() - Success - Returns Token"), Trait("User Controller", "GetToken")]
    public async Task GetToken_Success() {
        ResetMocks();
        
        MockHelper.TokenRepository.Create.Void(_mockTokenRepository);
        MockHelper.TokenRepository.SaveChanges.Void(_mockTokenRepository);

        SetSut();

        var user = TestModelHelper.User;
        user.AccountUsers = TestModelHelper.AccountUsers;
        var result = _sut.GetToken<TokenDto>(user);

        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.ReadJwtToken(result.Result.AccessToken);
        
        result.Result.Should().BeOfType<TokenDto>("This was passed in as the generic type to be returned as the result");
        token.Issuer.Should().BeEquivalentTo("bmp", "Token issuer should be bmp as this is set in the appsettings.json" );
        token.Audiences.Should().BeEquivalentTo("bmp");

        token.Claims.Should().Contain(c => c.Type == "UserId" && c.Value == user.Id.ToString(), "Token should contain UserId");
        token.Claims.Should().Contain(c => c.Type == "AccountId" && c.Value == user.DefaultAccount.ToString(), "Token should contain AccountId");
        token.Claims.Should().Contain(c => c.Type == "role" && c.Value == "User", "Token should contain a user role for the account");

        var expiryTime = token.Claims.SingleOrDefault(c => c.Type == "exp")?.Value;
        DateTime.UtcNow.AddMinutes(15).Should().BeCloseTo(token.ValidTo, TimeSpan.FromSeconds(3), "Token should expire in 15 minutes");

        _mockTokenRepository.Verify(r => r.Create(It.IsAny<Token>()), Times.Once, "TokenRepository.Create should be called once to create a token");
        _mockTokenRepository.Verify(r => r.SaveChanges(), Times.Once, "TokenRepository.SaveChanges should be called once to save the token");
    }

    [Fact(DisplayName = "GenerateToken - Failure - Should not handle exception"), Trait("TokenService", "GenerateToken")]
    public async void GenerateToken_ThrowsAccountUserNotFoundException_WhenAccountUserCannotBeFound() {
        ResetMocks();

        MockHelper.AccountUserRepository.GetAllUserAccountIdsRole.ReturnsAnEmptyList(_mockAccountUserRepository);

        SetSut();

        await Assert.ThrowsAsync<AccountUserNotFoundException>(() =>
            _sut.GenerateToken(TestModelHelper.User, TestModelHelper.User.DefaultAccount.ToString()));
    }

    [Fact(DisplayName = "GenerateToken - Failure - Should not handle exception"), Trait("TokenService", "GenerateToken")]
    public async void GenerateToken_ThrowsAccountUserNotFoundException_WhenAccountUserCannotBeFound_WhenAccountUserIsNull() {
        ResetMocks();

        MockHelper.AccountUserRepository.GetAllUserAccountIdsRole.ReturnsListOfAccount_WithoutUsedAccountId(_mockAccountUserRepository);

        SetSut();

        await Assert.ThrowsAsync<AccountUserNotFoundException>(() =>
            _sut.GenerateToken(TestModelHelper.User, TestModelHelper.User.DefaultAccount.ToString()));
    }

    [Fact(DisplayName = "ValidateToken - Success - Should return true"), Trait("TokenService", "ValidateToken")]
    public async void ValidateToken_ReturnsTrue_WhenTokenIsValid() {
        SetSut();

        var result = _sut.ValidateToken(TestModelHelper.Token.ValidToken.AccessToken);

        result.Should().BeTrue();
    }

    [Fact(DisplayName = "ValidateToken - Failure - Should return false - Invalid Issuer"), Trait("TokenService", "ValidateToken")]
    public async void ValidateToken_ReturnsFalse_WhenTokenIsInvalid_InvalidIssuer() {
        SetSut();

        var result = _sut.ValidateToken(TestModelHelper.Token.InvalidIssToken.AccessToken);

        result.Should().BeFalse();
    }

    [Fact(DisplayName = "ValidateToken - Failure - Should return false - Invalid Audience"), Trait("TokenService", "ValidateToken")]
    public async void ValidateToken_ReturnsFalse_WhenTokenIsInvalid_InvalidAudience() {
        SetSut();

        var token = TestModelHelper.Token.ValidToken;
        var result = _sut.ValidateToken(TestModelHelper.Token.InvalidAudToken.AccessToken);

        result.Should().BeFalse();
    }

    [Fact(DisplayName = "ValidateToken - Failure - Should return false - Invalid Signing"), Trait("TokenService", "ValidateToken")]
    public async void ValidateToken_ReturnsFalse_WhenTokenIsInvalid_InvalidSigning() {
        SetSut();

        var result = _sut.ValidateToken(TestModelHelper.Token.InvalidSigningToken.AccessToken);

        result.Should().BeFalse();
    }

    [Fact(DisplayName = "GetUserIdFromToken - Success - Should return user id"), Trait("TokenService", "GetUserIdFromToken")]
    public async void GetUserIdFromToken_ReturnsUserId_WhenTokenIsValid() {
        SetSut();

        var result = _sut.GetUserIdFromToken(TestModelHelper.Token.ValidToken.AccessToken);

        result.Should().Be(TestModelHelper.Ids.UserGuid.ToString());
    }

    [Fact(DisplayName = "RefreshToken() - Success - Returns Ok()"), Trait("User Controller", "RefreshToken")]
    public async Task RefreshToken_Success() {
        ResetMocks();
        
        MockHelper.TokenRepository.FindByCondition.ReturnsToken(_mockTokenRepository);
        MockHelper.TokenRepository.Update.ReturnsUpdatedToken(_mockTokenRepository);
        MockHelper.AccountUserRepository.GetAllUserAccountIdsRole.ReturnsListOfAccountUserIdsRole(_mockAccountUserRepository);

        var expiringToken = TestModelHelper.Token.ExpiringTokenDto;
        SetSut();
        var result = await _sut.RefreshToken(expiringToken);
        var response = result.Should().BeOfType<TokenDto>().Subject;
        
        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.ReadJwtToken(response.AccessToken);
        var oldToken = jwtHandler.ReadJwtToken(expiringToken.AccessToken);
        
        token.Claims.Should().Contain(c => c.Type == "UserId" && c.Value == TestModelHelper.Ids.UserGuid.ToString(), "This is the owners of the token ID");
        token.Claims.Should().Contain(c => c.Type == "AccountId" && c.Value == TestModelHelper.Ids.AccountGuid.ToString(), "This is the accountId of the original token");
        token.Claims.Should().Contain(c => c.Type == "role" && c.Value == "User", "This is the role set in the AccountUser with the AccountId");
        token.ValidTo.Should().BeAfter(oldToken.ValidTo, "Issued token should have refreshed and be valid for 15 minutes");
    }
    
    [Fact(DisplayName = "RefreshToken() - Failure Not Handled - Throws InvalidTokenException"), Trait("User Controller", "RefreshToken")]
    public async Task RefreshToken_Failure_NotHandled() {
        ResetMocks();
        
        var expiringToken = TestModelHelper.Token.ExpiringTokenDto;
        SetSut();
        await Assert.ThrowsAsync<InvalidTokenException>(() => _sut.RefreshToken(expiringToken));
    }
    
    [Fact(DisplayName = "RefreshToken() - Failure - Throws TokenStillValidException"), Trait("User Controller", "RefreshToken")]
    public async Task RefreshToken_Failure_TokenStillValid() {
        ResetMocks();
        
        MockHelper.TokenRepository.FindByCondition.ReturnsToken(_mockTokenRepository);
        
        SetSut();
        await Assert.ThrowsAsync<TokenStillValidException>(() => _sut.RefreshToken(TestModelHelper.Token.ValidTokenDto));
    }
}
