# Stack de Desarrollo para Infraestructura como Código (IaC)

## 1. Entorno de Desarrollo y Extensiones

**Entorno**: Visual Studio Code  
**Extensiones recomendadas**:
- **HashiCorp Terraform**: Proporciona soporte de autocompletado, validación de sintaxis y comandos integrados para Terraform.

## 2. SDK y Frameworks

**Terraform**: Herramienta de infraestructura como código que permite definir tanto recursos en la nube como on-premise mediante archivos de configuración para facilitar la gestión y reproducibilidad.

## 3. Librerías Oficiales

- **Terraform Provider for Azure**: Permite gestionar recursos de Azure. [Descargar aquí](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs)
- **Terraform CLI**: Herramienta principal para ejecutar Terraform, incluye todo lo necesario para gestionar los ciclos de vida de la infraestructura. [Descargar aquí](https://www.terraform.io/downloads.html)

## 4. Gestión de la Configuración

Utilizamos **Azure Artifacts** para almacenar y gestionar las dependencias de configuración y artefactos de Terraform, asegurando la integridad y la disponibilidad de los mismos.

## 5. Integración y Entrega Continua

Se implementa CI/CD con **Azure Pipelines**, automatizando la creación y despliegue de infraestructura a través de Terraform. Los pipelines están configurados para validar, planear y aplicar cambios de infraestructura de manera segura y eficiente.

## 6. Monitoreo y Logs

Integración con **IBM Instana** para el monitoreo avanzado de la infraestructura desplegada, permitiendo una visibilidad detallada del estado y el rendimiento de los recursos, junto con la gestión centralizada de logs.

## 7. Análisis de Código Estático

**Terraform fmt** y **Terraform validate** se utilizan para asegurar que los archivos de Terraform sigan las mejores prácticas y estén libres de errores de sintaxis, respectivamente.

## 8. Pruebas Unitarias

Las pruebas unitarias se realizan utilizando el framework de Terraform, que permite testear módulos individualmente para garantizar su correcto funcionamiento antes de su despliegue.

## 9. Pruebas Funcionales y No Funcionales

Se emplean herramientas como **Terratest** para escribir y ejecutar pruebas funcionales y no funcionales, asegurando que la infraestructura cumpla con los requerimientos y funcione correctamente bajo diferentes condiciones.
