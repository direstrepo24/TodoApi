Política de Implementación de Jobs Utilizando Kubernetes

Objetivo
El propósito de esta política es estandarizar la implementación de tareas automatizadas y trabajos batch dentro de nuestras aplicaciones .NET Core, delegando exclusivamente la gestión y ejecución de estas tareas a Kubernetes y no a través de librerías internas de .NET Core. Esta aproximación asegura la coherencia, eficiencia y escalabilidad en la gestión de trabajos.

Alcance
Esta política aplica a todos los equipos de desarrollo que implementen aplicaciones .NET Core que requieran ejecución de tareas repetitivas o batch, asegurándose de que dichas tareas se configuren y gestionen a través de Jobs y CronJobs de Kubernetes.

Justificación

1. Separación de preocupaciones: Al utilizar Kubernetes para la gestión de jobs, se separa la lógica de la aplicación de la lógica de planificación y ejecución de tareas. Esto mejora la mantenibilidad y escalabilidad del código.
2. No dependencia de librerías específicas: Evita la dependencia en librerías de terceros de .NET Core para la gestión de trabajos, lo cual reduce la complejidad del proyecto y minimiza el riesgo de incompatibilidades o problemas relacionados con actualizaciones de dichas librerías.
3. Escalabilidad y flexibilidad: Kubernetes ofrece capacidades avanzadas de escalado y gestión de recursos que superan las posibilidades de la mayoría de las librerías de jobs en .NET Core. Puede gestionar fácilmente el escalado horizontal de tareas según la demanda.
4. Resiliencia y alta disponibilidad: Kubernetes proporciona mecanismos integrados para manejo de fallos y reintentos, asegurando que los jobs se completen exitosamente incluso en caso de errores transitorios o fallos de hardware.
5. Consistencia entre entornos: Usar Kubernetes para la ejecución de jobs asegura que el comportamiento sea consistente a través de todos los entornos de desarrollo, pruebas y producción, dado que la infraestructura y las políticas de ejecución son gestionadas de manera uniforme.
6. Monitorización y logging centralizados: Kubernetes facilita la integración con sistemas de monitoreo y logging centralizados, permitiendo una visibilidad completa sobre la ejecución y el estado de los jobs.

Responsabilidades

* DevOps: Supervisar la implementación y configuración de Kubernetes, asegurando que las prácticas y herramientas estén alineadas con los objetivos empresariales.
* Arquitectura: Definir y revisar la arquitectura necesaria para la implementación eficiente de jobs, garantizando que se adhiera a las mejores prácticas y estándares de la industria.
* Desarrollo: Asegurarse de que las aplicaciones .NET Core se diseñen sin incluir librerías de manejo de jobs internos, y que cualquier necesidad de ejecución de tareas programadas sea delegada a Kubernetes.
* Calidad: Validar que los jobs configurados en Kubernetes cumplan con los criterios de aceptación y los requisitos de calidad antes de su despliegue en producción.
* Seguridad: Evaluar y aplicar políticas de seguridad para proteger los jobs y la infraestructura de Kubernetes, incluyendo la gestión de secretos y accesos.
* Líder de Célula: Coordinar entre los equipos para asegurar la adopción y el cumplimiento de la política, resolviendo cualquier conflicto o barrera que pueda surgir durante la implementación.


Implementación

* Definición de Jobs y CronJobs: Todos los jobs deberán ser definidos utilizando las especificaciones de Jobs y CronJobs de Kubernetes, según la necesidad de ejecución única o periódica de las tareas.
* Prohibición del uso de librerías para jobs: Las aplicaciones .NET Core no deberán implementar librerías o frameworks que manejen la programación y ejecución de tareas programadas internamente. Toda funcionalidad relacionada con la ejecución de tareas en el tiempo debe ser delegada a Kubernetes.
* Documentación y capacitación: Se proporcionará documentación detallada y capacitaciones sobre cómo implementar y gestionar jobs en Kubernetes, asegurando que todos los equipos de desarrollo comprendan y apliquen correctamente esta política.

Cumplimiento

El cumplimiento de esta política será monitoreado por los equipos de DevOps y el CoE de Ingeniería de software, quienes revisarán periódicamente los proyectos para asegurarse de que no se utilicen librerías de gestión de jobs internas de .NET Core y que los Jobs y CronJobs de Kubernetes estén correctamente definidos y configurados según los estándares de la organización.
Esta política entra en vigencia inmediatamente y es obligatoria para todos los equipos de desarrollo involucrados en la creación y mantenimiento de aplicaciones .NET Core que requieran la ejecución de tareas automatizadas o batch.


Guía para la Gestión de Jobs en Kubernetes

Esta guía proporciona los lineamientos detallados para configurar y gestionar jobs en Kubernetes, enfocándose en la utilización de imágenes de aplicaciones de consola .NET Core 8. Se abordan las mejores prácticas en la creación de jobs, configuración de crontabs, seguridad, manejo de errores y políticas de reintentos. Todos los procesos de repetición y ejecución en lote deben ser manejados a través de Kubernetes, sin utilizar librerías específicas de manejo de jobs en la aplicación.

Capítulo 1: Introducción a Kubernetes Jobs

Sección 1.1: Conceptos Básicos

Un Job en Kubernetes es un controlador que crea uno o más Pods y asegura que un número especificado de ellos terminen exitosamente. En el contexto de .NET Core, un Job puede ser utilizado para ejecutar tareas batch o procesamientos que requieran ser completados una sola vez o periódicamente.

Sección 1.2: Ventajas de Usar Jobs en Kubernetes
* Automatización y Escalabilidad: Kubernetes maneja la escala y la repetición de jobs sin intervención manual.
* Independencia del Lenguaje y la Plataforma: Puede usar cualquier imagen de contenedor, como .NET Core 8.
* Gestión Centralizada: Permite una administración centralizada de logs, políticas de retry y monitoreo.

Capítulo 2: Configuración de Jobs

Sección 2.1: Creación de un Job Básico

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

Sección 2.2: Configuración de CronJobs para Tareas Periódicas
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

Capítulo 3: Mejores Prácticas y Seguridad
Sección 3.1: Seguridad y Roles
* Roles y RBAC: Definir roles específicos y usar Role-Based Access Control (RBAC) para limitar los permisos que tiene el job dentro del clúster.
* Secretos y ConfigMaps: Usar Secrets y ConfigMaps para manejar configuraciones y secretos, evitando exponer información sensible directamente en los archivos de configuración.
Sección 3.2: Manejo de Logs y Monitoreo
* Logs: Configurar la agregación de logs en Instana para facilitar el monitoreo y la solución de problemas.
* Monitoreo: Utilizar herramientas como Prometheus en Instana para monitorear el desempeño de los jobs y alertar en caso de comportamientos anómalos.
Capítulo 4: Manejo de Errores y Políticas de Reintentos
Sección 4.1: Estrategias de Reintentos
* backoffLimit: Configurar el backoffLimit para limitar el número de reintentos después de un fallo.
* restartPolicy: Definir la política de reinicio adecuada (OnFailure, Never).
Sección 4.2: Rollbacks y Escalabilidad
* Escalabilidad: Asegurar que los jobs puedan escalar de acuerdo con la carga de trabajo.
* Rollbacks: Implementar estrategias para revertir a una versión anterior del job si la nueva versión falla.


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

