#!/bin/bash

# Este es el script de entrada para tu contenedor

# Source del script de instrumentacion
source /otel/instrument.sh

# Ejecución de la aplicación .NET
exec dotnet TodoApi.dll
