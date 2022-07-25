using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Api.MapperProfiles;
using Api.Services;
using AutoMapper;
using Domain.Repository;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Server.Core.Interfaces.Repository;
using Server.Core.Interfaces.Service;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;
using ServerTests.Helpers;
using Xunit;

namespace ServerTests.Services; 

public class UserServiceTests {
    private Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
    private Mock<IAccountUserRepository> _mockAccountUserRepository = new Mock<IAccountUserRepository>();
    private Mock<ILogger<UserService>> _mockLogger = new Mock<ILogger<UserService>>();
    private Mock<ITokenService> _mockTokenService = new Mock<ITokenService>();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => {
        cfg.AddProfile<UserMap>();
    }));

    private UserService _sut;
    private void ResetMocks() {
        _mockUserRepository.Reset();
        _mockAccountUserRepository.Reset();
        _mockLogger.Reset();
        _mockTokenService.Reset();
    }

    private void SetSut() {
        _sut = new UserService(_mockUserRepository.Object, _mockAccountUserRepository.Object, _mockTokenService.Object, _mapper, _mockLogger.Object);
    }
    
    [Fact(DisplayName = "CreateUser() - Return TokenDto"), Trait("UserService", "CreateUser")]
    public void CreateUser_ShouldReturnUser() {
        ResetMocks();
        
        MockHelper.TokenService.GetToken.ReturnsToken(_mockTokenService);
        MockHelper.UserRepository.Create.Void(_mockUserRepository);
        MockHelper.UserRepository.SaveChanges.Void(_mockUserRepository);
        
        SetSut();
        var result = _sut.CreateUser(TestModelHelper.UserRequest);

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<TokenDto>();
        result.Result.Should().BeEquivalentTo(TestModelHelper.TokenDto);
    }
    
    [Fact(DisplayName = "CreateUser() - Failure - throws Exception"), Trait("UserService", "CreateUser")]
    public void GetUser_ShouldThrowException() {
        ResetMocks();
     
        MockHelper.TokenService.GetToken.Throws(_mockTokenService, new Exception());
        
        SetSut();

        Assert.ThrowsAsync<Exception>(() => _sut.CreateUser(TestModelHelper.UserRequest));
    }
    
    [Fact(DisplayName = "GetAllUserAccountIds() - Return IQueryable<UserAccountIds>"), Trait("UserService", "GetAllUserAccountIds")]
    public void GetAllUserAccountIds_ShouldReturnIQueryable() {
        ResetMocks();
        
        MockHelper.AccountUserRepository.GetAllUserAccountIds.ReturnsIQueryable(_mockAccountUserRepository);
        
        SetSut();
        var result = _sut.GetUserAccounts(TestModelHelper.Ids.UserGuid.ToString());
        
        result.Result.Should().NotBeNull();
        result.Result.Should().BeAssignableTo<IEnumerable<Guid>>();
        result.Result.Count().Should().Be(3);
    }
    
    [Fact(DisplayName = "GetAllUserAccountIds() - Failure - throws Exception"), Trait("UserService", "GetAllUserAccountIds")]
    public void GetAllUserAccountIds_ShouldThrowException() {
        ResetMocks();
        
        MockHelper.AccountUserRepository.GetAllUserAccountIds.Throws(_mockAccountUserRepository, new Exception());
        
        SetSut();
        
        Assert.ThrowsAsync<Exception>(() => _sut.GetUserAccounts(TestModelHelper.Ids.UserGuid.ToString()));
    }
}
