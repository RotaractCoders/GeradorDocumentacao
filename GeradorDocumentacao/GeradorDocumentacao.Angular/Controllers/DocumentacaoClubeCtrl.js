app.controller('DocumentacaoClubeCtrl', function ($scope, $http, $filter) {
    
    Carregar();
    
	$scope.AvancarParaSocios = function(clube) {
        
        console.log('funfo');
        localStorage.setItem('clube', JSON.stringify(clube));
        location.assign("#/socios");
    }
    
	$scope.$watch('Clube.DataFundacao', function (newValue) {
	    $scope.Clube.DataFundacao = $filter('date')(newValue, 'yyyy-MM-dd');
	});

    function Carregar() {
        var clube = localStorage.getItem('clube');
        
        if (clube != null && clube != undefined){
            $scope.Clube = JSON.parse(clube);
        }
    }
});