// CPasteleria/Repositories/IPastelRepository.cs
using CPasteleria.Model;
using System.Collections.Generic;

namespace CPasteleria.Repositories
{
    public interface IPastelRepository
    {
        void Add(PastelModel pastelModel);
        void Edit(PastelModel pastelModel);
        void Remove(int idPastel);
        PastelModel GetById(int idPastel);
        PastelModel GetByName(string name);
        IEnumerable<PastelModel> GetAll();
        IEnumerable<string> GetAllNames(); // Útil para ComboBoxes
        void UpdateStock(int idPastel, int quantityChange); // Para actualizar después de venta
    }
}