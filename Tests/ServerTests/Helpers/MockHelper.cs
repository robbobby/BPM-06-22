using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Api.Controllers;
using Api.Services;
using Domain.Repository;
using Microsoft.AspNetCore.Http;
using Moq;
using Server.Core.Aggregates;
using Server.Core.Interfaces.Service;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace ServerTests.Helpers;

public class MockHelper {
    public static readonly MockTokenServiceHelper TokenService = MockTokenServiceHelper.Instance;
    public static readonly MockAuthServiceHelper AuthService = MockAuthServiceHelper.Instance;
    public static readonly MockUserServiceHelper UserService = MockUserServiceHelper.Instance;
    public static readonly MockProjectServiceHelper ProjectService = MockProjectServiceHelper.Instance;
    public static readonly MockUserRepositoryHelper UserRepository = MockUserRepositoryHelper.Instance;
    public static readonly MockTokenRepositoryHelper TokenRepository = MockTokenRepositoryHelper.Instance;
    public static readonly MockAccountUserRepositoryHelper AccountUserRepository = MockAccountUserRepositoryHelper.Instance;
    public static readonly HttpContextHelper HttpContext = HttpContextHelper.Instance;

    private static MockHelper _instance;
    public static MockHelper Instance {
        get {
            return _instance ??= new MockHelper();
        }
    }
}

public class MockAccountUserRepositoryHelper {
    public static MockAccountUserRepositoryHelper Instance { get; } = new MockAccountUserRepositoryHelper();
    public MockMethod_GetAllUserAccountsIdsRole GetAllUserAccountIdsRole { get; } = new MockMethod_GetAllUserAccountsIdsRole();

    public class MockMethod_GetAllUserAccountsIdsRole {
        public void ReturnsListOfAccountUserIdsRole(Mock<IAccountUserRepository> mockAccountUserRepository) {
            mockAccountUserRepository.Setup(x => x.GetAllUserAccountsIdsRole(It.IsAny<string>()))
                .Returns(TestModelHelper.AccountUserIdsRoles);
        }

        public void ReturnsAnEmptyList(Mock<IAccountUserRepository> mockAccountUserRepository) {
            mockAccountUserRepository.Setup(x => x.GetAllUserAccountsIdsRole(It.IsAny<string>()))
                .Returns(new List<AccountUserIdsRole>().AsQueryable());
        }

        public void ReturnsListOfAccount_WithoutUsedAccountId(Mock<IAccountUserRepository> mockAccountUserRepository) {
            mockAccountUserRepository.Setup(x => x.GetAllUserAccountsIdsRole(It.IsAny<string>()))
                .Returns(TestModelHelper.AccountUserIdsRoles.Where(x => x.AccountId != TestModelHelper.Ids.AccountGuid.ToString()).AsQueryable());
        }
    }
}

public class MockTokenRepositoryHelper {
    public static MockTokenRepositoryHelper Instance { get; } = new MockTokenRepositoryHelper();
    public MockMethod_Create Create { get; } = new MockMethod_Create();
    public MockMethod_SaveChanges SaveChanges { get; } = new MockMethod_SaveChanges();
    public MockMethod_FindByCondition FindByCondition { get; } = new MockMethod_FindByCondition();
    public MockMethod_Update Update { get; } = new MockMethod_Update();

    public class MockMethod_Create {
        public void Void(Mock<ITokenRepository> mockTokenRepository) {
            mockTokenRepository.Setup(x => x.Create(It.IsAny<Token>()));
        }

        public void Throws<T>(Mock<ITokenRepository> mockTokenRepository, T exception) where T : Exception {
            mockTokenRepository.Setup(x => x.Create(It.IsAny<Token>())).Throws(exception);
        }
    }

    public class MockMethod_SaveChanges {
        public void Void(Mock<ITokenRepository> mockTokenRepository) {
            mockTokenRepository.Setup(x => x.SaveChanges());
        }
    }

    public class MockMethod_FindByCondition {
        public void ReturnsToken(Mock<ITokenRepository> mockTokenRepository) {
            mockTokenRepository.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Token, bool>>>()))
                .Returns(new List<Token> {
                        TestModelHelper.Token.ValidToken
                    }.AsQueryable
                    );

        }
    }
    
    public class MockMethod_Update {
        public void ReturnsUpdatedToken(Mock<ITokenRepository> mockTokenRepository) {
            mockTokenRepository.Setup(x => x.Update(It.IsAny<Token>()))
                .Returns(TestModelHelper.Token.ValidToken);
        }
    }
}


public class MockProjectServiceHelper {
    public static MockProjectServiceHelper Instance { get; } = new MockProjectServiceHelper();
    public MockMethod_Create Create { get; } = new MockMethod_Create();
    public MockMethod_GetAll GetAll { get; } = new MockMethod_GetAll();

    public class MockMethod_GetAll {
        public void ReturnsProjects(Mock<IProjectService> mockProjectService) {
            mockProjectService.Setup(x =>
                x.GetAllProjects(It.IsAny<string>())).Returns(TestModelHelper.Projects);
        }
    }

    public class MockMethod_Create {
        public void ReturnsProject(Mock<IProjectService> mockProjectService) {
            mockProjectService.Setup(x =>
                x.Create(It.IsAny<ProjectCreateRequest>())).Returns(TestModelHelper.CreatedProject);
        }

        public void ThrowsException(Mock<IProjectService> mockProjectService) {
            mockProjectService.Setup(x =>
                x.Create(It.IsAny<ProjectCreateRequest>())).Throws(new Exception());
        }


    }
}

public class HttpContextHelper {
    public static HttpContextHelper Instance { get; } = new HttpContextHelper();

    public void SetAuthHeader(HttpContext httpContext) {
        httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiQWRtaW4iLCJVc2VySWQiOiIxM2E1MGE3ZC1mMzczLTRmMmQtODcyMS04OGQ5NDNjZTljZTIiLCJBY2NvdW50SWQiOiIyOTZkNTBlNC00Yjc3LTRmZDgtODJlOC1lNGUxNGJlMDMxYzkiLCJuYmYiOjE2NTg0MTc0NjUsImV4cCI6MTY1ODQxNzQ4MCwiaWF0IjoxNjU4NDE3NDY1LCJpc3MiOiJibXAiLCJhdWQiOiJibXAifQ.-bo6A5Ul6souohsnXCurzffzfMSXq6Y_haPhGKN7h5o";
    }
}
