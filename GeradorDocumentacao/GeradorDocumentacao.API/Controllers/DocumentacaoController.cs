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
using System.Web.Http;

namespace GeradorDocumentacao.API.Controllers
{
    public class DocumentacaoController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Gerar(Clube clube)
        {
            try
            {
                var arquivo = PopularWord(clube);

                using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(arquivo)))
                {
                    var result = new HttpResponseMessage(HttpStatusCode.OK);
                    result.Content = new ByteArrayContent(ms.ToArray());
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-word");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "HelloWorld.docx";
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string PopularWord(Clube clube)
        {
            var caminho = Environment.GetEnvironmentVariable("TEMP") + "\\";
            var template = System.Web.HttpContext.Current.Server.MapPath("~") + "Template.docx";
            var arquivo = caminho + clube.Nome.Replace(' ', '_').ToLower() + ".docx";

            System.IO.File.Copy(template, arquivo, true);

            var dicionario = new Dictionary<string, string>();

            ///Pagina 1
            dicionario.Add("@nome", clube.Nome.ToUpper());
            dicionario.Add("@data_fundacao", $"{ clube.DataFundacao.Day } DE { clube.DataFundacao.ToString("MMMM").ToUpper() } DE { clube.DataFundacao.Year }");
            dicionario.Add("@clube_padrinho", $"CLUBE PADRINHO: { clube.ClubePadrinho}");
            dicionario.Add("@reuniao_local", clube.ReuniaoLocal);
            dicionario.Add("@reuniao_horario", clube.ReuniaoHorario);
            dicionario.Add("@email", clube.Email);

            ///Pagina 3
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

            SearchAndReplace(arquivo, dicionario, clube);

            return arquivo.ToString();
        }

        private string ObterNome(Socio socio)
        {
            return socio == null ? string.Empty : socio.Nome;
        }

        private void SearchAndReplace(string document, Dictionary<string, string> dict, Clube clube)
        {
            using (var wordDoc = WordprocessingDocument.Open(document, true))
            {
                PopularCampos(wordDoc, dict);
                PopularTabelas(wordDoc, clube);
            }
        }

        private void PopularCampos(WordprocessingDocument wordDoc, Dictionary<string, string> dict)
        {
            var body = wordDoc.MainDocumentPart.Document.Body;

            var lista = body.Descendants<Text>().ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                var text = lista[i];

                foreach (KeyValuePair<string, string> item in dict)
                {
                    if (text.Text.Contains(item.Key))
                    {
                        text.Text = text.Text.Replace(item.Key, item.Value);

                        if (text.Text == string.Empty)
                        {
                            text.Parent.Parent.Remove();
                            i--;
                        }
                    }
                }
            }
        }

        private void PopularTabelas(WordprocessingDocument wordDoc, Clube clube)
        {
            var body = wordDoc.MainDocumentPart.Document.Body;
            var paras = body.Elements<Table>();

            //Ex-presidentes
            for (int i = 0; clube.ExPresidentes != null && i < clube.ExPresidentes.Count; i++)
            {
                PopularTabela(paras.ToList()[0], $"{i + 1}.  {clube.ExPresidentes[i].Nome} - Gestão {clube.ExPresidentes[i].De} - {clube.ExPresidentes[i].Ate}");
            }

            //Socios fundadores
            for (int i = 0; clube.SociosFundadores != null && i < clube.SociosFundadores.Count; i++)
            {
                PopularTabela(paras.ToList()[1], $"{i + 1}.  {clube.SociosFundadores[i].Nome} - Gestão {clube.SociosFundadores[i].De} - {clube.SociosFundadores[i].Ate}");
            }

            //soscios honorarios
            for (int i = 0; clube.SociosHonorarios != null && i < clube.SociosHonorarios.Count; i++)
            {
                PopularTabela(paras.ToList()[2], $"{i + 1}.  {clube.SociosHonorarios[i].Nome} - Gestão {clube.SociosHonorarios[i].De} - {clube.SociosHonorarios[i].Ate}");
            }

            for (int i = 0; clube.PaulHarris != null && i < clube.PaulHarris.Count; i++)
            {
                PopularTabela(paras.ToList()[3], $"{i + 1}. {clube.PaulHarris[i].Nome}");
            }

            for (int i = 0; clube.ConcursosDistritais != null && i < clube.ConcursosDistritais.Count; i++)
            {
                PopularTabela(paras.ToList()[4], $"{i + 1}. {clube.ConcursosDistritais[i].Descricao} - Gestão {clube.ConcursosDistritais[i].De} - {clube.ConcursosDistritais[i].Ate}");
            }

            for (int i = 0; clube.MencoesPresidenciais != null && i < clube.MencoesPresidenciais.Count; i++)
            {
                PopularTabela(paras.ToList()[5], $"{i + 1}. Gestão {clube.MencoesPresidenciais[i].De} - {clube.MencoesPresidenciais[i].Ate}");
            }

            //Popular despesas
            PopularTabelaComDespesas(paras.ToList()[6], clube.Despesas);
        }

        private void PopularTabela(Table tabela, string texto)
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

        private void PopularTabelaComDespesas(Table tabela, List<Despesa> despesas)
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
    }
}
