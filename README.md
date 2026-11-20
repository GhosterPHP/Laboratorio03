# Sistema de Gestión de Ventas (Windows Forms)

Aplicación de escritorio desarrollada en .NET Windows Forms para la administración de inventario y reportes de una tienda de productos electrónicos, conectada a SQL Server Express mediante el ORM **RepoDb** y el patrón **Repositorio Genérico**.

---

## Tecnologías Utilizadas

* **Lenguaje / Framework:** C# / .NET (Windows Forms)
* **Base de Datos:** SQL Server Express (`GestionVentas`)
* **ORM:** RepoDb (`RepoDb.SqlServer`)
* **Seguridad:** Cifrado criptográfico SHA-256 (`System.Security.Cryptography`)

---

## Módulos Implementados

* **Inicio de Sesión (Login):** Validación de credenciales contra la tabla `Empleados` comparando el hash SHA-256 de la contraseña.
* **Persistencia con RepoDb:** Implementación de interfaz e implementación genérica (`IRepository<T>` y `GenericRepository<T>`) para operaciones CRUD.
* **Formulario Principal y Modal:** Panel lateral de navegación en `FrmPrincipal` y formulario hijo modal (`FrmProductoModal`) invocado con `ShowDialog()` para el registro y edición de productos.
* **Módulo de Reportes:** Visualización de historial de productos vendidos (cantidades y montos acumulados) y consulta de stock individual o general.
* **Semáforo de Stock Crítico (Bonus):** Resaltado visual automático en el `DataGridView` (amarillo para stock menor o igual a 5 unidades, rojo para productos agotados con stock en 0).

---

## Estructura del Proyecto

```text
Laboratorio3/
├── Data/
│   └── Repositorios.cs        # Cadena de conexión, IRepository<T>, GenericRepository<T> y consultas
├── Forms/
│   ├── FrmLogin.cs            # Autenticación SHA-256
│   ├── FrmPrincipal.cs        # Menú principal y catálogo con semáforo de stock
│   ├── FrmProductoModal.cs    # Formulario modal CRUD de productos (ShowDialog)
│   └── FrmReportes.cs         # Reportes de ventas acumuladas y filtro de stock
├── Models/
│   └── Modelos.cs             # Entidades mapeadas con atributos de RepoDb
├── Utils/
│   └── SecurityHelper.cs      # Generador de hash SHA-256
├── Database/
│   └── script.sql             # Script de creación y datos de prueba
└── Program.cs                 # Inicialización de RepoDb y punto de entrada
```

---

## Configuración y Ejecución

1. Ejecutar el script SQL de la carpeta `Database/` en SQL Server Management Studio (SSMS) para crear la base de datos `GestionVentas`.
2. Verificar la cadena de conexión en `Data/Repositorios.cs` (configurada por defecto para `MSI\SQLEXPRESS`).
3. Restaurar los paquetes NuGet (`RepoDb.SqlServer`) y ejecutar el proyecto desde Visual Studio.

### Credenciales de Prueba

| Campo | Valor |
| :--- | :--- |
| **Usuario** | `admin` |
| **Contraseña** | `admin123` |
