using System.Text;
using Api.Interfaces;
using Api.Interfaces.Repository;
using Api.Repository;
using Api.Services;
using Api.Setup;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Bpm Api",
        Description = "Bmp Api"
    });
    options.DocInclusionPredicate((docName, description) => true);
    
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        // BearerFormat = "JWT",
        // Scheme = "Bearer",
    });
    
    options.DocInclusionPredicate((docName, description) => true);
    
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

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
    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
    options.AddPolicy("User", policy => policy.RequireClaim("User"));
    options.AddPolicy("Guest", policy => policy.RequireClaim("Guest"));
    options.AddPolicy("AdminOrUser", policy => policy.RequireClaim("Admin", "User"));
});

DbSetup.SetDbContext(builder);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class JwtBearerDefaults {
    public static string AuthenticationScheme { get; set; } = "Bearer";
}
