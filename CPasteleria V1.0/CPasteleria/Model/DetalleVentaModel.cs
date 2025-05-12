// CPasteleria/Model/DetalleVentaModel.cs
using System;

namespace CPasteleria.Model
{
    public class DetalleVentaModel
    {
        public int IDDetalle { get; set; }
        public int IDVenta { get; set; } // Referencia a Venta(IDVenta)
        public string Nombre { get; set; } // Nombre del pastel
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}