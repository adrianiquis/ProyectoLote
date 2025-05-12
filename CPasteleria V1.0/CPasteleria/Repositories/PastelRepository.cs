// CPasteleria/Repositories/PastelRepository.cs
using CPasteleria.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq; // Añadido para .Any() y .Max() si lo usaras dentro del repositorio

namespace CPasteleria.Repositories
{
    public class PastelRepository : RepositoryBase, IPastelRepository
    {
        public void Add(PastelModel pastelModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Comando para cuando IDPastel NO es IDENTITY y se provee desde el modelo
                command.CommandText = "INSERT INTO Pastel (IDPastel, Nombre, Precio, Existencias) VALUES(@id, @nombre, @precio, @existencias)";
                command.Parameters.Add("@id", SqlDbType.Int).Value = pastelModel.IDPastel; // <--- AÑADIR ESTE PARÁMETRO
                command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = pastelModel.Nombre;
                command.Parameters.Add("@precio", SqlDbType.Decimal).Value = pastelModel.Precio; // Tu BD usa NUMERIC(4,0)
                command.Parameters.Add("@existencias", SqlDbType.Int).Value = pastelModel.Existencias;
                command.ExecuteNonQuery();
            }
        }

        // ... (El resto de los métodos Edit, UpdateStock, GetAll, GetAllNames, GetById, GetByName, Remove pueden permanecer igual)
        // ... (Asegúrate que los tipos de datos y longitudes coincidan con tu BD)
        public void Edit(PastelModel pastelModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Pastel
                                        SET Nombre=@nombre, Precio=@precio, Existencias=@existencias
                                        WHERE IDPastel=@id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = pastelModel.IDPastel;
                command.Parameters.Add("@nombre", SqlDbType.VarChar, 25).Value = pastelModel.Nombre; // Especificar longitud
                command.Parameters.Add("@precio", SqlDbType.Decimal).Value = pastelModel.Precio;
                // Para NUMERIC(4,0) en BD, asegúrate que el decimal no tenga parte fraccional o esté dentro del rango
                // Si el precio es, por ejemplo, 170.50, esto podría dar error si la BD es NUMERIC(4,0)
                // Deberías convertirlo a entero o asegurar que sea entero antes de pasarlo si la BD es NUMERIC(X,0)
                // command.Parameters.Add("@precio", SqlDbType.Decimal){Precision = 4, Scale = 0}.Value = Math.Truncate(pastelModel.Precio);

                command.Parameters.Add("@existencias", SqlDbType.Int).Value = pastelModel.Existencias;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateStock(int idPastel, int quantityChange)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Pastel SET Existencias = Existencias - @quantity WHERE IDPastel=@id";
                command.Parameters.Add("@quantity", SqlDbType.Int).Value = quantityChange;
                command.Parameters.Add("@id", SqlDbType.Int).Value = idPastel;
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<PastelModel> GetAll()
        {
            var pastelList = new List<PastelModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT IDPastel, Nombre, Precio, Existencias FROM Pastel ORDER BY Nombre ASC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pastelModel = new PastelModel()
                        {
                            IDPastel = (int)reader["IDPastel"],
                            Nombre = reader["Nombre"].ToString(),
                            Precio = (decimal)reader["Precio"], // SQL NUMERIC se mapea bien a C# decimal
                            Existencias = reader["Existencias"] != DBNull.Value ? (int)reader["Existencias"] : 0
                        };
                        pastelList.Add(pastelModel);
                    }
                }
            }
            return pastelList;
        }

        public IEnumerable<string> GetAllNames()
        {
            var nameList = new List<string>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT Nombre FROM Pastel ORDER BY Nombre ASC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        nameList.Add(reader["Nombre"].ToString());
                    }
                }
            }
            return nameList;
        }


        public PastelModel GetById(int idPastel)
        {
            PastelModel pastel = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT IDPastel, Nombre, Precio, Existencias FROM Pastel WHERE IDPastel=@id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = idPastel;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pastel = new PastelModel()
                        {
                            IDPastel = (int)reader["IDPastel"],
                            Nombre = reader["Nombre"].ToString(),
                            Precio = (decimal)reader["Precio"],
                            Existencias = reader["Existencias"] != DBNull.Value ? (int)reader["Existencias"] : 0
                        };
                    }
                }
            }
            return pastel;
        }

        public PastelModel GetByName(string name)
        {
            PastelModel pastel = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT IDPastel, Nombre, Precio, Existencias FROM Pastel WHERE Nombre=@name";
                command.Parameters.Add("@name", SqlDbType.VarChar, 25).Value = name; // Especificar longitud
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pastel = new PastelModel()
                        {
                            IDPastel = (int)reader["IDPastel"],
                            Nombre = reader["Nombre"].ToString(),
                            Precio = (decimal)reader["Precio"],
                            Existencias = reader["Existencias"] != DBNull.Value ? (int)reader["Existencias"] : 0
                        };
                    }
                }
            }
            return pastel;
        }

        public void Remove(int idPastel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM Pastel WHERE IDPastel=@id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = idPastel;
                command.ExecuteNonQuery();
            }
        }
    }
}