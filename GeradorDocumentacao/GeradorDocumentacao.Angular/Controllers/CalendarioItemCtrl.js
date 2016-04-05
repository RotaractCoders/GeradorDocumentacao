app.controller('CalendarioItemCtrl', function ($scope, $http) {
    
    var update = false;

    Carregar();

    $scope.Salvar = function (calendarioItem) {

        if (!update)
        {
            $scope.Clube.Calendario.push(calendarioItem);
        }

        localStorage.removeItem('calendarioItem');
        localStorage.setItem('clube', JSON.stringify($scope.Clube));
        location.assign('#/calendario');
    }

    function Carregar() {
        $scope.Clube = JSON.parse(localStorage.getItem('clube'));
        var calendarioItemString = localStorage.getItem('calendarioItem');

        if (calendarioItemString != null && calendarioItemString != undefined) {

            update = true;

            var calendarioItem = JSON.parse(calendarioItemString);

            for (var i = 0; i < $scope.Clube.Calendario.length; i++) {

                if ($scope.Clube.Calendario[i].$$hashKey == calendarioItem.$$hashKey) {

                    $scope.CalendarioItem = $scope.Clube.Calendario[i];
                    break;
                }
            }
        }
    }
});