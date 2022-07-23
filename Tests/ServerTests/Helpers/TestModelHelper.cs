using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Api.Controllers;
using Domain.Repository;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Server.Core.Aggregates;
using Server.Core.Models;
using Server.Core.Models.Entities.Entity;

namespace ServerTests.Helpers;

public static class TestModelHelper {
    public static GuidIds Ids = new GuidIds();
    

    private static DateTime DateCreated = DateTime.Now;
    private static DateTime DateUpdated = DateTime.Now;
    private static DateTime DateDeleted = DateTime.Now;

    public static User User =>
        new User {
            DateCreated = DateCreated,
            DateUpdated = DateUpdated,
            DateDeleted = null,
            Id = Ids.UserGuid,
            AccountUsers = new List<AccountUser>(),
            Password = "$2a$08$tv1Pr6yWYvlr92ePC8Vou.a56ngrtRR/i6LbXvZPHUbmNZ3lR/MIi",
            FirstName = "FirstName",
            LastName = "LastName",
            EmailAddress = "EmailAddress@email.com",
            DefaultAccount = Ids.AccountGuid,
            LastActive = DateUpdated,
            Disabled = false,
            Salt = "$2a$08$tv1Pr6yWYvlr92ePC8Vou."
        };

    public static LoginRequestModel LoginRequest =>
        new LoginRequestModel() {
            EmailAddress = "email@email.com",
            Password = "password"
        };
    public static RefreshTokenRequestModel RefreshTokenRequest =>
        new RefreshTokenRequestModel {
            AccessToken = "accessToken",
            RefreshToken = "refreshToken"
        };

    public static TokenDto RefreshedToken =>
        new TokenDto {
            AccessToken = "newAccessToken",
            RefreshToken = "refreshedToken"
        };
    public static TokenDto TokenDto =>
        new TokenDto() {
            AccessToken = "accessToken",
            RefreshToken = "refreshToken"
        };
    public static UserRequest UserRequest => new UserRequest {
        Username = "username",
        Password = "password",
        FirstName = "FirstName",
        LastName = "LastName",
        EmailAddress = "email@email.com"
    };
    public static ProjectCreateRequest ProjectCreateRequest => new ProjectCreateRequest {
        Name = "ProjectName",
        Description = "ProjectDescription",
        AccountId = null
    };

    public static Project CreatedProject => new Project {
        DateCreated = DateCreated,
        DateUpdated = DateUpdated,
        DateDeleted = null,
        Id = Ids.ProjectGuid,
        Name = "ProjectName",
        Description = "ProjectDescription",
        AccountId = Ids.AccountGuid,
        Account = null
    };

    public static IQueryable<Project> Projects => new List<Project> {
        new Project {
            DateCreated = DateCreated,
            DateUpdated = DateUpdated,
            DateDeleted = null,
            Id = Ids.ProjectGuid,
            Name = "ProjectName",
            Description = "ProjectDescription",
            AccountId = Ids.AccountGuid,
            Account = null
        },
        new Project {
            DateCreated = DateCreated,
            DateUpdated = DateUpdated,
            DateDeleted = null,
            Id = Ids.ProjectGuid,
            Name = "ProjectName",
            Description = "ProjectDescription",
            AccountId = Ids.AccountGuid,
            Account = null
        }
    }.AsQueryable();
    public static TokenHelper Token => TokenHelper.Instance;

    public static List<AccountUser> AccountUsers => new List<AccountUser> {
        new AccountUser {
            DateCreated = DateCreated,
            DateUpdated = DateUpdated,
            DateDeleted = null,
            AccountId = Ids.AccountGuid,
            UserId = Ids.UserGuid,
            Account = null,
            User = null,
            Role = "User"
        }
    };

    public static IQueryable<AccountUserIdsRole> AccountUserIdsRoles => new List<AccountUserIdsRole> {
        new AccountUserIdsRole {
            AccountId = Ids.AccountGuid.ToString(),
            UserId = Ids.UserGuid.ToString(),
            Role = "User"
        },
        new AccountUserIdsRole {
            AccountId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString(),
            Role = "Admin"
        }
    }.AsQueryable();
}

public class TokenHelper {
    private static string SecretKey = "26f61df0-f902-4746-98b9-d00fd8ed00f6";
    private static SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey));
    private static string Issuer = "bmp";
    private static string Audience = "bmp";
    
    private static GuidIds Ids = new GuidIds();
    public static TokenHelper Instance { get; } = new TokenHelper();
    
    public TokenDto ExpiringTokenDto => new TokenDto() {
        AccessToken = GenerateToken(Audience, Issuer, SigningKey, DateTime.Now.AddMilliseconds(1)),
        RefreshToken = "refreshToken"
    };

    public TokenDto ValidTokenDto => new TokenDto() {
        AccessToken = GenerateToken(Audience, Issuer, SigningKey),
        RefreshToken = "refreshToken"
    };

    public Token ValidToken => new Token {
        AccessToken = GenerateToken(Audience,
            Issuer,
            SigningKey),
        RefreshToken = "refreshToken",
        User = TestModelHelper.User,
        AccountId = Ids.AccountGuid.ToString(),
    };

    public Token InvalidAudToken => new Token() {
        AccessToken = GenerateToken("Invalid", Issuer, SigningKey),
        RefreshToken = "refreshToken"
    };

    public Token InvalidIssToken => new Token() {
        AccessToken = GenerateToken(Audience, "Invalid", SigningKey),
        RefreshToken = "refreshToken"
    };

    public Token InvalidSigningToken => new Token() {
        AccessToken = GenerateToken(Audience, Issuer, new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()))),
        RefreshToken = "refreshToken"
    };

    private static string GenerateToken(string audience, string issuer, SymmetricSecurityKey signingKey, DateTime? expires = null) {
        expires ??= DateTime.Now.AddMinutes(15);
            
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Role, "User"),
                new Claim("UserId", Ids.UserGuid.ToString()),
                new Claim("AccountId", Ids.AccountGuid.ToString()),
            }),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenSec = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(tokenSec);
        Console.WriteLine(token);
        return tokenHandler.WriteToken(tokenSec);
    }
}

public class GuidIds {
    public readonly Guid AccountGuid = Guid.Parse("c62422f5-b2c0-480c-91bd-15bbb1ec029d");
    public readonly Guid ProjectGuid = Guid.Parse("383b6024-4e14-454a-90bb-d63efbab1d77");
    public readonly Guid UserGuid = Guid.Parse("359b579d-26bd-4c20-bcae-f6ef8f0b643f");
}