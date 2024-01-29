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


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//logs normales
// Configurar el logging
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.SetMinimumLevel(LogLevel.Debug); // Puedes ajustar el nivel de log según sea necesario


// // Read environment variables
// var instanaEndpointUrl = Environment.GetEnvironmentVariable("INSTANA_ENDPOINT_URL") ?? "https://serverless-coral-saas.instana.io:4318";
// var instanaAgentKey = Environment.GetEnvironmentVariable("INSTANA_AGENT_KEY") ?? "m2VxAzQJRUWvpTYEqnltvA";

// Custom metrics for the application
var greeterMeter = new Meter("OtPrGrYa.Example", "1.0.0");
var countGreetings = greeterMeter.CreateCounter<int>("greetings.count", description: "Counts the number of greetings");

// Custom ActivitySource for the application
var greeterActivitySource = new ActivitySource("OtPrGrJa.Example");

var tracingOtlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
var otel = builder.Services.AddOpenTelemetry();

// Configure OpenTelemetry Resources with the application name
otel.ConfigureResource(resource => resource
    .AddService(serviceName: builder.Environment.ApplicationName));

// Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
otel.WithMetrics(metrics => metrics
    // Metrics provider from OpenTelemetry
    .AddAspNetCoreInstrumentation()
    .AddMeter(greeterMeter.Name)
    // Metrics provides by ASP.NET Core in .NET 8
    .AddMeter("Microsoft.AspNetCore.Hosting")
    .AddMeter("Microsoft.AspNetCore.Server.Kestrel"));
	
    //.AddPrometheusExporter());

// Add Tracing for ASP.NET Core and our custom ActivitySource and export to Jaeger
otel.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddHttpClientInstrumentation();
    tracing.AddSource(greeterActivitySource.Name);
    if (tracingOtlpEndpoint != null)
    {
        tracing.AddOtlpExporter(otlpOptions =>
         {
             otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
         });
    }
    else
    {
        tracing.AddConsoleExporter();
    }
});

///loger probe
///
/*
	var logger = new LoggerConfiguration()
				.Enrich.WithSensitiveDataMasking(MaskingMode.InArea, new IMaskingOperator[]
                {
                    new EmailAddressMaskingOperator(),
					new IbanMaskingOperator(),
					new CreditCardMaskingOperator(false)
				})
				.WriteTo.Console()
				.CreateLogger();

			logger.Warning("Hello, world");

			using (logger.EnterSensitiveArea())
			{
				// An e-mail address in text
				logger.Warning("This is a secret email address: james.bond@universal-exports.co.uk");

				// Works for properties too
				logger.Warning("This is a secret email address: {Email}", "james.bond@universal-exports.co.uk");

				// IBANs are also masked
				logger.Warning("Bank transfer from Felix Leiter on NL02ABNA0123456789");

				// IBANs are also masked
				logger.Warning("Bank transfer from Felix Leiter on {BankAccount}", "NL02ABNA0123456789");

				// Credit card numbers too
				logger.Warning("Credit Card Number: 4111111111111111");

				// even works in an embedded XML string
				var x = new
				{
					Key = 12345, XmlValue = "<MyElement><CreditCard>4111111111111111</CreditCard></MyElement>"
				};
				logger.Warning("Object dump with embedded credit card: {x}", x);

			}

			// But outside the sensitive area nothing is masked
			logger.Error("Felix can be reached at felix@cia.gov");


			// Now, show that this works for async contexts too
			logger.Error("Now, show the Async works");*/
 

builder.Host.UseSerilog();
//builder.Logging.AddSerilog();

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


