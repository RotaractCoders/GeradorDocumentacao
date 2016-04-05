function PrepararCampos(scope) {
    
    var origem = localStorage.getItem('origem');
    
    switch (origem)
    {
        case 'socios':
            
            scope.NomeVisivel = true;
            scope.EmailVisivel = true;
            scope.TelefoneVisivel = true;
            scope.DataNascimentoVisivel = true;
            
            scope.UrlVoltar = '#/';
            scope.UrlRetorno = '#/socios';
            scope.UrlAvancar = '#/cargos';
            scope.UrlNovo = '#/socios/cadastro';
            
            if (scope.Clube.Socios == null || scope.Clube.Socios == undefined)
            {
                scope.Clube.Socios = [];
            }
            
            scope.Lista = scope.Clube.Socios;
        break;
        
        case 'ex-presidentes':
            
            scope.NomeVisivel = true;
            scope.DeVisivel = true;
            scope.AteVisivel = true;
            
            scope.UrlVoltar = '#/cargos';
            scope.UrlRetorno = '#/ex-presidentes';
            scope.UrlAvancar = '#/socios-fundadores';
            scope.UrlNovo = '#/ex-presidentes/cadastro';
            
            if (scope.Clube.ExPresidentes == null || scope.Clube.ExPresidentes == undefined)
            {
                scope.Clube.ExPresidentes = [];
            }
            
            scope.Lista = scope.Clube.ExPresidentes;
        break;
        
        case 'socios-fundadores':
            
            scope.NomeVisivel = true;
            
            scope.UrlVoltar = '#/ex-presidentes';
            scope.UrlRetorno = '#/socios-fundadores';
            scope.UrlAvancar = '#/socios-honorarios';
            scope.UrlNovo = '#/socios-fundadores/cadastro';
            
            if (scope.Clube.SociosFundadores == null || scope.Clube.SociosFundadores == undefined)
            {
                scope.Clube.SociosFundadores = [];
            }
            
            scope.Lista = scope.Clube.SociosFundadores;
        break;
        
        case 'socios-honorarios':
            
            scope.NomeVisivel = true;
            scope.DeVisivel = true;
            scope.AteVisivel = true;
            
            scope.UrlVoltar = '#/socios-fundadores';
            scope.UrlRetorno = '#/socios-honorarios';
            scope.UrlAvancar = '#/paul-harris';
            scope.UrlNovo = '#/socios-honorarios/cadastro';
            
            if (scope.Clube.SociosHonorarios == null || scope.Clube.SociosHonorarios == undefined)
            {
                scope.Clube.SociosHonorarios = [];
            }
            
            scope.Lista = scope.Clube.SociosHonorarios;
        break;
        
        case 'paul-harris':
            
            scope.NomeVisivel = true;
            
            scope.UrlVoltar = '#/socios-fundadores';
            scope.UrlRetorno = '#/paul-harris';
            scope.UrlAvancar = '#/concursos-distritais';
            scope.UrlNovo = '#/paul-harris/cadastro';
            
            if (scope.Clube.PaulHarris == null || scope.Clube.PaulHarris == undefined)
            {
                scope.Clube.PaulHarris = [];
            }
            
            scope.Lista = scope.Clube.PaulHarris;
        break;
        
        case 'concursos-distritais':
            
            scope.DescricaoVisivel = true;
            scope.DeVisivel = true;
            scope.AteVisivel = true;
            
            scope.UrlVoltar = '#/paul-harris';
            scope.UrlRetorno = '#/concursos-distritais';
            scope.UrlAvancar = '#/mensoes-presidenciais';
            scope.UrlNovo = '#/concursos-distritais/cadastro';
            
            if (scope.Clube.ConcursosDistritais == null || scope.Clube.ConcursosDistritais == undefined)
            {
                scope.Clube.ConcursosDistritais = [];
            }
            
            scope.Lista = scope.Clube.ConcursosDistritais;
        break;
        
        case 'mensoes-presidenciais':
            
            scope.DeVisivel = true;
            scope.AteVisivel = true;
            
            scope.UrlVoltar = '#/concursos-distritais';
            scope.UrlRetorno = '#/mensoes-presidenciais';
            scope.UrlAvancar = '#/despesas';
            scope.UrlNovo = '#/mensoes-presidenciais/cadastro';
            
            if (scope.Clube.MensoesPresidenciais == null || scope.Clube.MensoesPresidenciais == undefined)
            {
                scope.Clube.MensoesPresidenciais = [];
            }
            
            scope.Lista = scope.Clube.MensoesPresidenciais;
        break;
        
        case 'despesas':
            
            scope.DescricaoVisivel = true;
            
            scope.UrlVoltar = '#/mensoes-presidenciais';
            scope.UrlRetorno = '#/despesas';
            scope.UrlAvancar = '#/calendario';
            scope.UrlNovo = '#/despesas/cadastro';
            
            if (scope.Clube.Despesas == null || scope.Clube.Despesas == undefined)
            {
                scope.Clube.Despesas = [];
            }
            
            scope.Lista = scope.Clube.Despesas;
        break;
    }
}