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

            scope.Titulo = 'Socios 2/11';
            
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
            
            scope.Titulo = 'Ex-Presidentes 4/11';

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
            
            scope.Titulo = 'Socios-Fundadores 5/11';

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
            
            scope.Titulo = 'Socios-Honorarios 6/11';

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
            
            scope.Titulo = 'Paul-Harris 7/11';

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
            
            scope.Titulo = 'Concursos-Distritais 8/11';

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
            
            scope.Titulo = 'Mensoes-Presidenciais 9/11';

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
            
            scope.Titulo = 'Despesas 10/11';

            if (scope.Clube.Despesas == null || scope.Clube.Despesas == undefined)
            {
                scope.Clube.Despesas = [];
            }
            
            scope.Lista = scope.Clube.Despesas;
        break;
    }
}