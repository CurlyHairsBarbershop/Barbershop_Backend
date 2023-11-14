using System.Text;
using BLL.Services.Appointments;
using BLL.Services.Favor;
using BLL.Services.Reviews;
using Core;
using DAL.Context;
using Infrustructure.Extensions.DI.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using API.Models.Options;
using API.Services.AuthService;
using Infrustructure.Extensions.DI.BLL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
// Add services to the container.
builder.Services.AddCors();
builder.Services.AddDbContext<ApplicationContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
});

builder.Services
    .AddIdentityCore<Barber>()
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();
builder.Services
    .AddIdentityCore<Client>()
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

var secret = builder.Configuration.GetSection("Jwt:Key").Value ?? throw new InvalidDataException("jwt key was not provided");
var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
var issuer = builder.Configuration.GetSection("Jwt:ValidIssuer").Value;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.SaveToken = true;
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        //ValidAudience = builder.Configuration.GetSection("Jwt:ValidAudience").Value,
        ValidIssuer = issuer,
        IssuerSigningKey = key
    };
});

builder.Services.TryAddScoped(typeof(IAuthService<>), typeof(AuthService<>));

// INFO: Add business layer
builder.Services.AddBll();

builder.Services.AddControllers();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
});
builder.Services.Configure<JwtProviderOptions>(opt =>
{
    opt.Key = secret;
    opt.ValidIssuer = builder.Configuration.GetSection("Jwt:ValidIssuer").Value ?? throw new InvalidCastException("jwt issuer was not provided");
    opt.ValidAudience =  builder.Configuration.GetSection("Jwt:ValidAudience").Value ?? throw new InvalidDataException("audience was not provided");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions (apiDescriptions => apiDescriptions.First());
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter bearer token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { scheme, Array.Empty<string>() } });
});

var app = builder.Build();
app.UseCors(build => build
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials()
);
InitializeDatabase(app, builder);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static void InitializeDatabase(IApplicationBuilder app, WebApplicationBuilder builder)
{
    using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
    var logger = LoggerFactory.Create(config =>
    {
        config.AddConsole();
        config.AddConfiguration(builder.Configuration.GetSection("Logging"));
        
    }).CreateLogger("Program");
    
    logger.LogInformation("Automigrations started");
    
    try
    {
        var pers = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
        var migrations = pers.Database.GetPendingMigrations();
        if (migrations.Any())
        {
            pers.Database.Migrate();

            foreach (var migration in migrations)
            {
                logger.LogInformation("Migration applied: {MigrationName} {ContextName}", migration, nameof(ApplicationContext));
            }
        }
            
    }
    catch (Exception ex)
    {
           
    }
}