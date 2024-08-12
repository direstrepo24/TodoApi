#migracion 3.1 a 6

```c#
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

