using System.Reflection;
using System.Security.Claims;
using System.Text;
using Api;
using Api.MapperProfiles;
using Api.Services;
using Api.Setup;
using Domain;
using Domain.Repository;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Core;
using Server.Core.Interfaces.Repository;
using Server.Core.Interfaces.Service;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1",
        Title = "Bpm Server.Api",
        Description = "Bmp Server.Api",
        TermsOfService = new Uri("https://example.com/terms")
    });
    options.DocInclusionPredicate((docName, description) => true);

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });

    options.DocInclusionPredicate((docName, description) => true);

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountUserRepository, AccountUserRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddAutoMapper(new Assembly[] {
    typeof(AutoMapperProfile).Assembly
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWT").GetValue<string>("JWT_SECRET"))),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWT").GetValue<string>("JWT_SECRET"))),

builder.Services.AddAuthorization(options => {
    options.AddPolicy("ApiScope", policy => {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("User", policy => {
        policy.RequireAssertion(context => {
            return context.User.HasClaim(claim => claim.Type == ClaimTypes.Role && claim.Value == "Admin")
                || context.User.HasClaim(claim => claim.Type == ClaimTypes.Role && claim.Value == "User");
        });
    });
    options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("Guest", policy => policy.RequireClaim("Guest"));
    options.AddPolicy("AdminOrUser", policy => policy.RequireClaim("Admin", "User"));
});

DbSetup.SetDbContext(builder.Configuration, builder.Services);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "docs");
        options.RoutePrefix = "docs";
        options.InjectStylesheet("/SwaggerUi/SwaggerDark.css");
    });
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

// enable cors
app.UseCors(corsPolicyBuilder => corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<BmpDbContext>().Migrate();
    await context;
}

app.Run();

namespace Api {
    public class JwtBearerDefaults {
        public static string AuthenticationScheme { get; set; } = "Bearer";
    }
}
