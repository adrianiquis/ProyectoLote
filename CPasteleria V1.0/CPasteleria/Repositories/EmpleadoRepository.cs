using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using CPasteleria.Model;

namespace CPasteleria.Repositories 
{
    public class EmpleadoRepository : RepositoryBase, IEmpleadoRepository
    {
        public void Add(EmpleadoModel empleadoModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Empleado (ID_Empleado, Usuario, Contraseña, RFC, Nombre) VALUES(@id, @usuario, @contraseña, @rfc, @nombre)";
                command.Parameters.Add("@id", SqlDbType.Int).Value = empleadoModel.ID_Empleado;
                command.Parameters.Add("@usuario", SqlDbType.VarChar).Value = empleadoModel.Usuario;
                command.Parameters.Add("@contraseña", SqlDbType.VarChar).Value = empleadoModel.Contraseña;
                command.Parameters.Add("@rfc", SqlDbType.VarChar).Value = empleadoModel.RFC;
                command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = empleadoModel.Nombre;
                command.ExecuteNonQuery();
            }
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Empleado WHERE Usuario=@usuario AND Contraseña=@contraseña";
                command.Parameters.Add("@usuario", SqlDbType.VarChar).Value = credential.UserName;
                command.Parameters.Add("@contraseña", SqlDbType.VarChar).Value = credential.Password; // Compara con la contraseña (hasheada?)
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }

        public void Edit(EmpleadoModel empleadoModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Empleado
                                        SET Usuario=@usuario, Contraseña=@contraseña, RFC=@rfc, Nombre=@nombre
                                        WHERE ID_Empleado=@id";
                command.Parameters.Add("@usuario", SqlDbType.VarChar).Value = empleadoModel.Usuario;
                command.Parameters.Add("@contraseña", SqlDbType.VarChar).Value = empleadoModel.Contraseña; // Considera hashear la contraseña
                command.Parameters.Add("@rfc", SqlDbType.VarChar).Value = empleadoModel.RFC;
                command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = empleadoModel.Nombre;
                command.Parameters.Add("@id", SqlDbType.Int).Value = empleadoModel.ID_Empleado;
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<EmpleadoModel> GetAll()
        {
            var empleadoList = new List<EmpleadoModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Empleado ORDER BY Nombre ASC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var empleadoModel = new EmpleadoModel()
                        {
                            ID_Empleado = (int)reader["ID_Empleado"],
                            Usuario = reader["Usuario"].ToString(),
                            Contraseña = string.Empty, 
                            RFC = reader["RFC"].ToString(),
                            Nombre = reader["Nombre"].ToString()
                        };
                        empleadoList.Add(empleadoModel);
                    }
                }
            }
            return empleadoList;
        }

        public EmpleadoModel GetById(int idEmpleado)
        {
            EmpleadoModel empleado = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Empleado WHERE ID_Empleado=@id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = idEmpleado;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        empleado = new EmpleadoModel()
                        {
                            ID_Empleado = (int)reader["ID_Empleado"],
                            Usuario = reader["Usuario"].ToString(),
                            Contraseña = string.Empty, 
                            RFC = reader["RFC"].ToString(),
                            Nombre = reader["Nombre"].ToString()
                        };
                    }
                }
            }
            return empleado;
        }

        public EmpleadoModel GetByUsername(string username)
        {
            EmpleadoModel empleado = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Empleado WHERE Usuario=@usuario";
                command.Parameters.Add("@usuario", SqlDbType.VarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        empleado = new EmpleadoModel()
                        {
                            ID_Empleado = (int)reader["ID_Empleado"],
                            Usuario = reader["Usuario"].ToString(),
                            Contraseña = string.Empty, 
                            RFC = reader["RFC"].ToString(),
                            Nombre = reader["Nombre"].ToString()
                        };
                    }
                }
            }
            return empleado;
        }

        public void Remove(int idEmpleado)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM Empleado WHERE ID_Empleado=@id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = idEmpleado;
                command.ExecuteNonQuery();
            }
        }
    }
}