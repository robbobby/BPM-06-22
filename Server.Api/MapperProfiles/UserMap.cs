using Server.Core.Aggregates;
using Server.Core.Models.Entities.Entity;

namespace Api.MapperProfiles; 


// TODO: Test this
public class UserMap : AutoMapperProfile{
    public UserMap() {
        CreateMap<UserRequest, User>();
        CreateMap<User, UserRequest>();
    }
}
