using MaceTech.API.Analytics.Application.Internal.CommandServices;
using MaceTech.API.Analytics.Application.Internal.QueryServices;
using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Analytics.Domain.Services.CommandServices;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;
using MaceTech.API.Analytics.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.Analytics.Interfaces.ACL;
using MaceTech.API.Analytics.Interfaces.ACL.Services;
using MaceTech.API.IAM.Application.Internal.CommandServices;
using MaceTech.API.IAM.Application.Internal.OutboundServices;
using MaceTech.API.IAM.Application.Internal.QueryServices;
using MaceTech.API.IAM.Domain.Repositories;
using MaceTech.API.IAM.Domain.Services;
using MaceTech.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using MaceTech.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using MaceTech.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using MaceTech.API.IAM.Infrastructure.Tokens.JWT.Services;
using MaceTech.API.IAM.Interfaces.ACL;
using MaceTech.API.IAM.Interfaces.ACL.Services;
using MaceTech.API.Profiles.Application.Internal.CommandServices;
using MaceTech.API.Profiles.Application.Internal.QueryServices;
using MaceTech.API.Profiles.Domain.Repositories;
using MaceTech.API.Profiles.Domain.Services;
using MaceTech.API.Profiles.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.Profiles.Interfaces.ACL;
using MaceTech.API.Profiles.Interfaces.ACL.Services;
using MaceTech.API.Shared.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.Watering.Application.Internal.CommandServices;
using MaceTech.API.Watering.Application.Internal.QueryServices;
using MaceTech.API.Watering.Domain.Repositories;
using MaceTech.API.Watering.Domain.Services.CommandServices;
using MaceTech.API.Watering.Domain.Services.QueryServices;
using MaceTech.API.Watering.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//  Add Controllers
builder.Services.AddControllers();

//  Configure Lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

//  Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//  Configure Database Context and Logging Levels
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (connectionString == null) return;
    if (builder.Environment.IsDevelopment())
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
    else
    {
        if (builder.Environment.IsProduction())
        {
            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Error)
                .EnableDetailedErrors();
        }
    }
});

//  Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MaceTech API",
            Version = "v1",
            Description = "MaceTech API Documentation",
            TermsOfService = new Uri("https://macetech.com/tos"),
            Contact = new OpenApiContact { Name = "MaceTech", Email = "support@macetech.com" },
            License = new OpenApiLicense { Name = "Apache 2.0", Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html") }
        });
        c.EnableAnnotations();
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Hello there! Please enter your token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                }, []
            }
        });
    }
    );

//  Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", policy => policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

//  Configure Dependency Injection
//      |: Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//      |: IAM Bounded Context Injection Configuration
//          .| TokenSettings Configuration
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
//          .| Services Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

//      |: Profiles Bounded Context Injection Configuration
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

//      |: Analytics Bounded Context Injection Configuration
builder.Services.AddScoped<IPotRecordRepository, PotRecordRepository>();
builder.Services.AddScoped<IPotRecordCommandService, PotRecordCommandService>();
builder.Services.AddScoped<IPotRecordQueryService, IPotRecordQueryService>();

builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IAlertCommandService, AlertCommandService>();
builder.Services.AddScoped<IAlertCommandService, AlertCommandService>();


builder.Services.AddScoped<IAnalyticsQueryService, AnalyticsQueryService>();

builder.Services.AddScoped<IAlertQueryService, AlertQueryService>(); 


builder.Services.AddScoped<IAlertsContextFacade, AlertsContextFacade>();
builder.Services.AddScoped<IWateringContextFacade, WateringContextFacade>();

//      |: Watering Bounded Context Injection Configuration
builder.Services.AddScoped<IWateringLogRepository, WateringLogRepository>();
builder.Services.AddScoped<IWateringLogCommandService, WateringLogCommandService>();
builder.Services.AddScoped<IWateringLogQueryService, WateringLogQueryService>();


var app = builder.Build();

//  Verify Database Objects Created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

//  Configure the HTTP request pipeline.
//  if (!app.Environment.IsDevelopment())   //  (Ignore this time, teacher is nearby...)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAllPolicy");
app.UseRequestAuthorization();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();