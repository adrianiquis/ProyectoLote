using ProyectoLote.Model;
using System.Collections.Generic;

namespace ProyectoLote.Repositories
{
    public interface IVendedorRepository
    {
        int Add(VendedorModel vendedor);
        IEnumerable<VendedorModel> GetAll();
        VendedorModel GetById(int id);
        IEnumerable<int> GetAllIds();
    }
}
