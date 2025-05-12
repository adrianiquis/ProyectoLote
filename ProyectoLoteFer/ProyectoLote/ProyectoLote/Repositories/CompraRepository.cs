using ProyectoLote.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoLote.Repositories
{
    public class CompraRepository : RepositoryBase, ICompraRepository
    {
        public int Add(CompraModel compra)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = @"INSERT INTO Compra (usuario, vin, fecha, metodo_pago, detalles) 
                                        OUTPUT INSERTED.folio 
                                        VALUES (@usuario, @vin, @fecha, @metodo_pago, @detalles)";
                command.Parameters.Add("@usuario", SqlDbType.VarChar).Value = compra.usuario;
                command.Parameters.Add("@vin", SqlDbType.VarChar).Value = compra.vin;
                command.Parameters.Add("@fecha", SqlDbType.Date).Value = compra.fecha.Date;
                command.Parameters.Add("@metodo_pago", SqlDbType.VarChar).Value = compra.metodoPago;
                command.Parameters.Add("@detalles", SqlDbType.VarChar).Value = compra.detalles ?? string.Empty;

                return (int)command.ExecuteScalar();
            }
        }

        public IEnumerable<CompraModel> GetAll()
        {
            var compras = new List<CompraModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = @"
                    SELECT c.folio, c.usuario, c.vin, c.fecha, c.metodo_pago, c.detalles,
                           u.nombre AS NombreUsuario, car.descripcion AS DescripcionCarro
                    FROM Compra c
                    LEFT JOIN Usuario u ON c.usuario = u.usuario
                    LEFT JOIN Carro car ON c.vin = car.vin
                    ORDER BY c.fecha DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        compras.Add(new CompraModel
                        {
                            folio = (int)reader["folio"],
                            usuario = reader["usuario"].ToString(),
                            vin = reader["vin"].ToString(),
                            fecha = (DateTime)reader["fecha"],
                            metodoPago = reader["metodo_pago"].ToString(),
                            detalles = reader["detalles"].ToString(),
                            
                        });
                    }
                }
            }
            return compras;
        }

        public CompraModel GetById(int folio)
        {
            CompraModel compra = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                    SELECT c.folio, c.usuario, c.vin, c.fecha, c.metodo_pago, c.detalles,
                           u.nombre AS NombreUsuario, car.descripcion AS DescripcionCarro
                    FROM Compra c
                    LEFT JOIN Usuario u ON c.usuario = u.usuario
                    LEFT JOIN Carro car ON c.vin = car.vin
                    WHERE c.folio = @folio";

                command.Parameters.Add("@folio", SqlDbType.Int).Value = folio;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        compra = new CompraModel
                        {
                            folio = (int)reader["folio"],
                            usuario = reader["usuario"].ToString(),
                            vin = reader["vin"].ToString(),
                            fecha = (DateTime)reader["fecha"],
                            metodoPago = reader["metodo_pago"].ToString(),
                            detalles = reader["detalles"].ToString(),
                         
                        };
                    }
                }
            }
            return compra;
        }

        public IEnumerable<int> GetAllFolios()
        {
            var folios = new List<int>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT folio FROM Compra ORDER BY folio DESC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        folios.Add((int)reader["folio"]);
                    }
                }
            }
            return folios;
        }
    }
}
