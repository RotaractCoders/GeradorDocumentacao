app.controller('CadastroDespesaItemCtrl', function ($scope, $http, $location) {
    
    var update = false;
    var despesa;
    
    Carregar();
    
    $scope.Voltar = function () {

        location.assign('#/' + $location.$$path.split('/')[1] + '/' + $location.$$path.split('/')[2]);
    }

	$scope.Salvar = function(despesaItem) {
        
        if (!update) {
            
            console.log(despesa);
            
            if (despesa.DespesaItens == null) {
                despesa.DespesaItens = [];
            }
            
            despesa.DespesaItens.push(despesaItem);    
        }
        
        localStorage.removeItem('despesa');
        localStorage.removeItem('despesaItem');
        localStorage.setItem('dados', JSON.stringify(despesa));
        localStorage.setItem('clube', JSON.stringify($scope.Clube));
        location.assign('#/despesas/cadastro');
    }
    
    function Carregar() {
        var clube = localStorage.getItem('clube');
        var despesaItemString = localStorage.getItem('despesaItem');
        var despesaString = localStorage.getItem('despesa');
        
        if (clube != null && clube != undefined) {
            $scope.Clube = JSON.parse(clube);
        }
        
        if (despesaString != null && despesaString != undefined) {
            despesa = JSON.parse(despesaString);
            console.log('Despesa:');
            console.log(despesa);
        }
        
        if (despesaItemString != null && despesaItemString != undefined) {
            
            var despesaItem = JSON.parse(despesaItemString);
            update = true;
            
            console.log($scope.Lista);
            console.log(despesaItem);
            
            if (despesa.DespesaItens == null) return;
            
            for (var i = 0; i < despesa.DespesaItens.length; i++) {
                
                var despesaItem = despesa.DespesaItens[i];
                
                if (despesaItem.$$hashKey == despesaItem.$$hashKey) {
                    
                    $scope.DespesaItem = despesaItem;
                    break;
                }
            }
        }
    }
});