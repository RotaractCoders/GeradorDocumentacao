using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GeradorDocumentacao.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using GeradorDocumentacao.Web.Word;

namespace GeradorDocumentacao.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public FileResult Gerar(Clube clube)
        {
            var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

            Response.AddHeader("Content-Disposition", "inline; filename=" + clube.Nome.Replace(' ', '_').ToLower() + ".docx");

            string arquivo;

            try
            {
                arquivo = PopularWord(clube);
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                return null;
            }

            return File(arquivo, contentType);
        }

        public string PopularWord(Clube clube)
        {
            clube = CarregarInicial();

            var caminho = Environment.GetEnvironmentVariable("TEMP") + "\\";
            var template = Server.MapPath("~") + "Template.docx";
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
            dicionario.Add("@presidente", ObterSocio("Presidente", clube));
            dicionario.Add("@vice_presidente", ObterSocio("Vice-Presidente", clube));
            dicionario.Add("@1_secretario", ObterSocio("1º Secretário", clube));
            dicionario.Add("@2_secretario", ObterSocio("2º Secretário", clube));
            dicionario.Add("@1_tesoureiro", ObterSocio("1º Tesoureiro", clube));
            dicionario.Add("@2_tesoureiro", ObterSocio("2º Tesoureiro", clube));
            dicionario.Add("@diretor_protocolo", ObterSocio("Diretor de protocólo", clube));
            dicionario.Add("@diretor_servicos_internos", ObterSocio("Diretor de serviços internos", clube));
            dicionario.Add("@diretor_servicos_profissionais", ObterSocio("Diretor de serviços profissionais", clube));
            dicionario.Add("@diretor_servicos_comunidade", ObterSocio("Diretor de serviços à comunidade", clube));
            dicionario.Add("@diretor_servicos_internacionais", ObterSocio("Diretor de serviços internacionais", clube));
            dicionario.Add("@diretor_imagem_publica", ObterSocio("Diretor de imagem publica", clube));
            dicionario.Add("@past_president", ObterSocio("Past president", clube));

            SearchAndReplace(arquivo, dicionario, clube);

            return arquivo.ToString();
        }

        private string ObterSocio(string cargo, Clube clube)
        {
            var socio = clube.Socios.FirstOrDefault(x => x.Cargos.Count(y => y.Descricao == cargo) > 0);

            if (socio == null)
            {
                return string.Empty;
            }
            else
            {
                return socio.Nome;
            }
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
            for (int i = 0; i < clube.ExPresidentes.Count; i++)
            {
                PopularTabela(paras.ToList()[0], $"{i + 1}.  {clube.ExPresidentes[i].Nome} - Gestão {clube.ExPresidentes[i].De} - {clube.ExPresidentes[i].Ate}");
            }

            //Socios fundadores
            for (int i = 0; i < clube.SociosFundadores.Count; i++)
            {
                PopularTabela(paras.ToList()[1], $"{i + 1}.  {clube.SociosFundadores[i].Nome} - Gestão {clube.SociosFundadores[i].De} - {clube.SociosFundadores[i].Ate}");
            }

            //soscios honorarios
            for (int i = 0; i < clube.SociosHonorarios.Count; i++)
            {
                PopularTabela(paras.ToList()[2], $"{i + 1}.  {clube.SociosHonorarios[i].Nome} - Gestão {clube.SociosHonorarios[i].De} - {clube.SociosHonorarios[i].Ate}");
            }

            for (int i = 0; i < clube.PaulHarris.Count; i++)
            {
                PopularTabela(paras.ToList()[3], $"{i + 1}. {clube.PaulHarris[i].Nome}");
            }

            for (int i = 0; i < clube.ConcursosDistritais.Count; i++)
            {
                PopularTabela(paras.ToList()[4], $"{i + 1}. {clube.ConcursosDistritais[i].Descricao} - Gestão {clube.ConcursosDistritais[i].De} - {clube.ConcursosDistritais[i].Ate}");
            }

            for (int i = 0; i < clube.MencoesPresidenciais.Count; i++)
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

        private Clube CarregarInicial()
        {
            return new Clube
            {
                Nome = "Rotaract clube Tremembé",
                DataFundacao = new DateTime(2000, 01, 05),
                ClubePadrinho = "Rotary Tremembé",
                ReuniaoLocal = "Rua francisco narciso, 300",
                ReuniaoHorario = "Quinzenamente aos domingos",
                Email = "contato@rotaracttremembe.com",
                ExPresidentes = new List<ExPresidente>
                {
                    new ExPresidente { Nome = "Eduardo", De = "2008", Ate = "2009" },
                    new ExPresidente { Nome = "Bruna", De = "2009", Ate = "2010" }
                },
                SociosFundadores = new List<SocioFundador>
                {
                    new SocioFundador { Nome = "Eduardo", De = "2008", Ate = "2009" },
                    new SocioFundador { Nome = "Bruna", De = "2009", Ate = "2010" }
                },
                SociosHonorarios = new List<SocioHonorario>
                {
                    new SocioHonorario { Nome = "Eduardo", De = "2008", Ate = "2009" },
                    new SocioHonorario { Nome = "Bruna", De = "2009", Ate = "2010" }
                },
                PaulHarris = new List<PaulHarris>
                {
                    new PaulHarris
                    {
                        Nome = "Manu"
                    },
                    new PaulHarris
                    {
                        Nome = "Obama"
                    }
                },
                ConcursosDistritais = new List<ConcursoDistrital>
                {
                    new ConcursoDistrital
                    {
                        Descricao = "Concurso de oratório",
                        De = "2014",
                        Ate = "2015"
                    },
                    new ConcursoDistrital
                    {
                        Descricao = "Melhor projeto - rotamira",
                        De = "2015",
                        Ate = "2016"
                    }
                },
                MencoesPresidenciais = new List<MencaoPresidencial>
                {
                    new MencaoPresidencial
                    {
                        De = "2008",
                        Ate = "2009"
                    },
                    new MencaoPresidencial
                    {
                        De = "2009",
                        Ate = "2010"
                    },
                    new MencaoPresidencial
                    {
                        De = "2010",
                        Ate = "2011"
                    }
                },
                Socios = new List<Socio>
                {
                    new Socio
                    {
                        Nome = "Eduardo Baltazar Fernandes",
                        Telefones = new List<Telefone>
                        {
                            new Telefone
                            {
                                DDD = "11",
                                Numero = "949554268"
                            }
                        },
                        Email = "edubalf@hotmail.com",
                        Cargos = new List<Cargo>
                        {
                            new Cargo
                            {
                                Descricao = "Vice-Presidente"
                            },
                            new Cargo
                            {
                                Descricao = "Diretor de protocólo"
                            }
                        }
                    },
                    new Socio
                    {
                        Nome = "Vitinho",
                        Telefones = new List<Telefone>
                        {
                            new Telefone
                            {
                                DDD = "11",
                                Numero = "29304802"
                            }
                        },
                        Email = "vitinho@hotmail.com",
                        Cargos = new List<Cargo>
                        {
                            new Cargo
                            {
                                Descricao = "Presidente"
                            }
                        }
                    },
                    new Socio
                    {
                        Nome = "Manu",
                        Telefones = new List<Telefone>
                        {
                            new Telefone
                            {
                                DDD = "11",
                                Numero = "235234645"
                            }
                        },
                        Email = "manu@hotmail.com",
                        Cargos = new List<Cargo>
                        {
                            new Cargo
                            {
                                Descricao = "Past president"
                            },
                            new Cargo
                            {
                                Descricao = "Diretor de serviços internos"
                            },
                            new Cargo
                            {
                                Descricao = "Diretor de serviços profissionais"
                            }
                        }
                    },
                    new Socio
                    {
                        Nome = "Obama",
                        Telefones = new List<Telefone>
                        {
                            new Telefone
                            {
                                DDD = "11",
                                Numero = "879234783"
                            }
                        },
                        Email = "obama@hotmail.com",
                        Cargos = new List<Cargo>
                        {
                            new Cargo
                            {
                                Descricao = "1º Secretário"
                            },
                            new Cargo
                            {
                                Descricao = "1º Tesoureiro"
                            },
                            new Cargo
                            {
                                Descricao = "Diretor de serviços à comunidade"
                            },
                            new Cargo
                            {
                                Descricao = "Diretor de serviços internacionais"
                            },
                            new Cargo
                            {
                                Descricao = "Diretor de imagem publica"
                            }
                        }
                    }
                },
                Despesas = new List<Despesa>
                {
                    new Despesa
                    {
                        Descricao = "Contribuições Obrigatórias (US$1,00 = R$ 2,06, ref. Jul/12)",
                        DespesaItens = new List<DespesaItem>
                        {
                            new DespesaItem
                            {
                                Descricao = "Per capita para RI em Jul/12 – 17 x US$27,00 x 2,06",
                                Valor = 945.54M
                            },
                            new DespesaItem
                            {
                                Descricao = "17 x US$27,00 x 2,06",
                                Valor = 45.54M
                            },
                            new DespesaItem
                            {
                                Descricao = "Per capita para RI em Jul/12",
                                Valor = 95.54M
                            }
                        }
                    },
                    new Despesa
                    {
                        Descricao = "Revista Rotária",
                        DespesaItens = new List<DespesaItem>
                        {
                            new DespesaItem
                            {
                                Descricao = "Brasil Rotário 18 x 4,80 x 12",
                                Valor = 1036.80M
                            }
                        }
                    }
                }
            };
        }
    }
}