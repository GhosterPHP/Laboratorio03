#Sistema de Gestión de Ventas - Tienda Electrónica

Aplicación de escritorio desarrollada en .NET Windows Forms para la gestión de inventario, ventas y reportes de una tienda de productos electrónicos, aplicando desarrollo por capas, el ORM RepoDb y el patrón Repositorio Genérico.

Características Principales

Autenticación Segura: Inicio de sesión (FrmLogin) contra la base de datos utilizando cifrado SHA-256 para la validación de contraseñas.

Persistencia con RepoDb: Implementación de IRepository<T> y GenericRepository<T> para operaciones CRUD, además de consultas especializadas con mapeo de modelos.

Interfaz y Flujo Modal: Formulario principal (FrmPrincipal) con panel de navegación lateral y formulario hijo modal (FrmProductoModal) abierto mediante ShowDialog() para el alta y edición de productos.

Módulo de Reportes (FrmReportes):

Productos Vendidos: Resumen de cantidades vendidas y montos acumulados.

Stock de Inventario: Consulta general del catálogo y filtro por producto específico.

Bonus - Semáforo de Stock Crítico: Alerta visual automática en las tablas (DataGridView) que resalta productos agotados (stock = 0 en rojo) y con stock crítico (stock <= 5 en amarillo).

Tecnologías Utilizadas

Lenguaje y Framework: C# / .NET Windows Forms

Base de Datos: SQL Server Express (GestionVentas)

ORM: RepoDb (RepoDb.SqlServer)

Estructura del Proyecto

Laboratorio3/
├── Data/       # Configuración de conexión, IRepository<T>, GenericRepository<T> y consultas
├── Forms/      # FrmLogin, FrmPrincipal, FrmProductoModal y FrmReportes
├── Models/     # Clases de entidades mapeadas a SQL Server y DTOs
├── Utils/      # Utilidad de cifrado SHA-256 (SecurityHelper)
└── Program.cs  # Inicialización de RepoDb y punto de entrada


Configuración y Ejecución

Base de datos: Ejecuta el script SQL de la base de datos GestionVentas en tu instancia de SQL Server.

Cadena de conexión: Verifica o ajusta el nombre de tu servidor en Data/Repositorios.cs (DbConfig.ConnectionString), configurado por defecto para MSI\SQLEXPRESS.

Dependencias: Abre la solución en Visual Studio y restaura el paquete NuGet RepoDb.SqlServer.

Ejecutar: Presiona F5 para compilar e iniciar la aplicación.

Credenciales de Prueba

Usuario: admin

Contraseña: admin123
