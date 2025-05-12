using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ProyectoLote.Model;

namespace ProyectoLote.Repositories
{
    public class CarroRepository : RepositoryBase, ICarroRepository
    {
        public void Add(CarroModel carroModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Carro (vin, marca, modelo, color, existencia, clave_vendedor)
                                        VALUES (@vin, @marca, @modelo, @color, @existencia, @clave_vendedor, @precio)";
                command.Parameters.Add("@vin", SqlDbType.VarChar, 17).Value = carroModel.Vin;
                command.Parameters.Add("@marca", SqlDbType.VarChar, 50).Value = carroModel.Marca;
                command.Parameters.Add("@modelo", SqlDbType.VarChar, 50).Value = carroModel.Modelo;
                command.Parameters.Add("@color", SqlDbType.VarChar, 30).Value = carroModel.Color ?? (object)DBNull.Value;
                command.Parameters.Add("@existencia", SqlDbType.Bit).Value = carroModel.Existencia;
                command.Parameters.Add("@clave_vendedor", SqlDbType.Int).Value = carroModel.ClaveVendedor;
                command.Parameters.Add("@precio", SqlDbType.Decimal).Value = carroModel.Precio;
                command.ExecuteNonQuery();
            }
        }

        public void Edit(CarroModel carroModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Carro SET marca=@marca, modelo=@modelo, color=@color, existencia=@existencia, clave_vendedor=@clave_vendedor,
                                        precio = @precio WHERE vin=@vin";
                command.Parameters.Add("@vin", SqlDbType.VarChar, 17).Value = carroModel.Vin;
                command.Parameters.Add("@marca", SqlDbType.VarChar, 50).Value = carroModel.Marca;
                command.Parameters.Add("@modelo", SqlDbType.VarChar, 50).Value = carroModel.Modelo;
                command.Parameters.Add("@color", SqlDbType.VarChar, 30).Value = carroModel.Color ?? (object)DBNull.Value;
                command.Parameters.Add("@existencia", SqlDbType.Bit).Value = carroModel.Existencia;
                command.Parameters.Add("@clave_vendedor", SqlDbType.Int).Value = carroModel.ClaveVendedor;
                command.Parameters.Add("@precio", SqlDbType.Decimal).Value = carroModel.Precio;
                command.ExecuteNonQuery();
            }
        }

        public void Remove(string vin)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM Carro WHERE vin=@vin";
                command.Parameters.Add("@vin", SqlDbType.VarChar, 17).Value = vin;
                command.ExecuteNonQuery();
            }
        }

        public CarroModel GetByVin(string vin)
        {
            CarroModel carro = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Carro WHERE vin=@vin";
                command.Parameters.Add("@vin", SqlDbType.VarChar, 17).Value = vin;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        carro = MapCarro(reader);
                    }
                }
            }
            return carro;
        }

        public IEnumerable<CarroModel> GetAll()
        {
            var lista = new List<CarroModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Carro", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MapCarro(reader));
                    }
                }
            }
            return lista;
        }

        public IEnumerable<CarroModel> GetByMarca(string marca)
        {
            var lista = new List<CarroModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Carro WHERE marca = @marca", connection))
            {
                connection.Open();
                command.Parameters.Add("@marca", SqlDbType.VarChar, 50).Value = marca;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(MapCarro(reader));
                    }
                }
            }
            return lista;
        }

        private CarroModel MapCarro(SqlDataReader reader)
        {
            return new CarroModel
            {
                Vin = reader["vin"].ToString(),
                Marca = reader["marca"].ToString(),
                Modelo = reader["modelo"].ToString(),
                Color = reader["color"]?.ToString(),
                Existencia = reader["existencia"] != DBNull.Value && (bool)reader["existencia"],
                ClaveVendedor = (int)reader["clave_vendedor"],
                Precio = reader["precio"] != DBNull.Value ? Convert.ToDouble(reader["precio"]) : 0
            };
        }
    }
}