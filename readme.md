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
Administrar la deprecación de aplicaciones, APIs y artefactos tecnológicos de manera segura y estratégica, evaluando y mitigando los riesgos operacionales y financieros asociados.
Versionamiento Semántico
Utilización del esquema MAJOR.MINOR.PATCH para versionar el software, identificando claramente las actualizaciones mayores que pueden implicar riesgos significativos.
Proceso de Deprecación
1. Inicio del Proceso
    * Disparador: Líder Técnico detecta la necesidad de deprecación.
    * Roles involucrados: Líder Técnico
    * Actividades: Convocar a una reunión inicial con Arquitectos y el CoE de Ingeniería de Software para discutir la viabilidad técnica y la necesidad de deprecación.
2. Evaluación de Riesgos
    * Roles involucrados: Equipo de Riesgos
    * Actividades: Análisis de los riesgos financieros y operacionales asociados con la deprecación propuesta, como impacto en clientes, pérdida de datos o interrupciones de servicio.
    * Control: Informe de riesgos con recomendaciones para mitigar impactos negativos, validado por el Líder Técnico y los Arquitectos.
3. Planificación de la Deprecación
    * Roles involucrados: Arquitectos, Líder Técnico, CoE de Ingeniería de Software
    * Actividades: Desarrollar un plan de deprecación que incluya las recomendaciones del equipo de riesgos.
    * Control: Revisión del plan por parte del CoE para asegurar la alineación con las políticas de la empresa.
4. Desarrollo e Implementación
    * Roles involucrados: Equipo de Desarrollo, DevOps
    * Actividades: Implementación de los cambios técnicos necesarios y actualización de la documentación.
    * Control: Pruebas de calidad y seguridad para garantizar la implementación efectiva sin interrupciones.
5. Comunicación de Deprecación
    * Roles involucrados: Líder Técnico, Equipo de Marketing
    * Actividades: Informar a usuarios internos y externos sobre la deprecación y las alternativas disponibles.
    * Control: No es necesario un control específico por parte del CoE o equipo de riesgos aquí.
6. Transición y Soporte
    * Roles involucrados: DevOps, Equipo de Soporte
    * Actividades: Facilitar la transición de los usuarios a nuevas versiones y resolver incidencias relacionadas con la deprecación.
    * Control: Monitoreo continuo durante la transición para identificar y mitigar problemas emergentes.
7. Evaluación y Retiro Final
    * Roles involucrados: Líder Técnico, Seguridad
    * Actividades: Confirmación de la eliminación completa de las versiones deprecadas y evaluación de cualquier impacto residual.
    * Control: Revisión final del proceso para documentar lecciones aprendidas y asegurar que no quedan riesgos sin mitigar.

opcion 2 detallada:

Fases del Proceso de Deprecación
1. Análisis y Planificación Inicial
    * Roles involucrados: Líder Técnico, CoE de Ingeniería de Software
    * Actividades:
        * Identificación de componentes a deprecar basado en análisis de obsolescencia y demanda.
        * Evaluación inicial de impacto considerando tipo de software y usuarios afectados.
    * Control: Documentación de la decisión de deprecación con justificaciones claras y aprobación del plan inicial.
2. Evaluación de Riesgos
    * Roles involucrados: Equipo de Riesgos
    * Actividades:
        * Evaluación de riesgos operacionales y financieros específicos para cada tipo de deprecación.
        * Sugerencias de mitigación.
    * Control: Revisión y aprobación del informe de riesgos con planes de acción específicos.
3. Desarrollo de Plan de Deprecación Detallado
    * Roles involucrados: Arquitectos, Líder Técnico, CoE de Ingeniería de Software, Equipo de Riesgos
    * Actividades:
        * Elaboración de cronogramas diferenciados para deprecación de APIs, aplicaciones y artefactos.
        * Definición de estrategias de comunicación y soporte técnico post-deprecación.
    * Control: Aprobación del plan detallado y revisión de su alineación con la estrategia tecnológica global.
4. Implementación y Seguimiento
    * Roles involucrados: Equipo de Desarrollo, DevOps
    * Actividades:
        * Implementación de la deprecación según lo planificado.
        * Monitoreo del proceso para ajustar la estrategia según sea necesario.
    * Control: Auditorías periódicas para asegurar cumplimiento y efectividad.
5. Comunicación y Gestión del Cambio
    * Roles involucrados: Líder Técnico, Equipo de Marketing, CoE de Ingeniería de Software
    * Actividades:
        * Comunicación clara y transparente a todas las partes interesadas internas y externas.
        * Capacitaciones y soporte para transiciones suaves a nuevas versiones o alternativas.
    * Control: Evaluación del impacto de las comunicaciones y ajustes basados en feedback.
6. Cierre y Revisión Post-Deprecación
    * Roles involucrados: Líder Técnico, Equipo de Riesgos, Seguridad
    * Actividades:
        * Confirmación de que todos los sistemas afectados están operando sin la versión deprecada.
        * Análisis de incidentes para aprender de cada proceso de deprecación.
    * Control: Revisión final y reporte de cierre, incluyendo lecciones aprendidas y recomendaciones para futuros procesos.

