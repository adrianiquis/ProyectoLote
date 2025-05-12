// CPasteleria/Repositories/DetalleVentaRepository.cs
using CPasteleria.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CPasteleria.Repositories
{
    public class DetalleVentaRepository : RepositoryBase, IDetalleVentaRepository
    {
        public void Add(DetalleVentaModel detalleVentaModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Asumiendo que IDDetalle es autoincremental o se genera de otra forma.
                // Si NO es autoincremental, necesitas añadirlo aquí y en los parámetros.
                command.CommandText = "INSERT INTO DetalleVenta (IDVenta, Nombre, PrecioUnitario, Cantidad, Subtotal) VALUES(@idVenta, @nombre, @precioUnitario, @cantidad, @subtotal)";
                command.Parameters.Add("@idVenta", SqlDbType.Int).Value = detalleVentaModel.IDVenta;
                command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = detalleVentaModel.Nombre;
                command.Parameters.Add("@precioUnitario", SqlDbType.Decimal).Value = detalleVentaModel.PrecioUnitario;
                command.Parameters.Add("@cantidad", SqlDbType.Int).Value = detalleVentaModel.Cantidad;
                command.Parameters.Add("@subtotal", SqlDbType.Decimal).Value = detalleVentaModel.Subtotal;
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<DetalleVentaModel> GetByVentaId(int idVenta)
        {
            var detalleList = new List<DetalleVentaModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM DetalleVenta WHERE IDVenta=@idVenta";
                command.Parameters.Add("@idVenta", SqlDbType.Int).Value = idVenta;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var detalle = new DetalleVentaModel()
                        {
                            IDDetalle = (int)reader["IDDetalle"],
                            IDVenta = (int)reader["IDVenta"],
                            Nombre = reader["Nombre"].ToString(),
                            PrecioUnitario = (decimal)reader["PrecioUnitario"],
                            Cantidad = (int)reader["Cantidad"],
                            Subtotal = (decimal)reader["Subtotal"]
                        };
                        detalleList.Add(detalle);
                    }
                }
            }
            return detalleList;
        }
    }
}