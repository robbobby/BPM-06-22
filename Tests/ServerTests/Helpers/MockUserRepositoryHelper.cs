using System.Threading.Tasks;
using Moq;
using Server.Core.Interfaces.Repository;
using Server.Core.Models.Entities.Entity;

namespace ServerTests.Helpers;

public class MockUserRepositoryHelper {

    public static MockUserRepositoryHelper Instance { get; } = new MockUserRepositoryHelper();


    public MockMethod_GetUserByEmail GetUserByEmail => new MockMethod_GetUserByEmail(); 


    public class MockMethod_GetUserByEmail {
        public void ReturnsNull(Mock<IUserRepository> mockUserRepository) {
            mockUserRepository.Setup(x =>
                    x.GetUserByEmail("test"))
                .Returns(Task.FromResult<User>(null!)!);
        }
        
        public void ReturnsUser(Mock<IUserRepository> mockUserRepository) {
            mockUserRepository.Setup(x =>
                    x.GetUserByEmail("test"))
                .Returns(Task.FromResult(TestModelHelper.User)!);
        }
    }
}
