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
