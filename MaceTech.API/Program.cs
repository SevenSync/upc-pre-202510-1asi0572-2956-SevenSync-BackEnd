using System;
using System.IO;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using MaceTech.API.Analytics.Application.Internal.CommandServices;
using MaceTech.API.Analytics.Application.Internal.QueryServices;
using MaceTech.API.Analytics.Domain.Repositories;
using MaceTech.API.Analytics.Domain.Services.CommandServices;
using MaceTech.API.Analytics.Domain.Services.QueriesServices;
using MaceTech.API.Analytics.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.Analytics.Interfaces.ACL;
using MaceTech.API.Analytics.Interfaces.ACL.Services;
using MaceTech.API.ARM.Application.Internal.CommandServices;
using MaceTech.API.ARM.Application.Internal.QueryServices;
using MaceTech.API.ARM.Domain.Repositories;
using MaceTech.API.ARM.Domain.Services;
using MaceTech.API.ARM.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.IAM.Application.External.Email.Services;
using MaceTech.API.IAM.Application.Internal.CommandServices;
using MaceTech.API.IAM.Application.Internal.OutboundServices;
using MaceTech.API.IAM.Application.Internal.QueryServices;
using MaceTech.API.IAM.Domain.Repositories;
using MaceTech.API.IAM.Domain.Services;
using MaceTech.API.IAM.Infrastructure.Authentication.Firebase.Configuration;
using MaceTech.API.IAM.Infrastructure.Email.SendGrid.Configuration;
using MaceTech.API.IAM.Infrastructure.Email.SendGrid.Services;
using MaceTech.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using MaceTech.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using MaceTech.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using MaceTech.API.IAM.Interfaces.ACL;
using MaceTech.API.IAM.Interfaces.ACL.Services;
using MaceTech.API.Planning.Infrastructure.Persistence.EFC.Seeders;
using MaceTech.API.Profiles.Application.Internal.CommandServices;
using MaceTech.API.Profiles.Application.Internal.QueryServices;
using MaceTech.API.Profiles.Domain.Repositories;
using MaceTech.API.Profiles.Domain.Services;
using MaceTech.API.Profiles.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.Profiles.Interfaces.ACL;
using MaceTech.API.Profiles.Interfaces.ACL.Services;
using MaceTech.API.Shared.Domain.Events;
using MaceTech.API.Shared.Domain.Repositories;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using MaceTech.API.SP.Application.External.Sku.Services;
using MaceTech.API.SP.Application.Internal.CommandServices;
using MaceTech.API.SP.Application.Internal.QueryServices;
using MaceTech.API.SP.Domain.Repositories;
using MaceTech.API.SP.Domain.Services;
using MaceTech.API.SP.Infrastructure.Persistence.EFC.Repositories;
using MaceTech.API.SP.Infrastructure.Plans.Repository;
using MaceTech.API.SP.Infrastructure.Sku;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Stripe;
using TokenService = MaceTech.API.IAM.Infrastructure.Tokens.JWT.Services.TokenService;

var builder = WebApplication.CreateBuilder(args);

var firebaseApp = FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(builder.Configuration["Firebase:ServiceAccountPath"])
});
builder.Services.AddSingleton(firebaseApp);
builder.Services.AddSingleton(FirebaseApp.DefaultInstance);
builder.Services.AddSingleton<FirebaseAuth>(sp => FirebaseAuth.GetAuth(sp.GetRequiredService<FirebaseApp>()));
builder.Services.Configure<FirebaseConfiguration>(builder.Configuration.GetSection("Firebase"));

builder.Services.Configure<SendGridOptions>(builder.Configuration.GetSection("SendGridOptions"));
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();

builder.Services.AddMediatR(typeof(UserDeletedEvent).Assembly);

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
builder.Services.AddSingleton(provider =>
{
    var key = provider.GetRequiredService<IConfiguration>()["Stripe:SecretKey"];
    return new StripeClient(key); 
});

builder.Services.AddControllers();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MaceTech API",
            Version = "v0.3.1a (inDev)",
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", policy => policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

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
builder.Services.AddScoped<IEmailComposer, EmailComposer>();

//      |: Profiles Bounded Context Injection Configuration
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

//      |: Subscriptions and Payments Bounded Context Injection Configuration
builder.Services.AddScoped<ISubscriptionPlansQueryService, SubscriptionsPlansQueryService>();
builder.Services.AddScoped<ISubscriptionPlansRepository, JsonSubscriptionPlansRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();
builder.Services.AddScoped<ISkuAndPriceIdConverter, SkuAndStipePriceConverter>();

//      |: Asset and Resource Management Bounded Context Injection Configuration
builder.Services.AddScoped<IPotRepository, PotRepository>();
builder.Services.AddScoped<IPotCommandService, PotCommandService>();
builder.Services.AddScoped<IPotQueryService, PotQueryService>();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    // var seeder = new PlantDataSeeder();
    // var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "Plants.json");
    // await seeder.SeedAsync(context, jsonFilePath);
    context.Database.EnsureCreated();
}

//  if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");
app.UseRequestAuthorization();
app.UseCustomExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();