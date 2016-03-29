using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDocumentacao.Web.Word
{
    public class Utils
    {
        public static TableRow TabelaLinha(int heigth, List<Celula> celulas)
        {
            var tabelaLinha = new TableRow();
            tabelaLinha.Append(TabelaLinhaPropriedades(heigth));

            foreach (var celula in celulas)
            {
                tabelaLinha.Append(TabelaCelula(celula.Width, celula.BorderValues, celula.Fonte, celula.TamanhoFonte, celula.Sublinhado, celula.Negrito, celula.Texto));
            }

            return tabelaLinha;
        }

        private static TableCell TabelaCelula(int width, BorderValues borderValues, string fonte, int tamanhoFonte, bool sublinhado, bool negrito, string texto)
        {
            var tabelaCelula = new TableCell();
            tabelaCelula.Append(TabelaCelulaPropriedades(width, borderValues));
            tabelaCelula.Append(Paragrafo(fonte, tamanhoFonte, sublinhado, negrito, texto));

            return tabelaCelula;
        }

        private static Paragraph Paragrafo(string fonte, int tamanhoFonte, bool sublinhado, bool negrito, string texto)
        {
            var paragrafo = new Paragraph();

            paragrafo.Append(Texto(fonte, tamanhoFonte, sublinhado, negrito, texto));

            return paragrafo;
        }

        private static Run Texto(string fonte, int tamanhoFonte, bool sublinhado, bool negrito, string texto)
        {
            var run = new Run();
            run.Append(TextoPropriedades(fonte, tamanhoFonte, sublinhado, negrito));
            run.Append(new Text(texto));

            return run;
        }

        private static TableRowProperties TabelaLinhaPropriedades(int heigth)
        {
            return new TableRowProperties(new TableRowHeight { Val = (UInt32)heigth });
        }

        private static TableCellProperties TabelaCelulaPropriedades(int width, BorderValues borderValues)
        {
            return new TableCellProperties
            {
                TableCellWidth = new TableCellWidth { Width = new StringValue(width.ToString()) },
                TableCellBorders = new TableCellBorders
                {
                    TopBorder = new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(borderValues)
                    },
                    LeftBorder = new LeftBorder
                    {
                        Val = new EnumValue<BorderValues>(borderValues)
                    },
                    RightBorder = new RightBorder
                    {
                        Val = new EnumValue<BorderValues>(borderValues)
                    },
                    BottomBorder = new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(borderValues)
                    }
                }
            };
        }

        private static RunProperties TextoPropriedades(string fonte, int tamanhoFonte, bool sublinhado, bool negrito)
        {
            return new RunProperties
            {
                RunFonts = new RunFonts { Ascii = fonte },
                FontSize = new FontSize
                {
                    Val = new StringValue(tamanhoFonte.ToString())
                },
                
                Underline = sublinhado ? new Underline() : null,
                Bold = negrito ? new Bold() : null
            };
        }
    }
}
