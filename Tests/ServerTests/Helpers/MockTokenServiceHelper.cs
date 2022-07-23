using System;
using System.Threading.Tasks;
using Api.Controllers;
using Microsoft.AspNetCore.Http;
using Moq;
using Server.Core.Exceptions;
using Server.Core.Interfaces.Service;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace ServerTests.Helpers;

public class MockTokenServiceHelper {
    public static MockTokenServiceHelper Instance { get; } = new MockTokenServiceHelper();
    public MockMethod_RefreshToken RefreshToken { get; } = new MockMethod_RefreshToken();
    public MockMethod_GetToken GetToken { get; } = new MockMethod_GetToken();
    
    public class MockMethod_GenerateToken {
        public void ReturnsToken(Mock<ITokenService> mockTokenService) {
            mockTokenService.Setup(tokenService =>
                    tokenService.GenerateToken(
                        It.IsAny<User>(),
                        It.IsAny<string>()))
                .Returns(Task.FromResult(new Token() {
                        AccessToken = "accessToken",
                        RefreshToken = "refreshToken"
                    }
                    ));
        }
    }

    public class MockMethod_GetToken {
            public void ReturnsToken(Mock<ITokenService> mockTokenService) {
            mockTokenService.Setup(x => x
                    .GetToken<TokenDto>(It.IsAny<User>(), It.IsAny<string?>()))
                .Returns(Task.FromResult(new TokenDto() {
                    AccessToken = "accessToken",
                    RefreshToken = "refreshToken"
                }
                ));
        }

            public void Throws<T>(Mock<ITokenService> mockTokenService, T exception) where T : Exception {
                mockTokenService.Setup(x => x
                        .GetToken<TokenDto>(It.IsAny<User>(), It.IsAny<string?>()))
                    .Throws(exception);
            }
    }
    
    public class MockMethod_RefreshToken {
        public void ReturnsToken(Mock<ITokenService> mockTokenService) {
            mockTokenService.Setup(x => x
                    .RefreshToken(It.IsAny<TokenDto>()))
                .Returns(Task.FromResult(TestModelHelper.RefreshedToken));
        }

        public void ThrowsInvalidTokenException(Mock<ITokenService> mockTokenService) {
            mockTokenService.Setup(x => x
                    .RefreshToken(It.IsAny<TokenDto>()))
                .Throws(new InvalidTokenException());
        }

        public void ThrowsTokenStillValid(Mock<ITokenService> mockTokenService) {
            mockTokenService.Setup(x => x
                    .RefreshToken(It.IsAny<TokenDto>()))
                .Throws(new TokenStillValidException());
        }

        public void ThrowsException(Mock<ITokenService> mockTokenService) {
            mockTokenService.Setup(x => x
                    .RefreshToken(It.IsAny<TokenDto>()))
                .Throws(new Exception());
        }
    }
}
