app.controller('FinalizarCtrl', function ($scope, $http) {

    Carregar();
    
    $scope.Voltar = function() {
        location.assign('#/calendario');
    }
    
    function Gerar(clube) {
        
        $http({
            method: 'POST',
            cache: false,
            url: 'http://rotaract4430api.azurewebsites.net/api',
            responseType:'arraybuffer',
            data: clube,
            headers: {
                'Content-Type': 'application/json'
            }})
            .success(function (data, status, headers, config) {
                console.log(headers.ContentDisposition);
                
                var blob = new Blob([data], {type: "application/vnd.ms-word", fileName: 'asdf.docx'});
                Save(blob, clube.Nome.replace(' ', '_') + '.docx');
                //var objectUrl = URL.createObjectURL(blob);
                //window.open(objectUrl);
            }).error(function (data, status, headers, config) {
                //upload failed
            });
    }
    
    function Save(blob, fileName) {
        if (window.navigator.msSaveOrOpenBlob) {
            navigator.msSaveBlob(blob, fileName);
        } else {
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.click();
            window.URL.revokeObjectURL(link.href);
        }
    }
    
    function Carregar() {
        
        $scope.Clube = JSON.parse(localStorage.getItem('clube'));

        Gerar($scope.Clube);
    }
});