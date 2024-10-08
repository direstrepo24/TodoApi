#migracion 3.1 a 6

```c#

using Microsoft.Extensions.Configuration;
using Azure.Identity;
using System;

public interface IAppConfigurationService
{
    IConfiguration GetConfiguration();
}

public class AppConfigurationService : IAppConfigurationService
{
    private readonly IConfiguration configuration;

    public AppConfigurationService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IConfiguration GetConfiguration()
    {
        return configuration;
    }
}
```
```c#
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Agregar y configurar Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
{
    var connectionString = builder.Configuration.GetValue<string>("AppConfiguration:EndPoint");
    options.Connect(new Uri(connectionString), new DefaultAzureCredential());
    int durationCacheSeconds = builder.Configuration.GetValue<int>("AppConfiguration:DurationCacheSeconds");
    options.ConfigureRefresh(refresh =>
    {
        refresh.Register("Settings:Sentinel", refreshAll: true)
               .SetCacheExpiration(TimeSpan.FromSeconds(durationCacheSeconds));
    });
});

// Registro del servicio de configuración
builder.Services.AddSingleton<IAppConfigurationService, AppConfigurationService>();

var app = builder.Build();

// Configuración del middleware y los endpoints
app.MapGet("/", (IAppConfigurationService configService) =>
{
    var config = configService.GetConfiguration();
    return Results.Ok(config["SomeConfigKey"]);
});

app.Run();

```


using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using System;

namespace Tuya.CobranzaDigital.ClienteCastigado.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (TypeLoadException ex)
            {
                Console.WriteLine($"TypeLoadException: {ex.Message}");
                // Considerar la reconfiguración o reexaminar las dependencias si es recurrente
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception: {ex.Message}");
                // Log general para otras excepciones no manejadas
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var settings = config.Build();
                        int durationCacheSeconds = settings.GetValue<int>("AppConfiguration:DurationCacheSeconds");
                        string connectionString = settings.GetValue<string>("AppConfiguration:EndPoint");

                        try
                        {
                            config.AddAzureAppConfiguration(options =>
                            {
                                options.Connect(new Uri(connectionString), new DefaultAzureCredential());
                                options.ConfigureRefresh(refresh =>
                                {
                                    refresh.Register(key: "Settings:Sentinel", refreshAll: true)
                                    .SetCacheExpiration(TimeSpan.FromSeconds(durationCacheSeconds));
                                });
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error configuring Azure App Configuration: {ex.Message}");
                            // Manejar específicamente los errores de configuración
                        }

                        if (hostingContext.HostingEnvironment.IsDevelopment())
                        {
                            config.AddUserSecrets<Program>();
                        }
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}

```
namespace Tuya.CobranzaDigital.ClienteCastigado.API
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        protected Program() { }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((host, config) =>
                    {
                        var settings = config.Build();
                        int durationCacheSeconds = settings.GetValue<int>("AppConfiguration:DurationCacheSeconds");
                        string connectionString = settings.GetValue<string>("AppConfiguration:EndPoint");
                        config.AddAzureAppConfiguration(options =>
                        {    
                            options.Connect(new Uri(connectionString), new DefaultAzureCredential());
                            options
                            .ConfigureRefresh(refresh =>
                            {
                                refresh.Register(key: "Settings:Sentinel", refreshAll: true)
                                .SetCacheExpiration(TimeSpan.FromSeconds(durationCacheSeconds));
                            });
                        });
                        if (host.HostingEnvironment.IsDevelopment())
                        {
                            config.AddUserSecrets<Program>();
                        }
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
```
# Guia Serenity 
```xml
pom.xml
<dependency>
    <groupId>org.apache.commons</groupId>
    <artifactId>commons-codec</artifactId>
    <version>1.15</version>
</dependency>
<dependency>
    <groupId>net.serenity-bdd</groupId>
    <artifactId>serenity-core</artifactId>
    <version>3.2.0</version>
</dependency>
<dependency>
    <groupId>net.serenity-bdd</groupId>
    <artifactId>serenity-junit</artifactId>
    <version>3.2.0</version>
</dependency>
```
EncryptionUtil:

```Java
import javax.crypto.Cipher;
import javax.crypto.KeyGenerator;
import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;
import org.apache.commons.codec.binary.Base64;

public class EncryptionUtil {

    private static final String ALGORITHM = "AES";

    public static SecretKey generateKey() throws Exception {
        KeyGenerator keyGen = KeyGenerator.getInstance(ALGORITHM);
        keyGen.init(128); // AES-128
        return keyGen.generateKey();
    }

    public static String encrypt(String data, SecretKey key) throws Exception {
        Cipher cipher = Cipher.getInstance(ALGORITHM);
        cipher.init(Cipher.ENCRYPT_MODE, key);
        byte[] encryptedData = cipher.doFinal(data.getBytes());
        return Base64.encodeBase64String(encryptedData);
    }

    public static String decrypt(String encryptedData, SecretKey key) throws Exception {
        Cipher cipher = Cipher.getInstance(ALGORITHM);
        cipher.init(Cipher.DECRYPT_MODE, key);
        byte[] decodedData = Base64.decodeBase64(encryptedData);
        byte[] originalData = cipher.doFinal(decodedData);
        return new String(originalData);
    }
}
```

```Java
import net.serenitybdd.junit.runners.SerenityRunner;
import net.thucydides.core.annotations.Step;
import org.junit.BeforeClass;
import org.junit.Test;
import org.junit.runner.RunWith;

import javax.crypto.SecretKey;

@RunWith(SerenityRunner.class)
public class SoapTest {

    private static SecretKey secretKey;

    @BeforeClass
    public static void setup() throws Exception {
        secretKey = EncryptionUtil.generateKey();
    }

    @Step("Encrypt and send sensitive data")
    public void sendEncryptedData(String data) throws Exception {
        String encryptedData = EncryptionUtil.encrypt(data, secretKey);
        // Aquí agregarías el código para enviar la solicitud SOAP con encryptedData
        System.out.println("Encrypted Data: " + encryptedData);
    }

    @Test
    public void testSendEncryptedIdentification() throws Exception {
        String sensitiveData = "123456789"; // Ejemplo de número de identificación
        sendEncryptedData(sensitiveData);
    }
}
```

