using ProyectoLote.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoLote.Repositories
{
    public class VendedorRepository : RepositoryBase
    {
        public int Add(VendedorModel vendedor)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Vendedor (nombre, correo) 
                                        OUTPUT INSERTED.clave_vendedor 
                                        VALUES (@nombre, @correo)";
                command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = vendedor.nombre;
                command.Parameters.Add("@correo", SqlDbType.VarChar).Value = vendedor.correo;

                int insertedId = (int)command.ExecuteScalar();
                return insertedId;
            }
        }

        public IEnumerable<VendedorModel> GetAll()
        {
            var vendedores = new List<VendedorModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT clave_vendedor, nombre, correo FROM Vendedor ORDER BY clave_vendedor DESC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var vendedor = new VendedorModel
                        {
                            clave_vendedor = (int)reader["clave_vendedor"],
                            nombre = reader["nombre"].ToString(),
                            correo = reader["correo"].ToString()
                        };
                        vendedores.Add(vendedor);
                    }
                }
            }
            return vendedores;
        }

        public VendedorModel GetById(int id)
        {
            VendedorModel vendedor = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT clave_vendedor, nombre, correo 
                                        FROM Vendedor 
                                        WHERE clave_vendedor = @id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        vendedor = new VendedorModel
                        {
                            clave_vendedor = (int)reader["clave_vendedor"],
                            nombre = reader["nombre"].ToString(),
                            correo = reader["correo"].ToString()
                        };
                    }
                }
            }
            return vendedor;
        }

        public IEnumerable<int> GetAllIds()
        {
            var ids = new List<int>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT clave_vendedor FROM Vendedor ORDER BY clave_vendedor DESC";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add((int)reader["clave_vendedor"]);
                    }
                }
            }
            return ids;
        }
    }
}
