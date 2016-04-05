app.controller('CalendarioCtrl', function ($scope, $http) {

    Carregar();

    $scope.Salvar = function (dados) {

        localStorage.setItem('clube', JSON.stringify($scope.Clube));
        localStorage.removeItem('dados');
        location.assign('#/finalizar');
    }

    $scope.Novo = function (clube) {

        localStorage.setItem('clube', JSON.stringify(clube));
        location.assign('#/calendario/cadastro');
    }

    $scope.Editar = function (calendarioItem) {

        localStorage.setItem('clube', JSON.stringify($scope.Clube));
        localStorage.setItem('calendarioItem', JSON.stringify(calendarioItem));
        location.assign('#/calendario/cadastro');
    }

    $scope.Deletar = function (index) {

        $scope.Clube.Calendario.splice(index, 1);
    }

    function Carregar() {

        $scope.Clube = JSON.parse(localStorage.getItem('clube'));

        console.log($scope.Clube.Calendario);

        if ($scope.Clube.Calendario == null || $scope.Clube.Calendario == undefined)
        {
            $scope.Clube.Calendario = [];
        }
    }
});