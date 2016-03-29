using System;
using System.Collections.Generic;

namespace GeradorDocumentacao.Web.Models
{
    public class Clube
    {
        public string Nome { get; set; }
        public DateTime DataFundacao { get; set; }
        public string ClubePadrinho { get; set; }
        public string ReuniaoLocal { get; set; }
        public string ReuniaoHorario { get; set; }
        public string Email { get; set; }
        public List<Socio> Socios { get; set; }
        public List<ExPresidente> ExPresidentes { get; set; }
        public List<SocioFundador> SociosFundadores { get; set; }
        public List<SocioHonorario> SociosHonorarios { get; set; }
        public List<PaulHarris> PaulHarris { get; set; }
        public List<ConcursoDistrital> ConcursosDistritais { get; set; }
        public List<MencaoPresidencial> MencoesPresidenciais { get; set; }
        public List<Despesa> Despesas { get; set; }
    }
}
