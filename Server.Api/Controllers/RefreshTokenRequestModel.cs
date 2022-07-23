using Api.MapperProfiles;
using Server.Core.Models;

namespace Api.Controllers;

public class RefreshTokenRequestModel : TokenDto {
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

public class RefreshTokenRequestModelMap : AutoMapperProfile {
    public RefreshTokenRequestModelMap() {
        CreateMap<RefreshTokenRequestModel, TokenDto>();
        CreateMap<TokenDto, RefreshTokenRequestModel>();
    }
}
