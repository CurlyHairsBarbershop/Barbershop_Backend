using System.Text;
using Core;
using DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Models.Options;
using Presentation.Services.AuthService;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
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
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidAudience = builder.Configuration.GetSection("Jwt:ValidAudience").Value,
        ValidIssuer = builder.Configuration.GetSection("Jwt:ValidIssuer").Value,
        IssuerSigningKey = key
    };
});

builder.Services.TryAddScoped(typeof(IAuthService<>), typeof(AuthService<>));
builder.Services.AddControllers();

// builder.Configuration.GetSection("Jwt").Bind(new JwtProviderOptions());
builder.Services.Configure<JwtProviderOptions>(opt =>
{
    opt.Key = secret;
    opt.ValidAudience = builder.Configuration.GetSection("Jwt:ValidIssuer").Value ?? throw new InvalidCastException("jwt issuer was not provided");
    opt.ValidAudience =  builder.Configuration.GetSection("Jwt:ValidAudience").Value ?? throw new InvalidDataException("audience was not provided");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter bearer token",
        Name = "JwtAuthorization",
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

InitializeDatabase(app, builder);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

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