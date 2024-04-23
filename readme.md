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


Deprecación de apis y apps


Proceso de Manejo de Deprecación de Aplicaciones Usando Versionamiento Semántico
Objetivo
Facilitar la deprecación ordenada de APIs, aplicaciones y artefactos tecnológicos en una compañía de financiamiento, basándose en el versionamiento semántico.
Versionamiento Semántico
Se utiliza el esquema de versionamiento MAJOR.MINOR.PATCH, donde:
* MAJOR: Incremento por cambios que no son retrocompatibles.
* MINOR: Incremento por nuevas funcionalidades compatibles.
* PATCH: Incremento por correcciones de errores compatibles.
Proceso de Deprecación
1. Inicio del Proceso
    * Disparador: Líder Técnico identifica la necesidad de deprecación.
    * Roles involucrados: Líder Técnico
    * Actividades: Convocar a una reunión inicial con Arquitectos y Equipo de Desarrollo para discutir la obsolescencia y planificación de la deprecación.
2. Planificación de la Deprecación
    * Roles involucrados: Arquitectos, Líder Técnico
    * Actividades: Definir el alcance de la deprecación, seleccionar las versiones a deprecar y elaborar un cronograma detallado.
    * Control: Aprobación del plan de deprecación por los Arquitectos y el Líder Técnico.
3. Desarrollo e Implementación
    * Roles involucrados: Equipo de Desarrollo, DevOps
    * Actividades: Implementar la deprecación en el código, actualizar la documentación y preparar el sistema para el retiro de las versiones afectadas.
    * Control: Revisión de código y pruebas de regresión para validar los cambios.
4. Comunicación de Deprecación
    * Roles involucrados: Líder Técnico, Equipo de Marketing
    * Actividades: Notificar a los usuarios sobre la deprecación planificada y proporcionar información sobre las versiones alternativas o actualizaciones necesarias.
5. Transición y Soporte
    * Roles involucrados: DevOps, Equipo de Soporte
    * Actividades: Asistir a los usuarios en la transición a nuevas versiones y resolver incidencias relacionadas con la deprecación.
6. Evaluación y Retiro Final
    * Roles involucrados: Líder Técnico, Seguridad
    * Actividades: Verificar que todas las instancias de la versión deprecada se han retirado y que los usuarios han migrado a versiones soportadas.
    * Control: Confirmación del retiro completo y asegurar que no quedan resquicios de las versiones deprecadas.

