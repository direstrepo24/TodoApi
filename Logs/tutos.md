# Decisión de Arquitectura de Software: Adopción de REST Client para Visual Studio Code

## Decisión a Tomar
Adoptar la extensión "REST Client" para Visual Studio Code como herramienta principal para pruebas de APIs REST, reemplazando el uso de Postman y Thunder Client.

## Contexto
El equipo de desarrollo necesita una herramienta eficiente y segura para realizar pruebas de APIs. La autenticación obligatoria en las versiones recientes de Postman y Thunder Client presenta un riesgo de exposición de información sensible.

## Restricciones
- **Seguridad**: La herramienta no debe requerir autenticación que exponga información sensible.
- **Compatibilidad**: Debe ser compatible con la mayoría de los proyectos y tecnologías usadas actualmente por el equipo.
- **Costo**: Preferible que sea una solución sin costo adicional.

## Alternativas
1. **Postman**: Popular pero requiere autenticación en las últimas versiones.
2. **Thunder Client**: Integración con VS Code, pero también requiere autenticación reciente.
3. **REST Client**: Extensión de VS Code que no requiere autenticación para su uso.

## Decisión
Adoptar "REST Client" debido a su integración directa con el entorno de desarrollo y su política de no requerir autenticación para operar.

## Excepciones
En casos donde se requiera una colaboración extensiva y características avanzadas de documentación, se podría considerar el uso de Postman a pesar de sus restricciones.

## Implicaciones
- **Capacitación**: Se requerirá una breve sesión de capacitación para los desarrolladores en el uso de "REST Client".
- **Integración**: Integración directa en VS Code, facilitando el flujo de trabajo de desarrollo.

## Análisis

| Herramienta    | Seguridad (No exposición de datos) | Compatibilidad | Costo  | Versionamiento |
|----------------|-----------------------------------|----------------|--------|----------------|
| Postman        | Bajo (Requiere autenticación)     | Alta           | Medio  | Alta           |
| Thunder Client | Bajo (Requiere autenticación)     | Alta           | Bajo   | Media          |
| REST Client    | Alto (No requiere autenticación)  | Alta           | Bajo   | Baja           |

## Conclusiones
La elección de "REST Client" como herramienta para pruebas de APIs en el entorno de desarrollo de Visual Studio Code se alinea con los requisitos de seguridad, costo y eficiencia del equipo. Esta herramienta proporciona una solución robusta y segura sin la necesidad de manejar autenticaciones que podrían comprometer la información sensible del equipo y la organización.
