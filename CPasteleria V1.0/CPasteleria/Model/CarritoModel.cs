// CPasteleria/Model/CarritoModel.cs
using System;

namespace CPasteleria.Model
{
    public class CarritoModel
    {
        // Nota: Esta tabla no tiene PK en tu script SQL.
        // Considera añadir un ID si necesitas identificar filas únicas fácilmente.
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}