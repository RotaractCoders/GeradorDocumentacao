using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDocumentacao.API.Word
{
    public class Celula
    {
        public int Width { get; set; }
        public BorderValues BorderValues { get; set; }
        public string Fonte { get; set; }
        public string Texto { get; set; }
        public int TamanhoFonte { get; set; }
        public bool Sublinhado { get; set; }
        public bool Negrito { get; set; }
    }
}
