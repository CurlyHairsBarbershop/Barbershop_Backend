using Core;
using DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidAudience = builder.Configuration.GetSection("Jwt:ValidAudience").Value,
        ValidIssuer = builder.Configuration.GetSection("Jwt:ValidIssuer").Value
    };
});

builder.Services.TryAddScoped(typeof(IAuthService<>), typeof(AuthService<>));
builder.Services.AddControllers();

// builder.Configuration.GetSection("Jwt").Bind(new JwtProviderOptions());
builder.Services.Configure<JwtProviderOptions>(opt =>
{
    opt.Key = builder.Configuration.GetSection("Jwt:Key").Value ?? throw new InvalidDataException("jwt key was not provided");
    opt.ValidAudience = builder.Configuration.GetSection("Jwt:ValidIssuer").Value ?? throw new InvalidCastException("jwt issuer was not provided");
    opt.ValidAudience =  builder.Configuration.GetSection("Jwt:ValidAudience").Value ?? throw new InvalidDataException("audience was not provided");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();