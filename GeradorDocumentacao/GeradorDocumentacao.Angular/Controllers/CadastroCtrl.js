app.controller('CadastroCtrl', function ($scope, $http) {
    
    var update = false;
    
    Carregar();
    
	$scope.Salvar = function(dados) {
        
        
        if (!update) {
            
            if ($scope.Lista == null) {
                $scope.Lista = [];
            }
            
            $scope.Lista.push(dados);    
        }
        
        localStorage.setItem('clube', JSON.stringify($scope.Clube));
        location.assign($scope.UrlRetorno);
    }
    
    function Carregar() {
        var clube = localStorage.getItem('clube');
        var dadosString = localStorage.getItem('dados');
        
        if (clube != null && clube != undefined) {
            $scope.Clube = JSON.parse(clube);
            PrepararCampos($scope);
        }
        
        if (dadosString != null && dadosString != undefined) {
            
            var dados = JSON.parse(dadosString);
            update = true;
            
            console.log($scope.Lista);
            console.log(dados);
            
            for (var i = 0; i < $scope.Lista.length; i++) {
                
                if ($scope.Lista[i].$$hashKey == dados.$$hashKey) {
                    
                    $scope.Dados = $scope.Lista[i];
                    break;
                }    
            }
        }
    }
});