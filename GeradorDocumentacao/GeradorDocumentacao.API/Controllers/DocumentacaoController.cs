using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GeradorDocumentacao.API.Models;
using GeradorDocumentacao.API.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace GeradorDocumentacao.API.Controllers
{
    public class DocumentacaoController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Gerar(Clube clube)
        {
            HttpResponseMessage retorno = null;

            try
            {
                var arquivo = PopularWord(clube);

                retorno = MontarResponseMessage(clube, arquivo);
            }
            catch (Exception ex)
            {
                retorno.StatusCode = HttpStatusCode.InternalServerError;
            }

            return retorno;
        }

        private static HttpResponseMessage MontarResponseMessage(Clube clube, string arquivo)
        {
            HttpResponseMessage retorno;

            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(arquivo)))
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(ms.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-word");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = $"{ clube.Nome.Trim().Replace(' ', '_') }.docx";

                retorno = result;
            }

            return retorno;
        }

        public string PopularWord(Clube clube)
        {
            var dicionario = new Dictionary<string, string>();

            var caminhoDoNovoArquivo = CriarCopiaDoArquivoTemplate(clube.Nome);

            PopularDicionarioComDadosDoClube(clube).ToList()
                .ForEach(x => dicionario.Add(x.Key, x.Value));

            PopularDicionarioComCargosDoClube(clube).ToList()
                .ForEach(x => dicionario.Add(x.Key, x.Value));

            PopularTemplate(caminhoDoNovoArquivo, dicionario, clube);

            return caminhoDoNovoArquivo.ToString();
        }

        private static string CriarCopiaDoArquivoTemplate(string nomeDoClube)
        {
            var caminho = Environment.GetEnvironmentVariable("TEMP") + "\\";
            var caminhoTemplate = HttpContext.Current.Server.MapPath("~") + "Template.docx";
            var caminhoNovoArquivo = Path.Combine(caminho, ObterNomeDoArquivo(nomeDoClube));

            File.Copy(caminhoTemplate, caminhoNovoArquivo, true);

            return caminhoNovoArquivo;
        }

        private static string ObterNomeDoArquivo(string nomeDoClube)
        {
            return nomeDoClube.Replace(' ', '_').ToLower() + ".docx";
        }

        private static Dictionary<string, string> PopularDicionarioComCargosDoClube(Clube clube)
        {
            var dicionario = new Dictionary<string, string>();

            dicionario.Add("@presidente", ObterNome(clube.Presidente));
            dicionario.Add("@vice_presidente", ObterNome(clube.VicePresidente));
            dicionario.Add("@1_secretario", ObterNome(clube.PrimeiroSecretario));
            dicionario.Add("@2_secretario", ObterNome(clube.SegundoSecretario));
            dicionario.Add("@1_tesoureiro", ObterNome(clube.PrimeiroTesoureiro));
            dicionario.Add("@2_tesoureiro", ObterNome(clube.SegundoTesoureiro));
            dicionario.Add("@diretor_protocolo", ObterNome(clube.Protocolo));
            dicionario.Add("@diretor_servicos_internos", ObterNome(clube.ServicosInternos));
            dicionario.Add("@diretor_servicos_profissionais", ObterNome(clube.ServicosProfissionais));
            dicionario.Add("@diretor_servicos_comunidade", ObterNome(clube.ServicosComunidade));
            dicionario.Add("@diretor_servicos_internacionais", ObterNome(clube.ServicosInternacionais));
            dicionario.Add("@diretor_imagem_publica", ObterNome(clube.ImagemPublica));
            dicionario.Add("@past_president", ObterNome(clube.PastPresident));

            return dicionario;
        }

        private static Dictionary<string, string> PopularDicionarioComDadosDoClube(Clube clube)
        {
            var dicionario = new Dictionary<string, string>();

            dicionario.Add("@nome", clube.Nome.ToUpper());
            dicionario.Add("@data_fundacao", $"  { clube.DataFundacao.Day } DE { clube.DataFundacao.ToString("MMMM").ToUpper() } DE { clube.DataFundacao.Year }");
            dicionario.Add("@clube_padrinho", $"CLUBE PADRINHO: { clube.ClubePadrinho}");
            dicionario.Add("@reuniao_local", clube.ReuniaoLocal);
            dicionario.Add("@reuniao_horario", clube.ReuniaoHorario);
            dicionario.Add("@email", clube.Email);

            return dicionario;
        }

        private static string ObterNome(Socio socio)
        {
            return socio == null ? string.Empty : socio.Nome;
        }

        private static void PopularTemplate(string document, Dictionary<string, string> dicionario, Clube clube)
        {
            using (var wordDoc = WordprocessingDocument.Open(document, true))
            {
                PopularCamposComTextoSimples(wordDoc, dicionario);
                PopularTabelas(wordDoc, clube);
            }
        }

        private static void PopularCamposComTextoSimples(WordprocessingDocument wordDoc, Dictionary<string, string> dicionario)
        {
            var corpoDoWord = wordDoc.MainDocumentPart.Document.Body;

            var paragrafos = corpoDoWord.Elements<Paragraph>().ToList();

            foreach (var paragrafo in paragrafos)
            {
                var linha = paragrafo.InnerText;

                if (linha.Contains("@"))
                {
                    foreach (KeyValuePair<string, string> item in dicionario)
                    {
                        if (linha.Contains(item.Key))
                        {
                            var substituiu = false;

                            var textos = paragrafo.Elements<Run>().ToList();

                            foreach (var texto in textos)
                            {
                                if (texto.Elements<Text>().ToList()[0].Text.Contains(item.Key))
                                {
                                    substituiu = true;
                                    texto.Elements<Text>().ToList()[0].Text = texto.Elements<Text>().ToList()[0].Text.Replace(item.Key, item.Value);

                                    if (string.IsNullOrEmpty(texto.Elements<Text>().ToList()[0].Text))
                                    {
                                        paragrafo.Remove();
                                    }

                                    break;
                                }
                            }

                            if (!substituiu)
                            {
                                for (int i = 0; i < textos.Count; i++)
                                {
                                    if (textos[i].Elements<Text>().ToList()[0].Text.Contains(item.Key.Replace("@", string.Empty)))
                                    {
                                        textos[i - 1].Elements<Text>().ToList()[0].Text = textos[i - 1].Elements<Text>().ToList()[0].Text.Replace("@", string.Empty);
                                        textos[i].Elements<Text>().ToList()[0].Text = textos[i].Elements<Text>().ToList()[0].Text.Replace(item.Key.Replace("@", string.Empty), item.Value);

                                        if (string.IsNullOrEmpty(textos[i].Elements<Text>().ToList()[0].Text))
                                        {
                                            paragrafo.Remove();
                                        }

                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }

            //var listaComCadaUmaDasLinhasDoWord = corpoDoWord.Descendants<Text>().ToList();

            //for (int linhaAtual = 0; linhaAtual < listaComCadaUmaDasLinhasDoWord.Count; linhaAtual++)
            //{
            //    var linha = listaComCadaUmaDasLinhasDoWord[linhaAtual];

            //    foreach (KeyValuePair<string, string> item in dicionario)
            //    {
            //        if (linha.Text.Contains(item.Key))
            //        {
            //            linha.Text = linha.Text.Replace(item.Key, item.Value);

            //            if (linha.Text == string.Empty)
            //            {
            //                linha.Parent.Parent.Remove();
            //                linhaAtual--;
            //            }
            //        }
            //    }
            //}
        }

        private static void PopularTabelas(WordprocessingDocument wordDoc, Clube clube)
        {
            var body = wordDoc.MainDocumentPart.Document.Body;
            var tabelas = body.Elements<Table>().ToList();

            //Ex-presidentes
            for (int i = 0; clube.ExPresidentes != null && i < clube.ExPresidentes.Count; i++)
            {
                PopularTabelaSimples(tabelas[0], $"{i + 1}.  {clube.ExPresidentes[i].Nome} - Gestão {clube.ExPresidentes[i].De} - {clube.ExPresidentes[i].Ate}");
            }

            //Socios fundadores
            for (int i = 0; clube.SociosFundadores != null && i < clube.SociosFundadores.Count; i++)
            {
                PopularTabelaSimples(tabelas[1], $"{i + 1}.  {clube.SociosFundadores[i].Nome}");
            }

            //soscios honorarios
            for (int i = 0; clube.SociosHonorarios != null && i < clube.SociosHonorarios.Count; i++)
            {
                PopularTabelaSimples(tabelas[2], $"{i + 1}.  {clube.SociosHonorarios[i].Nome} - Gestão {clube.SociosHonorarios[i].De} - {clube.SociosHonorarios[i].Ate}");
            }

            for (int i = 0; clube.PaulHarris != null && i < clube.PaulHarris.Count; i++)
            {
                PopularTabelaSimples(tabelas[3], $"{i + 1}. {clube.PaulHarris[i].Nome}");
            }

            for (int i = 0; clube.ConcursosDistritais != null && i < clube.ConcursosDistritais.Count; i++)
            {
                PopularTabelaSimples(tabelas[4], $"{i + 1}. {clube.ConcursosDistritais[i].Descricao} - Gestão {clube.ConcursosDistritais[i].De} - {clube.ConcursosDistritais[i].Ate}");
            }

            for (int i = 0; clube.MencoesPresidenciais != null && i < clube.MencoesPresidenciais.Count; i++)
            {
                PopularTabelaSimples(tabelas[5], $"{i + 1}. Gestão {clube.MencoesPresidenciais[i].De} - {clube.MencoesPresidenciais[i].Ate}");
            }

            PopularTabelaComDespesas(tabelas[6], clube.Despesas);

            var countTabelas = 0;

            for (var i = 0; i < body.Elements().Count(); i++)
            {
                if (body.Elements().ToList()[i].GetType() == typeof(Table))
                {
                    countTabelas++;
                }

                if (countTabelas == 7)
                {
                    for (var x = i; x < i + clube.Despesas.Sum(z => z.DespesaItens.Count()) + 5; x++)
                    {
                        body.Elements().ToList()[x + 10].Remove();
                    }

                    break;
                }
            }

            PopularCalendario(tabelas.ToList(), clube.Calendario);
        }

        private static void PopularTabelaSimples(Table tabela, string texto)
        {
            tabela.Append(Utils.TabelaLinha(20, new List<Celula>
                {
                    new Celula
                    {
                        BorderValues = BorderValues.None,
                        Fonte = "Tahoma",
                        Negrito = false,
                        Sublinhado = false,
                        TamanhoFonte = 24,
                        Width = 9040,
                        Texto = texto
                    }
                }));
        }

        private static void PopularTabelaComDespesas(Table tabela, List<Despesa> despesas)
        {
            for (int count = 0; count < despesas.Count; count++)
            {
                tabela.Append(Utils.TabelaLinha(255, new List<Celula>
                {
                    new Celula
                    {
                        BorderValues = BorderValues.Nil,
                        Fonte = "Tahoma",
                        Width = 7160,
                        Texto = $"{count + 1}. {despesas[count].Descricao}",
                        Negrito = false,
                        Sublinhado = true,
                        TamanhoFonte = 20
                    }
                }));

                tabela.Append(new TableRow(new TableCell(new Paragraph(new Run(new Text(""))))));

                foreach (var despesaItem in despesas[count].DespesaItens)
                {
                    tabela.Append(Utils.TabelaLinha(255, new List<Celula>
                    {
                        new Celula
                        {
                            BorderValues = BorderValues.Single,
                            Fonte = "Tahoma",
                            Width = 7160,
                            Texto = despesaItem.Descricao,
                            Negrito = false,
                            Sublinhado = false,
                            TamanhoFonte = 20
                        },
                        new Celula
                        {
                            BorderValues = BorderValues.Single,
                            Fonte = "Tahoma",
                            Width = 7160,
                            Texto = $"R$ {despesaItem.Valor}",
                            Negrito = false,
                            Sublinhado = false,
                            TamanhoFonte = 20
                        }
                    }));
                }

                tabela.Append(Utils.TabelaLinha(255, new List<Celula>
                {
                    new Celula
                    {
                        BorderValues = BorderValues.Single,
                        Fonte = "Tahoma",
                        Width = 7160,
                        Texto = "SUB - TOTAL",
                        Negrito = true,
                        Sublinhado = false,
                        TamanhoFonte = 20
                    },
                    new Celula
                    {
                        BorderValues = BorderValues.Single,
                        Fonte = "Tahoma",
                        Width = 7160,
                        Texto = $"R$ {despesas[count].SubTotal}",
                        Negrito = true,
                        Sublinhado = false,
                        TamanhoFonte = 20
                    }
                }));

                tabela.Append(new TableRow(new TableCell(new Paragraph(new Run(new Text(""))))));
            }
        }

        private static void PopularCalendario(List<Table> tabelas, List<Evento> eventos)
        {
            foreach (var evento in eventos)
            {
                var tabela = tabelas[(int)evento.Mes];

                SublinharCalendario(evento, tabela);
                PopularEventosCalendario(evento, tabela);
            }
        }

        private static void PopularEventosCalendario(Evento evento, Table tabela)
        {
            for (int y = 12; y < tabela.ChildElements.Count; y++)
            {
                var linha = tabela.ChildElements[y].Elements<TableCell>().ToList();

                if (linha[0].InnerText.Trim() == evento.Dia.ToString())
                {
                    linha[2].Elements<Paragraph>().ToList()[0].Append(Utils.Texto("Tahoma", 15, false, true, evento.Quem));
                    linha[3].Elements<Paragraph>().ToList()[0].Append(Utils.Texto("Tahoma", 15, false, true, evento.Descricao));
                    linha[4].Elements<Paragraph>().ToList()[0].Append(Utils.Texto("Tahoma", 15, false, true, evento.Observacao));
                }
            }
        }

        private static void SublinharCalendario(Evento evento, Table tabela)
        {
            for (int y = 4; y < 10; y++)
            {
                for (int x = 1; x < 8; x++)
                {
                    if (tabela.ChildElements[y].ChildElements[x].ChildElements[1].ChildElements.Count < 2)
                    {
                        continue;
                    }

                    if (((Text)tabela.ChildElements[y].ChildElements[x].ChildElements[1].ChildElements[1].ChildElements[1]).Text.Trim() == evento.Dia.ToString())
                    {
                        if (tabela.ChildElements[y].ChildElements[x].ChildElements[0].Elements<Shading>().Count() > 0)
                        {
                            var cor = tabela.ChildElements[y].ChildElements[x].ChildElements[0].Elements<Shading>().ToList()[0];
                            tabela.ChildElements[y].ChildElements[x].ChildElements[0].Elements<Shading>().ToList()[0].Fill = "ffff00";
                            tabela.ChildElements[y].ChildElements[x].ChildElements[0].Elements<Shading>().ToList()[0].ThemeFill = null;// ThemeColorValues.Background1;
                        }
                        else
                        {
                            tabela.ChildElements[y].ChildElements[x].ChildElements[0].Append(new Shading() { Fill = "ffff00" });
                        }
                    }
                }
            }
        }
    }
}
