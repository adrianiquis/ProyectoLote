// CPasteleria/Repositories/CarritoRepository.cs
using CPasteleria.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq; // Necesario para Sum()

namespace CPasteleria.Repositories
{
    public class CarritoRepository : RepositoryBase, ICarritoRepository
    {
        public void Add(CarritoModel carritoModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Calcula el subtotal antes de insertar
                carritoModel.Subtotal = carritoModel.Precio * carritoModel.Cantidad;

                command.CommandText = "INSERT INTO Carrito (Nombre, Precio, Cantidad, Subtotal) VALUES(@nombre, @precio, @cantidad, @subtotal)";
                command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = carritoModel.Nombre;
                command.Parameters.Add("@precio", SqlDbType.Decimal).Value = carritoModel.Precio;
                command.Parameters.Add("@cantidad", SqlDbType.Int).Value = carritoModel.Cantidad;
                command.Parameters.Add("@subtotal", SqlDbType.Decimal).Value = carritoModel.Subtotal;
                command.ExecuteNonQuery();
            }
        }

        public void EditQuantity(string nombrePastel, int nuevaCantidad)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Primero, obten el precio para recalcular el subtotal
                decimal precio = 0;
                var cmdGetPrice = new SqlCommand("SELECT Precio FROM Carrito WHERE Nombre = @nombre", connection);
                cmdGetPrice.Parameters.AddWithValue("@nombre", nombrePastel);
                var result = cmdGetPrice.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    precio = (decimal)result;
                }

                // Ahora actualiza la cantidad y el subtotal
                decimal nuevoSubtotal = precio * nuevaCantidad;
                command.Connection = connection;
                command.CommandText = @"UPDATE Carrito
                                        SET Cantidad=@cantidad, Subtotal=@subtotal
                                        WHERE Nombre=@nombre";
                command.Parameters.Add("@cantidad", SqlDbType.Int).Value = nuevaCantidad;
                command.Parameters.Add("@subtotal", SqlDbType.Decimal).Value = nuevoSubtotal;
                command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombrePastel;

                command.ExecuteNonQuery();
            }
        }

        public void Clear()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM Carrito"; // Elimina todas las filas
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<CarritoModel> GetAll()
        {
            var carritoList = new List<CarritoModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT Nombre, Precio, Cantidad, Subtotal FROM Carrito";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var carritoItem = new CarritoModel()
                        {
                            Nombre = reader["Nombre"].ToString(),
                            Precio = (decimal)reader["Precio"],
                            Cantidad = (int)reader["Cantidad"],
                            Subtotal = (decimal)reader["Subtotal"]
                        };
                        carritoList.Add(carritoItem);
                    }
                }
            }
            return carritoList;
        }

        public decimal GetTotal()
        {
            decimal total = 0;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT SUM(Subtotal) FROM Carrito";
                var result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    total = (decimal)result;
                }
            }
            return total;
        }


        public void Remove(string nombrePastel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM Carrito WHERE Nombre=@nombre";
                command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombrePastel;
                command.ExecuteNonQuery();
            }
        }
    }
}