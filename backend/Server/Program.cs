using Server.Domain;
using Server.UseCases;
using Server.UserRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("http://localhost:2200", "http://127.0.0.1:5501")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<UserUseCase>();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.MapPost("/users", (UserUseCase userUseCase, User user) =>
{

    if(!string.IsNullOrWhiteSpace(user.Username)){

        try
        {   
            Console.WriteLine("Verificacao deu errrado");
            userUseCase.CreateUser(user.Username, user.Number, user.Password);
            return Results.Created($"/users/{user.Number}", user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro {ex.Message}");
            return Results.BadRequest(new { ex.Message });
        }

    }else{
        try
        {
            Console.WriteLine("Até aqui deu certo kkkkkkk");
            var login = userUseCase.GetUserByNumber(user.Number);
            Console.WriteLine(login);

            return Results.Created("", login);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro {ex.Message}");
            return Results.BadRequest(new { ex.Message });
        }
    }

    
});

app.MapGet("/users/{number}", (UserUseCase userUseCase, string number) =>
{
    var user = userUseCase.GetUserByNumber(number);
    return user is not null ? Results.Json(user) : Results.NotFound(new { Message = "User not found." });
});

app.MapPut("/users", (UserUseCase userUseCase, User user) =>
{
    try
    {
        userUseCase.UpdateUser(user);
        return Results.Ok(new { Message = "User updated successfully." });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { ex.Message });
    }
});

app.MapDelete("/users/{number}", (UserUseCase userUseCase, string number) =>
{
    try
    {
        userUseCase.DeleteUser(number);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { ex.Message });
    }
});

app.Run();
