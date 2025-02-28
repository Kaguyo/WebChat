using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Server.Domain.Entities;
using Server.Repositories;
using Server.UseCases;
using Server.UserRepositories;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SqliteEFCore", Version = "v1" });
});

builder.Services.AddScoped<UserUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseCors("AllowFrontend");

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
