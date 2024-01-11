# Fase de compilación utilizando el SDK de .NET 6 Alpine
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env
WORKDIR /app

# Copia los archivos csproj y restaura las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copia los demás archivos del proyecto y construye la aplicación
COPY . ./
RUN dotnet publish -c Release -o out

# Fase de ejecución utilizando la imagen runtime de .NET 6 Alpine
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .

# Configuración de Instana
ENV DOTNET_STARTUP_HOOKS="/app/Instana.Tracing.Core.dll"
ENV CORECLR_ENABLE_PROFILING="1"
ENV CORECLR_PROFILER="{cf0d821e-299b-5307-a3d8-b283c03916dd}"
ENV CORECLR_PROFILER_PATH="/app/instana_tracing/CoreProfiler.so"
ENV INSTANA_AGENT_HOST="instana-agent.instana-agent.svc.cluster.local"
ENV INSTANA_AGENT_PORT="42699"

# Ajustar si se usa OTLP para enviar datos
# Por ejemplo, para OTLP over HTTP:
ENV TRACER_EXPORTER_OTLP_ENDPOINT="http://$(INSTANA_AGENT_HOST):4317"


ENTRYPOINT ["dotnet", "TodoApi.dll"]

