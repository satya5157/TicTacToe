using System.Text.Json.Serialization;
using TicTacToe.Api.Services.ComputerStrategies;
using TicTacToe.Api.Services;
using TicTacToe.Api.Services.UndoPolicies;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<IComputerMoveStrategy, EasyComputerMoveStrategy>();
builder.Services.AddSingleton<IComputerMoveStrategy, MediumComputerMoveStrategy>();
builder.Services.AddSingleton<IComputerMoveStrategy, HardComputerMoveStrategy>();
builder.Services.AddSingleton<IComputerMoveStrategyFactory, ComputerMoveStrategyFactory>();

builder.Services.AddSingleton<IUndoPolicy, TwoPlayerUndoPolicy>();
builder.Services.AddSingleton<IUndoPolicy, VsComputerUndoPolicy>();

builder.Services.AddSingleton<IGameService>(provider =>
    new GameService(
        provider.GetRequiredService<IComputerMoveStrategyFactory>(),
        provider.GetServices<IUndoPolicy>()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
