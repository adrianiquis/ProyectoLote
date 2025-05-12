// CPasteleria/Repositories/ICarritoRepository.cs
using CPasteleria.Model;
using System.Collections.Generic;

namespace CPasteleria.Repositories
{
    public interface ICarritoRepository
    {
        void Add(CarritoModel carritoModel);
        void EditQuantity(string nombrePastel, int nuevaCantidad); // Editar basado en nombre
        void Remove(string nombrePastel); // Remover basado en nombre
        void Clear(); // Limpiar todo el carrito
        IEnumerable<CarritoModel> GetAll();
        decimal GetTotal(); // Calcular total del carrito
    }
}