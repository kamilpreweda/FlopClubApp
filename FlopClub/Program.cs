global using Microsoft.EntityFrameworkCore;
global using System.Text.Json.Serialization;
global using FlopClub.Models;
global using FlopClub.Data;
global using AutoMapper;
global using FlopClub.Dtos.Game;
global using FlopClub.Dtos.User;
global using FlopClub.Services.Encrypter;
global using FlopClub.Services.GameService;
global using FlopClub.Services.TableService;
global using Microsoft.AspNetCore.Authorization;
global using FlopClub.Repositories.Auth;
global using System.ComponentModel.DataAnnotations;
global using FlopClub.Dtos.Role;
global using FlopClub.Services.UserService;
global using FlopClub.Services.RoleService;
global using FlopClub.Services.DeckService;
global using FlopClub.Services.HandEvaluator;
global using FlopClub.Services.GameLogicService;
global using FlopClub.Services.PlayerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = """Standard Authorization header using the Bearer scheme. Example: "bearer {token}" """,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IEncrypter, Encrypter>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IDeckService, DeckService>();
builder.Services.AddScoped<IHandEvaluator, HandEvaluator>();
builder.Services.AddScoped<IGameLogicService, GameLogicService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddHttpContextAccessor();

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
