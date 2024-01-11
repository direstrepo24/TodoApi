using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
             _logger.LogWarning("Seri Log is Working");
          // Ejemplo de información sensible (número de tarjeta de crédito)
            var creditCardNumber = "1234-5678-9012-3456";

            // No agregues información sensible directamente en los registros,
            // utiliza el enriquecimiento para evitar exponerlo.
            _logger.LogWarning("Se ha recibido una solicitud GET en el endpoint /api/example");

            // Enriquecimiento: No exponer información sensible en los registros
            _logger.LogWarning("Número de tarjeta de crédito: {@CreditCardNumber}", new { CreditCardNumber = MaskCreditCard(creditCardNumber) });

            // Otro registro enriquecido
            _logger.LogError("Registro adicional con información enriquecida");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray(); 
        
    }
     private string MaskCreditCard(string creditCardNumber)
        {
            // Lógica para enmascarar el número de tarjeta de crédito
            // Ejemplo: 1234-5678-9012-3456 -> XXXX-XXXX-XXXX-3456
            return "XXXX-XXXX-XXXX-" + creditCardNumber.Substring(creditCardNumber.Length - 4);
        }
}
