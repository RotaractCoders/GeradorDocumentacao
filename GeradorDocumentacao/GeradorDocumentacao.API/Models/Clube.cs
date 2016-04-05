using System;
using System.Collections.Generic;

namespace GeradorDocumentacao.API.Models
{
    public class Clube
    {
        public string Nome { get; set; }
        public DateTime DataFundacao { get; set; }
        public string ClubePadrinho { get; set; }
        public string ReuniaoLocal { get; set; }
        public string ReuniaoHorario { get; set; }
        public string Email { get; set; }

        public Socio Presidente { get; set; }
        public Socio VicePresidente { get; set; }
        public Socio PrimeiroSecretario { get; set; }
        public Socio SegundoSecretario { get; set; }
        public Socio PrimeiroTesoureiro { get; set; }
        public Socio SegundoTesoureiro { get; set; }
        public Socio Protocolo { get; set; }
        public Socio ServicosInternos { get; set; }
        public Socio ServicosProfissionais { get; set; }
        public Socio ServicosComunidade { get; set; }
        public Socio ServicosInternacionais { get; set; }
        public Socio ImagemPublica { get; set; }
        public Socio PastPresident { get; set; }

        public List<Socio> Socios { get; set; }
        public List<ExPresidente> ExPresidentes { get; set; }
        public List<SocioFundador> SociosFundadores { get; set; }
        public List<SocioHonorario> SociosHonorarios { get; set; }
        public List<PaulHarris> PaulHarris { get; set; }
        public List<ConcursoDistrital> ConcursosDistritais { get; set; }
        public List<MencaoPresidencial> MencoesPresidenciais { get; set; }
        public List<Despesa> Despesas { get; set; }

        public List<Evento> Calendario { get; set; }
    }
}
