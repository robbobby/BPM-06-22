using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace Api.MapperProfiles;


// TODO: Tests for this file
public class AccountUserMap : AutoMapperProfile {
    public AccountUserMap() {
        CreateMap<AccountUserIdsRole, AccountUser>();
        CreateMap<AccountUser, AccountUserIdsRole>();
    }
}
