using Api.Models;
using Api.Models.DbModel;

namespace Api.MapperProfiles; 

public class TokenMap : AutoMapperProfile {
    public TokenMap() {
        CreateMap<TokenDto, Token>();
        CreateMap<Token, TokenDto>();
    }
}
