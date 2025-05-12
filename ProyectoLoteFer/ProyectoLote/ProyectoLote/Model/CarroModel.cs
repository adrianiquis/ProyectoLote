using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLote.Model
{
    public class CarroModel
    {
        public string Vin { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public bool Existencia { get; set; }
        public int ClaveVendedor { get; set; }
        public double Precio { get; set; }
    }
}