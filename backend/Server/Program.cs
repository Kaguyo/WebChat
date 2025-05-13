using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Domain.Entities;
using Server.Repositories;
using Server.UseCases;
using Server.UserRepositories;

var builder = WebApplication.CreateBuilder(args);
var jwtKey = builder.Configuration["Jwt:Key"];
var keyBytes = Encoding.ASCII.GetBytes(jwtKey!);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        builder =>
            builder
                .WithOrigins("http://localhost:2200", "http://127.0.0.1:5501")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader()
    );
});

builder.Services.AddDbContext<AppDbContext>(x =>
    // x.UseNpgsql(@"Host=localhost;Port=5433;Username=docker;Password=docker;Database=connect")
    x.UseSqlite("Data Source=UsersDB.db")
);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>{
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });

    // Adiciona suporte a Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header.  
Exemplo: 'Bearer {seu_token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddScoped<UserUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // abre em /swagger
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();

app.MapPost(
    "/users",
    async (UserUseCase userUseCase, User user) =>
    {
        try
        {
            var userId = await userUseCase.CreateUser(user);
            return Results.Created($"/users/{user.Number}", userId);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { ex.Message });
        }
    }
);

app.MapPost(
    "/users/login",
    async (UserUseCase userUseCase, User user) =>
    {
        var login = await userUseCase.GetUserByNumber(user.Number, user.Password);
        return login != null
            ? Results.Ok(login.Username)
            : Results.NotFound(new { Message = "Number or password wrong" });
    }
);

app.MapPut(
    "/users",
    async (UserUseCase userUseCase, User user) =>
    {
        try
        {
            await userUseCase.UpdateUser(user);
            return Results.Ok(new { Message = "User updated successfully." });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { ex.Message });
        }
    }
);

app.MapDelete(
    "/users/{number}",
    async (UserUseCase userUseCase, int id) =>
    {
        try
        {
            await userUseCase.DeleteUser(id);
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { ex.Message });
        }
    }
);

app.Run();
