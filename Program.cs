using System.Globalization;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
	.MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .CreateLogger();

	

builder.Host.UseSerilog(); */


//var app = builder.Build();
//logs normales
//var logger = app.Logger;

//logs con serilog
// Configuración de Serilog
// Configuración de Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry()
    .CreateLogger();

builder.Logging.ClearProviders(); // Limpia los proveedores existentes
builder.Logging.AddSerilog(); // Agrega Serilog como proveedor
//builder.Host.UseSerilog();
var app = builder.Build();

int RollDice()
{
    return Random.Shared.Next(1, 7);
}

app.MapGet("/prueba/{player?}", async (HttpContext context, ILogger<Program> logger) =>
{

    var result = RollDice();

    if (!context.Request.RouteValues.TryGetValue("player", out var player))
        player = null;

    if (string.IsNullOrEmpty(player?.ToString()))
    {
        logger.LogInformation($"Anonymous player is rolling the dice: {result}", result.ToString());
    }
    else
    {
        logger.LogInformation("{player} is rolling the dice: {result}", player, result);
    }

    switch (result)
    {
        case < 3:
            logger.LogError($"Lower value {result}");
            break;
        case < 4:
            logger.LogCritical($"Medium value {result}");
            break;
        default:
            logger.LogWarning($"High value {result}");
            break;
    }

    await context.Response.WriteAsync(result.ToString(CultureInfo.InvariantCulture));
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


