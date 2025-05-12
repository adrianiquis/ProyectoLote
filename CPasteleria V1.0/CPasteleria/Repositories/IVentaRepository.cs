// CPasteleria/Repositories/IVentaRepository.cs
using CPasteleria.Model;
using System.Collections.Generic;

namespace CPasteleria.Repositories
{
    public interface IVentaRepository
    {
        int Add(VentaModel ventaModel); // Devuelve el ID de la venta insertada
        VentaModel GetById(int idVenta);
        IEnumerable<VentaModel> GetAll();
        IEnumerable<int> GetAllIds(); // Útil para ComboBoxes
        // No se suelen editar o eliminar ventas completas, pero se podrían añadir si es necesario
    }
}