using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Server.Core.Aggregates;
using Server.Core.Exceptions;
using Server.Core.Interfaces.Service;

namespace ServerTests.Helpers;

public class MockUserServiceHelper {
    public static MockUserServiceHelper Instance { get; } = new MockUserServiceHelper();
    public MockMethod_CreateUser CreateUser { get; } = new MockMethod_CreateUser();
    public MockMethod_GetAllUsersAccountIds GetAllUserAccountIds { get; } = new MockMethod_GetAllUsersAccountIds();

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
    
    public class MockMethod_GetAllUsersAccountIds {
        public void ReturnsListOfAccountIds(Mock<IUserService> mockUserService) {
            mockUserService.Setup(x => x.GetAllUserAccountIds(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() }.AsQueryable()));
        }

        public void ThrowsGenericException(Mock<IUserService> mockUserService) {
            mockUserService.Setup(x => x.GetAllUserAccountIds(It.IsAny<string>()))
                .Throws(new Exception());
        }
    }

}

