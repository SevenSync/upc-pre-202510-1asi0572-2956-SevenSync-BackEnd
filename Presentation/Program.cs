using System.Reflection;
using System.Text;
using Application.GeminiTest;
using Application.IAM.CommandServices;
using Application.IAM.Others;
using Application.IAM.QueryServices;
using Domain.GeminiTest;
using Domain.IAM.Repositories;
using Domain.IAM.Services.Interfaces;
using Domain.IAM.Services.Others;
using Infrastructure.Context;
using Infrastructure.IAM;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Mapper;

var builder = WebApplication.CreateBuilder(args);

//  @Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "MaceTech API Documentation v1.0.0",
            Description = "An ASP.NET Core Web API for managing MaceTech domain, data, and presentation layers.",
            Contact = new OpenApiContact
            {
                Name = "Contact the team",
                Url = new Uri("https://macetech.com")
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT")
            },
            TermsOfService = new Uri("https://macetech.com/terms-of-service")
        });
        
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });
        
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
);
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserAuthenticationQueryService, UserAuthenticationQueryService>();
builder.Services.AddScoped<IUserAuthenticationCommandService, UserAuthenticationCommandService>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEncryptService, EncryptService>();
builder.Services.AddHttpClient<IGenerativeAiService, GenerativeAiService>();

var key = builder.Configuration.GetValue<string>("JwtSettings:key");
var keyBytes = Encoding.ASCII.GetBytes(key!);

//  @Authentication
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAutoMapper(
    typeof(RequestToModels)
);

//  @Database
var connectionString = builder.Configuration.GetConnectionString("macetech");
builder.Services.AddDbContext<MaceTechDataCenterContext>(
    dbContextOptions =>
    {
        dbContextOptions.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString),
            options => options.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: System.TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null)
        );
    });


var app = builder.Build();

//  app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseCors("AllowAllOrigins");

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<MaceTechDataCenterContext>())
{
    context!.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI(c => { });
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<AuthenticationMiddleware>();
app.Run();