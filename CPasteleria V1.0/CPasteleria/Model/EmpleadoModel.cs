using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPasteleria.Model 
{
    public class EmpleadoModel
    {
        public int ID_Empleado { get; set; } 
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public string RFC { get; set; }
        public string Nombre { get; set; }
    }
}