using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLote.Model
{
    public class CompraModel
    {
        public int folio { get; set; }
        public string usuario { get; set; }
        public string vin { get; set; }
        public DateTime fecha { get; set; }
        public string metodoPago { get; set; }
        public string detalles { get; set; }

    }
}
