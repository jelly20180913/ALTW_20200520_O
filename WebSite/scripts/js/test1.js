angular.module("myApp", [])
angular.module("myApp").controller('myController', function ($http, $scope) {
    $scope.ngpost = function () {
        alert(sessionStorage.getItem("LoginId"));
        var account = document.getElementById("account").value;
        var MyJson = { "account": account, "pwd": "123" };

        if (MyJson.pwd == "123") {
            $http.post('http://localhost:52006/api/testpost', JSON.stringify(MyJson)).then(function (response) {
                if (response.data.Data.length == 0) {
                    { alert("no data!") }
                    //alert(JSON.stringify(response.data));
                }

                $scope.employees = response.data.Data;

            }, function (response) {
                alert("error!");
            });
        }

        if (MyJson.pwd == "456") {
            var config = {
                params: JSON.stringify(MyJson), 
                headers: { 'Accept': 'application/json' }
            };
            $http.get('http://localhost:52006/api/testpost',config).then(function (response) {
                    //alert(JSON.stringify(response.data.Data));
                $scope.employees = response.data.Data;

            }, function (response) {
                alert("error!");
            });
        }
    }
});

