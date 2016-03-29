using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDocumentacao.API.Models
{
    public class Despesa
    {
        public string Descricao { get; set; }
        public List<DespesaItem> DespesaItens { get; set; } = new List<DespesaItem>();
        public decimal SubTotal => DespesaItens.Sum(x => x.Valor);
    }
}
