using System.Threading.Tasks;
using Api.Controllers;
using Api.MapperProfiles;
using Api.Services;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Server.Core.Interfaces.Repository;
using Server.Core.Models.Entities.Entity;
using ServerTests.Helpers;
using Xunit;

namespace ServerTests.Services; 

public class AuthServiceTests {
    private Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
    private Mock<ILogger<AuthService>> _mockLogger = new Mock<ILogger<AuthService>>();
    
    private void ResetMocks() {
        _mockLogger.Reset();
        _mockUserRepository.Reset();
    }
    
    [Fact(DisplayName = "Login() - Success - Returns User"), Trait("AuthService", "Login")]
    public async Task Login_Success_ReturnsUser() {
        // Arrange
        ResetMocks();
        
        MockHelper.UserRepository.GetUserByEmail.ReturnsUser(_mockUserRepository);
        var authService = new AuthService(_mockUserRepository.Object, _mockLogger.Object);
        
        var result = await authService.Login("test", "string");
        
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(TestModelHelper.User);
    }
    
    [Fact(DisplayName = "Login() - Failed - incorrect password - Returns null"), Trait("AuthService", "Login")]
    public async Task Login_Failed_incorrectPassword_ReturnsNull() {
        // Arrange
        ResetMocks();
        
        MockHelper.UserRepository.GetUserByEmail.ReturnsUser(_mockUserRepository);
        var authService = new AuthService(_mockUserRepository.Object, _mockLogger.Object);
        
        var result = await authService.Login("test", "wrong");
        
        result.Should().BeNull();
    }
    
    [Fact(DisplayName = "Login() - Failed - user not found - Returns null"), Trait("AuthService", "Login")]
    public async Task Login_Failed_userNotFound_ReturnsNull() {
        // Arrange
        ResetMocks();
        
        MockHelper.UserRepository.GetUserByEmail.ReturnsNull(_mockUserRepository);
        var authService = new AuthService(_mockUserRepository.Object, _mockLogger.Object);
        
        var result = await authService.Login("test", "string");
        
        result.Should().BeNull();
    }
}
