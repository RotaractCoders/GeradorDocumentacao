var app = angular
    .module("app", ['ui.date', 'ngRoute', 'angular.filter'])
    .directive('formatDate',formatDate);

function formatDate(){
    return {
        require: 'ngModel',
        link: function(scope, elem, attr, modelCtrl) {
            modelCtrl.$formatters.push(function(modelValue){
                return new Date(modelValue);
            })
        }
    }
}

app.config(['$routeProvider', function ($routeProvider) {
	$routeProvider
	    .when("/", {
	        templateUrl: "Views/DocumentacaoClube.html",
	        controller: "DocumentacaoClubeCtrl"
	    })
        .when("/socios", {
            templateUrl: "Views/DocumentacaoTabela.html",
            controller: "DocumentacaoTabelaCtrl"
        })
        .when("/socios/cadastro", {
            templateUrl: "Views/Cadastro.html",
            controller: "CadastroCtrl"
        })
        .when("/cargos", {
            templateUrl: "Views/DocumentacaoCargo.html",
            controller: "DocumentacaoCargoCtrl"
        })
        .when("/ex-presidentes", {
            templateUrl: "Views/DocumentacaoTabela.html",
            controller: "DocumentacaoTabelaCtrl"
        })
        .when("/ex-presidentes/cadastro", {
            templateUrl: "Views/Cadastro.html",
            controller: "CadastroCtrl"
        })
        .when("/socios-fundadores", {
            templateUrl: "Views/DocumentacaoTabela.html",
            controller: "DocumentacaoTabelaCtrl"
        })
        .when("/socios-fundadores/cadastro", {
            templateUrl: "Views/Cadastro.html",
            controller: "CadastroCtrl"
        })
        .when("/socios-honorarios", {
            templateUrl: "Views/DocumentacaoTabela.html",
            controller: "DocumentacaoTabelaCtrl"
        })
        .when("/socios-honorarios/cadastro", {
            templateUrl: "Views/Cadastro.html",
            controller: "CadastroCtrl"
        })
        .when("/paul-harris", {
            templateUrl: "Views/DocumentacaoTabela.html",
            controller: "DocumentacaoTabelaCtrl"
        })
        .when("/paul-harris/cadastro", {
            templateUrl: "Views/Cadastro.html",
            controller: "CadastroCtrl"
        })
        .when("/concursos-distritais", {
            templateUrl: "Views/DocumentacaoTabela.html",
            controller: "DocumentacaoTabelaCtrl"
        })
        .when("/concursos-distritais/cadastro", {
            templateUrl: "Views/Cadastro.html",
            controller: "CadastroCtrl"
        })
        .when("/mensoes-presidenciais", {
            templateUrl: "Views/DocumentacaoTabela.html",
            controller: "DocumentacaoTabelaCtrl"
        })
        .when("/mensoes-presidenciais/cadastro", {
            templateUrl: "Views/Cadastro.html",
            controller: "CadastroCtrl"
        })
        .when("/despesas", {
            templateUrl: "Views/DocumentacaoTabela.html",
            controller: "DocumentacaoTabelaCtrl"
        })
        .when("/despesas/cadastro", {
            templateUrl: "Views/CadastroDespesa.html",
            controller: "CadastroDespesaCtrl"
        })
        .when("/despesas/cadastro/item", {
            templateUrl: "Views/CadastroDespesaItem.html",
            controller: "CadastroDespesaItemCtrl"
        })
        .when("/calendario", {
            templateUrl: "Views/Calendario.html",
            controller: "CalendarioCtrl"
        })
        .when("/calendario/cadastro", {
            templateUrl: "Views/CalendarioItem.html",
            controller: "CalendarioItemCtrl"
        })
        .when("/finalizar", {
            templateUrl: "Views/Finalizar.html",
            controller: "FinalizarCtrl"
        })
	    .otherwise("/404", {
	        templateUrl: "Views/404.html"
	    });
}]);