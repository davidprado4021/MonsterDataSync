## Estructura de API
![](https://github.com/davidprado4021/MonsterDataSync/blob/main/MonsterDataSyncDOC.png)

Implementación de una API en .NET para la Gestión de Datos de Monster Hunter World
Descripción del Proyecto
Este proyecto tiene como objetivo desarrollar una API en .NET que realice solicitudes a la API de Monster Hunter World para obtener datos relacionados con las habilidades (skills) y rangos (ranks) del juego. Posteriormente, estos datos serán almacenados en una base de datos local, sobre la cual se implementarán operaciones CRUD (Crear, Leer, Actualizar, Eliminar).

Objetivos Específicos
Integración con la API de Monster Hunter World:
Realizar solicitudes HTTP a la API de Monster Hunter World para obtener datos actualizados de habilidades y rangos.
Procesar y almacenar los datos obtenidos en una base de datos local.
Implementación de Operaciones CRUD:
Crear endpoints en la API para permitir la creación, lectura, actualización y eliminación de registros de habilidades y rangos.
Asegurar que las operaciones CRUD sean eficientes y seguras.
Diseño y Arquitectura:
Utilizar principios de arquitectura limpia para asegurar un código mantenible y escalable.
Implementar patrones de diseño adecuados para la gestión de datos y la comunicación con la API externa.
Implementar un sistema de logs para monitorear y depurar la API.
Tecnologías Utilizadas
Backend: .NET Core
Base de Datos: SQL Server
API Externa: Monster Hunter World API (mhw-db.com)
Alcance del Proyecto
Fase 1: Configuración del entorno de desarrollo y creación de la estructura básica del proyecto.
Fase 2: Integración con la API de Monster Hunter World y almacenamiento de datos en la base de datos.
Fase 3: Implementación de endpoints CRUD y sistema de logs.
Fase 4: Documentación y despliegue de la API.
Beneficios Esperados
Proveer una herramienta robusta para la gestión de datos de Monster Hunter World.
Facilitar el acceso y la manipulación de datos de habilidades y rangos para desarrolladores y usuarios finales.
Mejorar la eficiencia en la gestión de datos a través de una API bien estructurada y documentada.

## Restaurar la Base de Datos
Para restaurar la base de datos, sigue estos pasos:

1. Abre SQL Server Management Studio (SSMS).
2. Conéctate a tu servidor SQL Server.
3. Abre el archivo `DatabaseScripts/tu_script.sql`.
4. Ejecuta el script para crear y poblar la base de datos.
