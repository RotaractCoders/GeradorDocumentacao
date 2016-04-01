using GeradorDocumentacao.API.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDocumentacao.API.Models
{
    public class Evento
    {
        public int Dia { get; set; }
        public MesesComPosisaoNasTabelas Mes { get; set; }
        public int Ano { get; set; }
        public string Quem { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
    }
}
