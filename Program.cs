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
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry()
    .CreateLogger();

var logger = Log.Logger;

int RollDice()
{
    return Random.Shared.Next(1, 7);
}

string HandleRollDice(string? player)
{
    var result = RollDice();

    if (string.IsNullOrEmpty(player))
    {
        logger.Information($"Anonymous player is rolling the dice: {result}", result.ToString());
    }
    else
    {
        logger.Information("{player} is rolling the dice: {result}", player, result);
    }


    switch (result)
    {
        case < 3:
            logger.Error($"Lower value {result}");
            break;
        case < 4:
            logger.Fatal($"Medium value {result}");
            break;
        default:
            logger.Warning($"High value {result}");
            break;
    }

    return result.ToString(CultureInfo.InvariantCulture);
}

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog();
});

var app = builder.Build();

app.MapGet("/prueba/{player?}", HandleRollDice);
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


