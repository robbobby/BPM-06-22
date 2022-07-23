using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Server.Core.Interfaces.Service;
using Server.Core.Models;
using ServerTests.Helpers;
using Xunit;

namespace ServerTests.Controllers;

public class UserControllerTests {
    private readonly Mock<IUserService> _mockUserService = new();
    private readonly Mock<ILogger<UserController>> _logger = new();
    private UserController _sut;

    private void ResetMocks() {
        _mockUserService.Reset();
        _logger.Reset();
    }

    [Fact(DisplayName = "Register() - Success - Returns Ok()"), Trait("User Controller", "Register")]
    public async Task Register_Success() {
        ResetMocks();
        MockHelper.UserService.CreateUser.ReturnsTokenDto(_mockUserService);

        _sut = new UserController(_mockUserService.Object, _logger.Object);
        var result = await _sut.Register(TestModelHelper.UserRequest);

        var response = Assert.IsType<OkObjectResult>(result);
        response.StatusCode.Should().Be(200);

        var token = Assert.IsType<TokenDto>(response.Value);
        token.Should().BeEquivalentTo(TestModelHelper.TokenDto);
    }

    [Fact(DisplayName = "Register() - Failure - Throws Generic Exception - Returns StatusCode(500)"), Trait("User Controller", "Register")]
    public async Task Register_Failure() {
        ResetMocks();
        MockHelper.UserService.CreateUser.ThrowsGenericException(_mockUserService);
        _sut = new UserController(_mockUserService.Object, _logger.Object);

        var result = await _sut.Register(TestModelHelper.UserRequest);

        var response = Assert.IsType<ObjectResult>(result);
        response.StatusCode.Should().Be(500);
        response.Value.Should().Be("Internal server error");
    }

    [Fact(DisplayName = "Register() - Failure - ThrowsUserAlreadyRegisteredException - Returns Conflict()"), Trait("User Controller", "Register")]
    public async Task Register_Failure_ThrowsUserAlreadyRegisteredException() {
        ResetMocks();
        MockHelper.UserService.CreateUser.ThrowsUserAlreadyExistsException(_mockUserService);
        _sut = new UserController(_mockUserService.Object, _logger.Object);

        var result = await _sut.Register(TestModelHelper.UserRequest);

        var response = Assert.IsType<ConflictObjectResult>(result);
        response.StatusCode.Should().Be(409);
        response.Value.Should().Be("Email already registered");
    }
    
    [Fact(DisplayName = "GetAllUserAccountAccountIds() - Success - Returns Ok()"), Trait("User Controller", "GetAllUserAccountIds")]
    public async Task GetAllUserAccountIds_Success() {
        ResetMocks();
        MockHelper.UserService.GetAllUserAccountIds.ReturnsListOfAccountIds(_mockUserService);

        var httpContext = new DefaultHttpContext();
        MockHelper.HttpContext.SetAuthHeader(httpContext);

        _sut = new UserController(_mockUserService.Object, _logger.Object) {
            ControllerContext = new ControllerContext() {
                HttpContext = httpContext
            }
        };

        // mock TokenHelper
        var result = await _sut.GetAllUserAccountIds();

        var response = result.Should().BeOfType<OkObjectResult>().Subject;
        response.StatusCode.Should().Be(200);

        var accountIds = response.Value.Should().BeAssignableTo<IQueryable<Guid>>().Subject.ToList();
        accountIds.Count.Should().BeGreaterThan(0);
        accountIds[0].Should().NotBe(Guid.Empty);
    }
    
    [Fact(DisplayName = "GetAllUserAccountAccountIds() - Failure - Throws Generic Exception - Returns BadRequest()"), Trait("User Controller", "GetAllUserAccountIds")]
    public async Task GetAllUserAccountIds_Failure() {
        ResetMocks();
        MockHelper.UserService.GetAllUserAccountIds.ThrowsGenericException(_mockUserService);

        var httpContext = new DefaultHttpContext();
        MockHelper.HttpContext.SetAuthHeader(httpContext);
        
        _sut = new UserController(_mockUserService.Object, _logger.Object) {
            ControllerContext = new ControllerContext() {
                HttpContext = httpContext
            }
        };
        
        var result = await _sut.GetAllUserAccountIds();
        
        var response = Assert.IsType<BadRequestObjectResult>(result);
        response.StatusCode.Should().Be(400);
        response.Value.Should().Be("Internal server error");
    }
}
