using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ProyectoLote.Model;


namespace ProyectoLote.Repositories
{
    public class UserRepository:RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [Usuarios] where usuario=@usuario and [contrasenia]=@contrasenia";
                command.Parameters.Add("@usuario", System.Data.SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@contrasenia", System.Data.SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
                return validUser;
            }
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int usuario)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string usuario)
        {
            throw new NotImplementedException();
        }

        public void Remove(int usuario)
        {
            throw new NotImplementedException();
        }
    }
    }
