app.controller('CadastroDespesaCtrl', function ($scope, $http) {
    
    var update = false;
    
    Carregar();
    
	$scope.Salvar = function(dados) {
        
        if (!update) {
            
            console.log('Despesas:');
            console.log($scope.Clube.Despesas);
            
            if ($scope.Clube.Despesas == null) {
                $scope.Clube.Despesas = [];
            }
            
            $scope.Clube.Despesas.push(dados);    
        }
        
        localStorage.setItem('clube', JSON.stringify($scope.Clube));
        localStorage.removeItem('dados');
        location.assign('#/despesas');
    }
    
    $scope.Novo = function(clube) {
        
        if (!update) {
            
            if ($scope.Clube.Despesas == null) {
                $scope.Clube.Despesas = [];
            }
            
            $scope.Clube.Despesas.push($scope.Despesa);    
        }
        
        localStorage.setItem('clube', JSON.stringify(clube));
        localStorage.setItem('despesa', JSON.stringify($scope.Despesa));
        location.assign('#/despesas/cadastro/item');
    }
    
    $scope.Editar = function(despesaItem) {
        
        localStorage.setItem('clube', JSON.stringify($scope.Clube));
        localStorage.setItem('despesaItem', JSON.stringify(despesaItem));
        localStorage.setItem('despesa', JSON.stringify($scope.Despesa));
        location.assign('#/despesas/cadastro/item');
    }
    
    $scope.Deletar = function(index) {
        
        $scope.Despesa.DespesaItens.splice(index, 1);
    }
    
    function Carregar() {
        var clube = localStorage.getItem('clube');
        var despesaString = localStorage.getItem('dados');
        
        if (clube != null && clube != undefined) {
            $scope.Clube = JSON.parse(clube);
        }
        
        if (despesaString != null && despesaString != undefined) {
            
            var encontrou = false;
            var despesa = JSON.parse(despesaString);
            update = true;
            
            console.log($scope.Clube);
            
            for (var i = 0; i < $scope.Clube.Despesas.length; i++) {
                
                if ($scope.Clube.Despesas[i].$$hashKey == despesa.$$hashKey) {
                    
                    console.log('encontrou');
                    console.log(despesa);
                    
                    if (despesa != null) {
                        $scope.Clube.Despesas[i] = despesa;    
                    }
                    
                    $scope.Despesa = $scope.Clube.Despesas[i];
                    encontrou = true;
                    
                    break;
                }
            }
            
            if (!encontrou) {
                console.log('não encontrou');
                
                for (var i = 0; i < $scope.Clube.Despesas.length; i++) {
                
                    if ($scope.Clube.Despesas.Descricao == despesa.Descricao) {
                        
                        $scope.Despesa = $scope.Clube.Despesas;
                        encontrou = true;
                        break;
                    }
                }
            }
        }
    }
});