using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoLote.Model;

namespace ProyectoLote.Repositories
{
    public interface ICompraRepository
    {
        int Add(CompraModel compra);
        IEnumerable<CompraModel> GetAll();
        CompraModel GetById(int folio);
        IEnumerable<int> GetAllFolios();
    }
}


