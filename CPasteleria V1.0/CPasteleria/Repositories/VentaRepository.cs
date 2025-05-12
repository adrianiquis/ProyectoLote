// CPasteleria/Repositories/VentaRepository.cs
using CPasteleria.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CPasteleria.Repositories
{
    public class VentaRepository : RepositoryBase, IVentaRepository
    {
        // Devuelve el ID de la venta recién creada
        public int Add(VentaModel ventaModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Asegúrate de que la fecha se envía correctamente
                command.CommandText = "INSERT INTO Venta (Usuario, Fecha, Total) OUTPUT INSERTED.IDVenta VALUES(@usuario, @fecha, @total)";
                command.Parameters.Add("@usuario", SqlDbType.VarChar).Value = ventaModel.Usuario;
                command.Parameters.Add("@fecha", SqlDbType.Date).Value = ventaModel.Fecha.Date; // Enviar solo la fecha
                command.Parameters.Add("@total", SqlDbType.Decimal).Value = ventaModel.Total;

                // Ejecuta y obtén el ID devuelto
                int insertedId = (int)command.ExecuteScalar();
                return insertedId;
            }
        }

        public IEnumerable<VentaModel> GetAll()
        {
            var ventaList = new List<VentaModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Opcional: Hacer JOIN con Empleado para obtener el nombre
                command.CommandText = @"SELECT v.IDVenta, v.Usuario, v.Fecha, v.Total, e.Nombre as NombreEmpleado
                                        FROM Venta v
                                        LEFT JOIN Empleado e ON v.Usuario = e.Usuario
                                        ORDER BY v.Fecha DESC";
                // command.CommandText = "SELECT IDVenta, Usuario, Fecha, Total FROM Venta ORDER BY Fecha DESC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ventaModel = new VentaModel()
                        {
                            IDVenta = (int)reader["IDVenta"],
                            Usuario = reader["Usuario"].ToString(),
                            Fecha = (DateTime)reader["Fecha"],
                            Total = (decimal)reader["Total"],
                            NombreEmpleado = reader["NombreEmpleado"] != DBNull.Value ? reader["NombreEmpleado"].ToString() : reader["Usuario"].ToString() // Muestra usuario si no hay nombre
                        };
                        ventaList.Add(ventaModel);
                    }
                }
            }
            return ventaList;
        }

        public IEnumerable<int> GetAllIds()
        {
            var idList = new List<int>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT IDVenta FROM Venta ORDER BY IDVenta DESC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idList.Add((int)reader["IDVenta"]);
                    }
                }
            }
            return idList;
        }

        public VentaModel GetById(int idVenta)
        {
            VentaModel venta = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT v.IDVenta, v.Usuario, v.Fecha, v.Total, e.Nombre as NombreEmpleado
                                        FROM Venta v
                                        LEFT JOIN Empleado e ON v.Usuario = e.Usuario
                                        WHERE v.IDVenta=@id";
                // command.CommandText = "SELECT * FROM Venta WHERE IDVenta=@id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = idVenta;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        venta = new VentaModel()
                        {
                            IDVenta = (int)reader["IDVenta"],
                            Usuario = reader["Usuario"].ToString(),
                            Fecha = (DateTime)reader["Fecha"],
                            Total = (decimal)reader["Total"],
                            NombreEmpleado = reader["NombreEmpleado"] != DBNull.Value ? reader["NombreEmpleado"].ToString() : reader["Usuario"].ToString()
                        };
                    }
                }
                // Opcional: Cargar detalles de la venta aquí si se necesita
                if (venta != null)
                {
                    // Necesitas una instancia de DetalleVentaRepository
                    IDetalleVentaRepository detalleRepo = new DetalleVentaRepository();
                    venta.Detalles = new List<DetalleVentaModel>(detalleRepo.GetByVentaId(idVenta));
                }
            }
            return venta;
        }
    }
}