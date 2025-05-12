using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLote.Model
{
    public interface ICarroRepository
    {
        void Add(CarroModel carroModel);
        void Edit(CarroModel carroModel);
        void Remove(string vin);
        CarroModel GetByVin(string vin);
        IEnumerable<CarroModel> GetAll();
        IEnumerable<CarroModel> GetByMarca(string marca);

    }
}
