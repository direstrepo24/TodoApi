using Serilog;
using Serilog.Enrichers.Sensitive;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Exporter.Instana;

using Serilog.Sinks.OpenTelemetry;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using OpenTelemetry.Logs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var serviceName = "TodoApiAppDemoInit";
var serviceVersion = "1.0.0";
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(serviceName)

    .SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
            .AddHttpClientInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter()
    		.AddInstanaExporter()

    .Build();
// Add services to the container.


const string outputTemplate =
    "[{Level:w}]: {Timestamp:dd-MM-yyyy:HH:mm:ss} {MachineName} {EnvironmentName} {SourceContext} {Message}{NewLine}{Exception}";


var tracingOtlpEndpoint = builder.Configuration.GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT")
                           ?? Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT")
                           ?? "http://collector.default.svc:4317";

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
	.MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .WriteTo.Console(outputTemplate: outputTemplate)
    .WriteTo.OpenTelemetry(options =>
    {
        if (Uri.TryCreate(tracingOtlpEndpoint, UriKind.Absolute, out var uri))
        {
            options.Endpoint = uri.ToString();
            options.Protocol = OtlpProtocol.Grpc;
        }
        else
        {
            Console.WriteLine($"ADVERTENCIA: El valor de OTEL_EXPORTER_OTLP_ENDPOINT no es una URI válida. Usando el valor predeterminado.");
        }
		 options.ResourceAttributes = new Dictionary<string, object>
        {
            ["app"] = "webapi",
            ["runtime"] = "dotnet",
            ["service.name"] = "TodoWebApi"
        };
    })
    .CreateLogger();




    /*  .WriteTo.OpenTelemetry(options =>
    {
        // Configuración para el exportador gRPC
        options.Endpoint = "https://opentelemetry-collector:4317"; // Asume que el Collector está escuchando en el puerto 4317 para gRPC
        options.Protocol = OtlpProtocol.Grpc;
    })  */
	Log.Information("Log de prueba enviado a OpenTelemetry Collector");
	Log.Warning("Log de prueba enviado a OpenTelemetry Collector");

builder.Host.UseSerilog();


var app = builder.Build();


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


