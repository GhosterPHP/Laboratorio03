using Laboratorio3.Models;
using Microsoft.Data.SqlClient;
using RepoDb;

namespace Laboratorio3.Data
{
    public static class DbConfig
    {
        public const string ConnectionString =
            @"Server=MSI\SQLEXPRESS;Database=GestionVentas;Trusted_Connection=True;TrustServerCertificate=True;";
    }

    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        object Insert(T entity);
        int Update(T entity);
        int Delete(int id);
    }

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll()
        {
            using var conn = new SqlConnection(DbConfig.ConnectionString);
            return conn.QueryAll<T>();
        }

        public T? GetById(int id)
        {
            using var conn = new SqlConnection(DbConfig.ConnectionString);
            return conn.Query<T>(id).FirstOrDefault();
        }

        public object Insert(T entity)
        {
            using var conn = new SqlConnection(DbConfig.ConnectionString);
            return conn.Insert(entity);
        }

        public int Update(T entity)
        {
            using var conn = new SqlConnection(DbConfig.ConnectionString);
            return conn.Update(entity);
        }

        public int Delete(int id)
        {
            using var conn = new SqlConnection(DbConfig.ConnectionString);
            return conn.Delete<T>(id);
        }
    }

    public class ConsultasRepository
    {
        public Empleado? ValidarLogin(string usuario, string passwordHash)
        {
            using var conn = new SqlConnection(DbConfig.ConnectionString);
            return conn.Query<Empleado>(e => e.Usuario == usuario && e.PasswordHash == passwordHash)
                       .FirstOrDefault();
        }

        public IEnumerable<ReporteVentaDto> ObtenerProductosVendidos()
        {
            const string sql = @"
                SELECT 
                    p.id_producto AS IdProducto,
                    p.nombre AS Producto,
                    SUM(dv.cantidad) AS TotalVendido,
                    SUM(dv.cantidad * dv.precio_unitario) AS MontoAcumulado
                FROM dbo.DetalleVenta dv
                INNER JOIN dbo.Productos p ON dv.id_producto = p.id_producto
                GROUP BY p.id_producto, p.nombre
                ORDER BY MontoAcumulado DESC;";

            using var conn = new SqlConnection(DbConfig.ConnectionString);
            return conn.ExecuteQuery<ReporteVentaDto>(sql);
        }
    }
}