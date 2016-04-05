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
        public int Dia => Data.Day;
        public MesesComPosisaoNasTabelas Mes => FormatarMes(Data.Month);
        public int Ano => Data.Year;

        public DateTime Data { get; set; }
        public string Quem { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }

        private MesesComPosisaoNasTabelas FormatarMes(int mes)
        {
            if (mes >= 7)
            {
                return (MesesComPosisaoNasTabelas)mes + 1;
            }
            else
            {
                return (MesesComPosisaoNasTabelas)mes + 13;
            }
        }
    }
}
