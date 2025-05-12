// CPasteleria/Model/VentaModel.cs
using System;

namespace CPasteleria.Model
{
    public class VentaModel
    {
        public int IDVenta { get; set; }
        public string Usuario { get; set; } // Referencia a Empleado(Usuario)
        public DateTime Fecha { get; set; } // DateTime para DATE de SQL
        public decimal Total { get; set; }

        // Propiedad opcional para mostrar nombre del empleado (si se hace JOIN)
        public string NombreEmpleado { get; set; }
        // Propiedad opcional para la lista de detalles (si se cargan juntos)
        public System.Collections.Generic.List<DetalleVentaModel> Detalles { get; set; }
    }
}