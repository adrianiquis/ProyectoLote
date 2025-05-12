// CPasteleria/Repositories/IDetalleVentaRepository.cs
using CPasteleria.Model;
using System.Collections.Generic;

namespace CPasteleria.Repositories
{
    public interface IDetalleVentaRepository
    {
        void Add(DetalleVentaModel detalleVentaModel);
        IEnumerable<DetalleVentaModel> GetByVentaId(int idVenta);
        // No se suelen editar o eliminar detalles individuales, se maneja a nivel de Venta.
    }
}