app.controller('DocumentacaoClubeCtrl', function ($scope, $http) {
    
    Carregar();
    
	$scope.AvancarParaSocios = function(clube) {
        
        console.log('funfo');
        localStorage.setItem('clube', JSON.stringify(clube));
        location.assign("#/socios");
    }
    
    function Carregar() {
        var clube = localStorage.getItem('clube');
        
        if (clube != null && clube != undefined){
            $scope.Clube = JSON.parse(clube);
            console.log($scope.Clube);
        }
    }
});