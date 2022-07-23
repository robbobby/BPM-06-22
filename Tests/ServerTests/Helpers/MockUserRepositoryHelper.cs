using System.Threading.Tasks;
using Moq;
using Server.Core.Interfaces.Repository;
using Server.Core.Models.Entities.Entity;

namespace ServerTests.Helpers;

public class MockUserRepositoryHelper {

    public static MockUserRepositoryHelper Instance { get; } = new MockUserRepositoryHelper();


    public MockMethod_GetUserByEmail GetUserByEmail => new MockMethod_GetUserByEmail();
    public MockMethod_Create Create { get; } = new MockMethod_Create();
    public MockMethod_SaveChanges SaveChanges { get; } = new MockMethod_SaveChanges();


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


    public class MockMethod_Create {

        public void Void(Mock<IUserRepository> mockUserRepository) {
            mockUserRepository.Setup(x =>
                x.Create(It.IsAny<User>()));
        }
    }

    public class MockMethod_SaveChanges {
        public void Void(Mock<IUserRepository> mockUserRepository) {
            mockUserRepository.Setup(x =>
                x.SaveChanges());
        }
    }
}
