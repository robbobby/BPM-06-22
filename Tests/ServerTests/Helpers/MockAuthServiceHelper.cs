using System;
using System.Threading.Tasks;
using Moq;
using Server.Core.Interfaces.Service;
using Server.Core.Models.Entities.Entity;

namespace ServerTests.Helpers;

public class MockAuthServiceHelper {
    public static MockAuthServiceHelper Instance { get; } = new MockAuthServiceHelper();

    public MockMethod_Login Login { get; } = new MockMethod_Login();
    
    public class MockMethod_Login {
        public void ReturnsUser(Mock<IAuthService> mockAuthService) {
            mockAuthService.Setup(authMock =>
                    authMock.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(TestModelHelper.User)!);
        }

        public void ReturnsNull(Mock<IAuthService> mockAuthService) {
            mockAuthService.Setup(authMock =>
                    authMock.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult((User?)null));
        }

        public void ThrowsException(Mock<IAuthService> mockAuthService) {
            mockAuthService.Setup(authMock =>
                    authMock.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());
        }
    }
}
