app.controller('DocumentacaoCargoCtrl', function ($scope, $http) {
    
    Carregar();
    
	$scope.Avancar = function(clube) {
        
        localStorage.setItem('clube', JSON.stringify(clube));
        location.assign('#/ex-presidentes');
    }
    
    $scope.Voltar = function() {
        
        location.assign('#/socios');
    }
    
    function Carregar() {
        var clube = localStorage.getItem('clube');
        
        if (clube != null && clube != undefined){
            $scope.Clube = JSON.parse(clube);
            console.log($scope.Clube);
        }
    }
});