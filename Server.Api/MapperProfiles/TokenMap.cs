using Api.Controllers;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace Api.MapperProfiles; 

public class TokenMap : AutoMapperProfile {
    public TokenMap() {
        CreateMap<TokenDto, Token>();
        CreateMap<Token, TokenDto>();
    }
}
