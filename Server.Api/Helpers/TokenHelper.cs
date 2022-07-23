using System.IdentityModel.Tokens.Jwt;

namespace Api.Helpers; 


// TODO: Tests for this file
public static class TokenHelper {
    private static readonly JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
    
    public static string? GetUserIdFromToken(string token) {
        var jsonToken = TokenHandler.ReadToken(token) as JwtSecurityToken;
        var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        return userId;
    }
    
    public static string? GetUserIdFromToken(HttpRequest request) {
        var token = GetTokenFromRequest(request);
        return GetUserIdFromToken(token);
    }
    
    public static UserAccountIdUserId GetAccountAndUserId(string token) {
        var jsonToken = TokenHandler.ReadToken(token) as JwtSecurityToken;
        var user = new UserAccountIdUserId() {
            AccountId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value,
            UserId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value
        };
        return user;
    }
    
    public static UserAccountIdUserId GetAccountAndUserId(HttpRequest request) {
        var token = GetTokenFromRequest(request);
        return GetAccountAndUserId(token);
    }
    
    public static string GetAccountIdFromToken(string token) {
        var jsonToken = TokenHandler.ReadToken(token) as JwtSecurityToken;
        var accountId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
        return accountId;
    }
    
    public static string GetAccountIdFromToken(HttpRequest request) {
        var token = GetTokenFromRequest(request);
        return GetAccountIdFromToken(token);
    }
    
    public static string GetTokenFromRequest(HttpRequest request) {
        var token = request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        return token;
    }
}

public class UserAccountIdUserId {
    public string? AccountId { get; set; }
    public string? UserId { get; set; }
}