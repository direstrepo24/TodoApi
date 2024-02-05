# Fase de compilación utilizando el SDK de .NET 6 Alpine
# FROM mcr.microsoft.com/dotnet/sdk:6.0
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env
WORKDIR /app

# Copia los archivos csproj y restaura las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copia los demás archivos del proyecto y construye la aplicación
COPY . ./
RUN dotnet publish -c Release -o out

#Setup de Otel instrumentacion automatica



# Fase de ejecución utilizando la imagen runtime de .NET 6 Alpine
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .

#Export de variables Opentelemetry


ENV OTEL_TRACES_EXPORTER=otlp \
    OTEL_METRICS_EXPORTER=otlp \
    OTEL_LOGS_EXPORTER=otlp \
    OTEL_EXPORTER_OTLP_PROTOCOL=grpc \
    OTEL_DOTNET_AUTO_TRACES_CONSOLE_EXPORTER_ENABLED=true \
    OTEL_DOTNET_AUTO_METRICS_CONSOLE_EXPORTER_ENABLED=true \
    OTEL_DOTNET_AUTO_LOGS_CONSOLE_EXPORTER_ENABLED=true

#Setup de Otel instrumentacion automatica

RUN apk update && apk add unzip && apk add curl && apk add bash
RUN mkdir /otel
RUN curl -L -o /otel/otel-dotnet-auto-install.sh https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/latest/download/otel-dotnet-auto-install.sh
RUN chmod +x /otel/otel-dotnet-auto-install.sh
ENV OTEL_DOTNET_AUTO_HOME=/otel
RUN /bin/bash /otel/otel-dotnet-auto-install.sh


#Ejecucion de app utilizando script de instrumentacion automatica
ENTRYPOINT ["/bin/bash", "-c", "source /otel/instrument.sh && dotnet TodoApi.dll"]
