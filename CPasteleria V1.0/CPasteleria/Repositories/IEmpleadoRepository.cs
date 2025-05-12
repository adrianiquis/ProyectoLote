using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net; 
using CPasteleria.Model; 

namespace CPasteleria.Repositories 
{
    public interface IEmpleadoRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(EmpleadoModel empleadoModel);
        void Edit(EmpleadoModel empleadoModel);
        void Remove(int idEmpleado); 
        EmpleadoModel GetById(int idEmpleado);
        EmpleadoModel GetByUsername(string username);
        IEnumerable<EmpleadoModel> GetAll(); 
    }
}