using Serilog;
using Serilog.Enrichers.Sensitive;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Exporter.Instana;

using Serilog.Sinks.OpenTelemetry;


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
var serviceName = "Instana.OpenTelemetryTestApp.TestService";
var serviceVersion = "1.0.0";

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(serviceName)

    .SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
    //.AddSqlClientInstrumentation()
    //.AddConsoleExporter()
    .AddInstanaExporter()
    
    .Build();
// Add services to the container.



Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry(options =>
    {
        // Configuración para el exportador HTTP
        options.Endpoint = "https://opentelemetry-collector:4318"; // Asume que el Collector está escuchando en el puerto 4318 para HTTP
        options.Protocol = OtlpProtocol.HttpProtobuf;
    })
    /*  .WriteTo.OpenTelemetry(options =>
    {
        // Configuración para el exportador gRPC
        options.Endpoint = "https://opentelemetry-collector:4317"; // Asume que el Collector está escuchando en el puerto 4317 para gRPC
        options.Protocol = OtlpProtocol.Grpc;
    })  */
    .CreateLogger();
	Log.Information("Log de prueba enviado a OpenTelemetry Collector");
	Log.Warning("Log de prueba enviado a OpenTelemetry Collector");
  
//init logger

			

   

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


