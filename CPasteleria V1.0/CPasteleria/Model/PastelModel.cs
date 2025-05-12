// CPasteleria/Model/PastelModel.cs
using System;

namespace CPasteleria.Model
{
    public class PastelModel
    {
        public int IDPastel { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; } // Usar decimal para precios
        public int Existencias { get; set; }
    }
}