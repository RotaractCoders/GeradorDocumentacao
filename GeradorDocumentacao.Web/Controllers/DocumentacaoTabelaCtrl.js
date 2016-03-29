app.controller('DocumentacaoTabelaCtrl', function ($scope, $http) {

    Carregar();

	$scope.Avancar = function(clube) {
        
        localStorage.setItem('clube', JSON.stringify(clube));
        location.assign($scope.UrlAvancar);
    }
    
    $scope.Voltar = function() {
        
        location.assign($scope.UrlVoltar);
    }
    
    $scope.Novo = function(clube) {

        localStorage.setItem('clube', JSON.stringify(clube));
        location.assign($scope.UrlNovo);
    }
    
    $scope.Editar = function(dado) {
        
        localStorage.setItem('clube', JSON.stringify($scope.Clube));
        localStorage.setItem('dados', JSON.stringify(dado));
        location.assign($scope.UrlNovo);
    }
    
    $scope.Deletar = function(index) {
        
        $scope.Lista.splice(index, 1);
    }
    
    function Carregar() {
        localStorage.setItem('origem', window.location.hash.replace('#/',''));
        localStorage.removeItem('dados');
        
        var clube = localStorage.getItem('clube');
        
        if (clube != null && clube != undefined){
            $scope.Clube = JSON.parse(clube);
            console.log($scope.Clube);
        }
        
        PrepararCampos($scope);
    }
});