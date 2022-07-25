using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Server.Core.Aggregates;
using Server.Core.Exceptions;
using Server.Core.Interfaces.Service;
using Server.Core.Models;

namespace ServerTests.Helpers;

public class MockUserServiceHelper {
    public static MockUserServiceHelper Instance { get; } = new MockUserServiceHelper();
    public MockMethod_CreateUser CreateUser { get; } = new MockMethod_CreateUser();
    public MockMethod_GetUsersAccounts GetUserAccounts { get; } = new MockMethod_GetUsersAccounts();

    public class MockMethod_CreateUser {
        public void ReturnsTokenDto(Mock<IUserService> mockUserService) {
            mockUserService.Setup(x => x.CreateUser(It.IsAny<UserRequest>()))
                .Returns(Task.FromResult(TestModelHelper.TokenDto));
        }

        public void ThrowsGenericException(Mock<IUserService> mockUserService) {
            mockUserService.Setup(x => x.CreateUser(It.IsAny<UserRequest>()))
                .Throws(new Exception());
        }

        public void ThrowsUserAlreadyExistsException(Mock<IUserService> mockUserService) {
            mockUserService.Setup(x => x.CreateUser(It.IsAny<UserRequest>()))
                .Throws(new UserAlreadyExistsException());
        }
    }

    public class MockMethod_GetUsersAccounts {
        public void ReturnsListOfAccountIds(Mock<IUserService> mockUserService) {
            mockUserService.Setup(x => x.GetUserAccounts(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<AccountUserIdsRole>() {
                    new AccountUserIdsRole {
                        Name = "Project name 1",
                        Role = "User",
                        UserId = TestModelHelper.Ids.UserGuid,
                        AccountId = TestModelHelper.Ids.AccountGuid
                    },
                    new AccountUserIdsRole {
                        Name = "Project name 2",
                        Role = "User",
                        UserId = TestModelHelper.Ids.UserGuid,
                        AccountId = TestModelHelper.Ids.AccountGuid
                    },
                    new AccountUserIdsRole {
                        Name = "Project name 3",
                        Role = "User",
                        UserId = TestModelHelper.Ids.UserGuid,
                        AccountId = TestModelHelper.Ids.AccountGuid
                    }
                }.AsQueryable()));
        }

        public void ThrowsGenericException(Mock<IUserService> mockUserService) {
            mockUserService.Setup(x => x.GetUserAccounts(It.IsAny<string>()))
                .Throws(new Exception());
        }
    }

}
