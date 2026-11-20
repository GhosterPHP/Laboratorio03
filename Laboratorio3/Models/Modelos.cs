using RepoDb.Attributes;

namespace Laboratorio3.Models
{
    [Map("Clientes")]
    public class Cliente
    {
        [Primary, Identity, Map("id_cliente")]
        public int IdCliente { get; set; }
        [Map("nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Map("apellido")]
        public string Apellido { get; set; } = string.Empty;
        [Map("telefono")]
        public string? Telefono { get; set; }
    }

    [Map("Empleados")]
    public class Empleado
    {
        [Primary, Identity, Map("id_empleado")]
        public int IdEmpleado { get; set; }
        [Map("nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Map("apellido")]
        public string Apellido { get; set; } = string.Empty;
        [Map("cargo")]
        public string? Cargo { get; set; }
        [Map("usuario")]
        public string? Usuario { get; set; }
        [Map("password_hash")]
        public string? PasswordHash { get; set; }
    }

    [Map("Productos")]
    public class Producto
    {
        [Primary, Identity, Map("id_producto")]
        public int IdProducto { get; set; }
        [Map("nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Map("descripcion")]
        public string? Descripcion { get; set; }
        [Map("precio_base")]
        public decimal PrecioBase { get; set; }
        [Map("stock")]
        public int Stock { get; set; }
    }

    public class ReporteVentaDto
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public int TotalVendido { get; set; }
        public decimal MontoAcumulado { get; set; }
    }
}