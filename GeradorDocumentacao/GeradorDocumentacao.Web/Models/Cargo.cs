using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDocumentacao.Web.Models
{
    public class Cargo
    {
        public long CodCargo { get; set; }
        public string Descricao { get; set; }

        public List<Socio> Socios { get; set; }
    }
}