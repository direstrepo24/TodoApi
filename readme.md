```yaml

apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: modular-monolith-ingress
  annotations:
    cert-manager.io/cluster-issuer: "letsencrypt-prod"
    nginx.ingress.kubernetes.io/enable-cors: "true"
    nginx.ingress.kubernetes.io/rewrite-target: /$1
    nginx.ingress.kubernetes.io/configuration-snippet: |
      if ($uri ~* "^/api") {
        more_set_headers "Content-Security-Policy: default-src 'none'; script-src 'self'; connect-src 'self';";
        more_set_headers "X-Frame-Options: DENY";
        more_set_headers "X-Content-Type-Options: nosniff";
      }
      if ($uri ~* "^/paty(/|$)(.*)") {
        more_set_headers "Content-Security-Policy: default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline';";
        more_set_headers "Strict-Transport-Security: max-age=63072000; includeSubDomains; preload";
        more_set_headers "Referrer-Policy: strict-origin-when-cross-origin";
      }
spec:
  tls:
    - hosts:
        - "your-domain.com"
      secretName: your-domain-tls
  rules:
    - host: "your-domain.com"
      http:
        paths:
          - path: /api/(.*)
            pathType: ImplementationSpecific
            backend:
              service:
                name: monolith-service
                port:
                  number: 80
          - path: /paty(/|$)(.*)
            pathType: ImplementationSpecific
            backend:
              service:
                name: monolith-service
                port:
                  number: 80

```

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: modular-monolith-ingress
  annotations:
    nginx.ingress.kubernetes.io/enable-cors: "true"
    nginx.ingress.kubernetes.io/configuration-snippet: |
      if ($request_uri ~* ^/api) {
        more_set_headers "Content-Security-Policy: default-src 'none'; script-src 'self'; connect-src 'self';";
        more_set_headers "X-Frame-Options: DENY";
        more_set_headers "X-Content-Type-Options: nosniff";
      }
      if ($request_uri ~* ^/) {
        more_set_headers "Content-Security-Policy: default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline';";
        more_set_headers "Strict-Transport-Security: max-age=63072000; includeSubDomains; preload";
        more_set_headers "Referrer-Policy: strict-origin-when-cross-origin";
      }
spec:
  rules:
  - host: myapp.example.com
    http:
      paths:
      - path: /api
        pathType: Prefix
        backend:
          service:
            name: api-service
            port:
              number: 80
      - path: /
        pathType: Prefix
        backend:
          service:
            name: frontend-service
            port:
              number: 80
  tls:
  - hosts:
    - myapp.example.com
    secretName: tls-secret

```
```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: modular-monolith-ingress
  annotations:
    cert-manager.io/cluster-issuer: "letsencrypt-prod"
    nginx.ingress.kubernetes.io/rewrite-target: /$1
spec:
  tls:
    - hosts:
        - "your-domain.com"
      secretName: your-domain-tls
  rules:
    - host: "your-domain.com"
      http:
        paths:
          - path: /api/(.*)
            pathType: ImplementationSpecific
            backend:
              service:
                name: backend-service
                port:
                  number: 80
            path:
              metadata:
                annotations:
                  nginx.ingress.kubernetes.io/configuration-snippet: |
                    more_set_headers "Strict-Transport-Security: max-age=31536000; includeSubDomains";
                    more_set_headers "X-Frame-Options: SAMEORIGIN";
                    more_set_headers "X-Content-Type-Options: nosniff";
                    more_set_headers "X-XSS-Protection: 1; mode=block";
                    more_set_headers "Content-Security-Policy: default-src 'self'; script-src 'self'; object-src 'none';";
          - path: /(.*)
            pathType: ImplementationSpecific
            backend:
              service:
                name: frontend-service
                port:
                  number: 80
            path:
              metadata:
                annotations:
                  nginx.ingress.kubernetes.io/configuration-snippet: |
                    more_set_headers "Strict-Transport-Security: max-age=31536000; includeSubDomains";
                    more_set_headers "X-Frame-Options: DENY";
                    more_set_headers "X-Content-Type-Options: nosniff";
                    more_set_headers "X-XSS-Protection: 1; mode=block";
                    more_set_headers "Content-Security-Policy: default-src 'self'; img-src 'self' data:; style-src 'self' 'unsafe-inline';";

```

```c#
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddRazorPages();  // Si usas Razor para el front-end
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                // API specific security headers
                context.Response.Headers["Content-Security-Policy"] = "default-src 'none'; script-src 'self'; connect-src 'self';";
                context.Response.Headers["X-Frame-Options"] = "DENY";
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            }
            else
            {
                // Frontend specific security headers
                context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline';";
                context.Response.Headers["Strict-Transport-Security"] = "max-age=63072000; includeSubDomains; preload";
                context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            }
            await next();
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();  // Si usas Razor para el front-end
        });
    }
}

```

-  Creación de un Job Básico

apiVersion: batch/v1
kind: Job
metadata:
  name: ejemplo-job
spec:
  template:
    spec:
      containers:
      - name: ejemplo
        image: imagen-dotnetcore8
        command: ["dotnet", "MiAplicacion.dll"]
      restartPolicy: Never
  backoffLimit: 4

- Configuración de CronJobs para Tareas Periódicas
Para ejecutar jobs de manera periódica, utilizamos CronJobs:

apiVersion: batch/v1beta1
kind: CronJob
metadata:
  name: cronjob-ejemplo
spec:
  schedule: "0 0 * * *"
  jobTemplate:
    spec:
      template:
        spec:
          containers:
          - name: ejemplo
            image: imagen-dotnetcore8
            command: ["dotnet", "MiAplicacion.dll"]
          restartPolicy: OnFailure
  concurrencyPolicy: Replace
  startingDeadlineSeconds: 200


Paso 1: Crear una Aplicación de Consola .NET Core
Primero, crea un proyecto de consola .NET Core.
1. Usa el template de .net core para aplicaciones de consolar:
dotnet new console -n MiAplicacion
2. El dockerfile debe tern las siguiente características (solo para nube)

# Utiliza la imagen SDK para compilar el código
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copia csproj y restaura dependencias
COPY *.csproj ./
RUN dotnet restore

# Copia los archivos restantes y compila
COPY . ./
RUN dotnet publish -c Release -o out

# Genera la imagen de runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MiAplicacion.dll"]

Paso 3: Construir y Probar el Contenedor
Construye la imagen Docker y prueba localmente:

docker build -t miaplicacion:latest . docker run miaplicacion:latest

docker build -t miaplicacion:latest .

Paso 5: Deployment de Kubernetes y CronJob
Crea un archivo cronjob.yaml con el siguiente contenido:

apiVersion: batch/v1
kind: CronJob
metadata:
  name: miaplicacion-cronjob
spec:
  schedule: "*/1 * * * *"  # Ejecuta cada minuto
  jobTemplate:
    spec:
      template:
        spec:
          containers:
          - name: miaplicacion
            image: miaplicacion:latest
          restartPolicy: OnFailure


Aplicar el deployment del crab
kubectl apply -f cronjob.yaml


Paso 6: Verificar la Ejecución
Verifica que el CronJob está ejecutándose correctamente:
1. Listar los jobs:bash   kubectl get jobs   
2. Ver los logs de un pod específico (reemplaza <pod-name> con el nombre real obtenido):bash   kubectl logs <pod-name>


helm upgrade instana-agent \
   --repo https://agents.instana.io/helm \
   --namespace instana-agent \
   --create-namespace \
   --set agent.key=m2VxAzQJRUWvpTYEqnltvA \
   --set agent.downloadKey=m2VxAzQJRUWvpTYEqnltvA \
   --set agent.endpointHost=ingress-coral-saas.instana.io \
   --set agent.endpointPort=443 \
   --set cluster.name='my-instnet-cluster' \
   --set zone.name='Agent zone777' \
  --set opentelemetry.enabled=true \
  --set opentelemetry.grpc.enabled=true \
  --set opentelemetry.http.enabled=true \
   instana-agent


   apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: ingress-mi-aplicacion-web
  annotations:
    nginx.ingress.kubernetes.io/rewrite-target: /
    nginx.ingress.kubernetes.io/secure-backends: "true"
    nginx.ingress.kubernetes.io/add-headers: |
      Strict-Transport-Security "max-age=31536000; includeSubDomains"
      X-Frame-Options "DENY"
      X-Content-Type-Options "nosniff"
      Content-Security-Policy "script-src 'self'; object-src 'self'"
      X-XSS-Protection "1; mode=block"
spec:
  rules:
  - http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: mi-servicio-web
            port:
              number: 80

Para front:

Este ejemplo incluye configuraciones para las siguientes cabeceras de seguridad:

- `X-Content-Type-Options`: Evita que el navegador mime-snorfee (adivine) el tipo de contenido.
- `Set-Cookie`: Establece cookies con los atributos `Secure`, `HttpOnly` y `SameSite=Strict`.
- `Feature-Policy`: Controla el acceso a las funciones y APIs del navegador.
- `Referrer-Policy`: Controla cómo se incluye la información del referente en las solicitudes HTTP.

  con los valores asi:
- X-Content-Type-Options: nosniff es el valor utilizado en esta cabecera. Cuando esta cabecera está presente con el valor nosniff, le indica al navegador que no intente adivinar o "snifear" el tipo de contenido basándose en el contenido real del archivo, lo que ayuda a prevenir ataques de tipo MIME-sniffing.
- Set-Cookie: En esta cabecera, el valor name=value es el nombre y el valor de la cookie que se está estableciendo. Los atributos HttpOnly, Secure y SameSite=Strict indican que la cookie solo debe ser accesible a través de HTTP (no JavaScript), solo debe ser enviada sobre conexiones seguras (HTTPS) y solo debe ser enviada si el sitio web de origen coincide exactamente con el destino de la solicitud, respectivamente.
- Feature-Policy: En este caso, se establece una política de características que restringe el acceso a ciertas funciones y APIs del navegador. Con "geolocation 'self'; microphone 'self'", se permite el uso de la geolocalización y el micrófono solo desde el propio sitio web ('self').
- Referrer-Policy: El valor strict-origin en esta cabecera indica que solo se enviará el referente en las solicitudes si el protocolo de la solicitud es HTTPS y si el dominio de origen coincide exactamente con el destino de la solicitud.

Recuerda ajustar los valores según las necesidades específicas de tu aplicación y las prácticas recomendadas de seguridad.

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: my-ingress
  annotations:
    nginx.ingress.kubernetes.io/configuration-snippet: |
      add_header X-Content-Type-Options nosniff;
      add_header Set-Cookie "name=value; HttpOnly; Secure; SameSite=Strict";
      add_header Feature-Policy "geolocation 'self'; microphone 'self'";
      add_header Referrer-Policy "strict-origin";
spec:
  rules:
    - host: example.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: my-service
                port:
                  number: 80
´´´

Donde:

erDiagram
    Customer ||--o{ CallLog : "logs"
    Customer ||--o{ Payment : "payments"

    Customer {
        string customerId "ID único"
        string name "Nombre del cliente"
        string address "Dirección"
        string phoneNumber "Número de teléfono"
        CallLog calls "Historial de llamadas (Documentos embebidos)"
        Payment payments "Historial de pagos (Documentos embebidos)"
        array tags "Etiquetas para segmentación"
    }

    CallLog {
        dateTime date "Fecha y hora de la llamada"
        string agentId "ID del agente"
        string status "Estado de la llamada (Conectada/No Conectada/No Respondida)"
        boolean repeatedCall "Indica si la llamada es repetida"
        string notes "Notas sobre la llamada"
        InteractionDetails interaction "Detalles sobre la interacción"
        array followUps "Seguimientos (Documentos embebidos)"
    }

    InteractionDetails {
        boolean wasAttended "Si fue atendido"
        string result "Resultado de la llamada (Pago acordado, Promesa de pago, Sin acuerdo)"
        dateTime nextCall "Fecha y hora para la próxima llamada"
    }

    FollowUp {
        dateTime followUpDate "Fecha de seguimiento"
        string type "Tipo de seguimiento (Correo electrónico, SMS, Otra llamada)"
        string status "Estado del seguimiento"
        string result "Resultado del seguimiento"
    }

    Payment {
        dateTime paymentDate "Fecha de pago"
        double amount "Monto pagado"
        string paymentMethod "Método de pago (Tarjeta, Transferencia, Efectivo)"
        string receiptNumber "Número de recibo"
        boolean isConfirmed "Pago confirmado"
    }

