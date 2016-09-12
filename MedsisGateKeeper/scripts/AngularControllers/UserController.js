
var app = angular.module('GateKpr', ["ngRoute", "ui.grid"]);

//-------ROUTES--------------------

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/CreateUser', {
            templateUrl: 'CreateUser.html',
            controller: 'GateKprCtrlUserController'
        }).
        when('/ListUsers', {
            templateUrl: 'ListUsers.html',
            controller: 'GateKprCtrlUserController'
        }).
        when('/EditUser/:UserId', {
            templateUrl: 'EditUser.cshtml',
            controller: 'GateKprCtrlEditUsers'
        }).
        otherwise({
           // redirectTo: '/addOrder'
        });
  }]);

//-------CONTROLLERS---------------

//Controller used by CreateUser.cshtml
app.controller('GateKprCtrlUserController', function ($scope, $http, UserService) {

    $scope.noSpaceEx = "^\\S+$";

    //To configure the bootstrap alert message
    //alert-success
    //alert-danger
    $scope.alertType = "";
    $scope.alertTypeText = "";
    $scope.alertMessage = "";
    $scope.RequestResult;

    $scope.btnCreateClick = function (ActionController) {
        $scope.alertType = "";
        //Call service to save user data and RequestData holds the service response
        UserService.CreateUser($scope.User, ActionController, function (RequestData) {            
            $scope.alertType = RequestData.alertType;
            $scope.alertTypeText = RequestData.alertTypeText;
            $scope.alertMessage = RequestData.alertMessage;

            //Clear values
            $scope.User.name = "";
            $scope.User.userName = "";
            $scope.User.password = "";
        });        
    }

    //$scope.btnCancelClick = function (ActionController) {
    //    UserService.CancelUser(ActionController);
    //}
});


////Controller used by ListUsers.cshtml
app.controller('GateKprCtrlListUsers', ['$scope', '$http', 'uiGridConstants', 'UserService', function ($scope, $http, uiGridConstants, UserService) {

    //Initial values when the page loads
    $scope.initPage = function (urlActionController) {
        $scope.loadListUsers(urlActionController);
    }   

    //Calls the Edit View
    $scope.clickEdit = function (value) {
        alert("Nothing");
        //$http.get('\\Account\\EditUser?UserEdit=' + value).then(function success (data) {
        var url="/Account/EditUser";
        $http({
            url: url,
            method: "GET",
            //headers: { 'Content-Type' : 'application/x-www-form-urlencoded'}//,
            params: { UserEdit: value}
        })
        //    .then(function success(data) {
        //    alert("hace algo..." + url);
        //},
        //function Error(data){
        //    alert("Se espaturro esta vaina");
        //});
    }   

    //Saves user datas
    $scope.clickEditUser = function (value) {
        //UserService.EditUser(value, function (value) {
        //    alert(value);
        //});
    }

    //Settings of the user list grid.
    $scope.gridLstUsersOptions = {
        data: "",        
        enableSorting: true,
        columnDefs: [
          { field: 'name', width: 150, displayName: 'Name'},
          { field: 'userName', width: 150, displayName: 'User Name' }, 
          {
              field: '_id', name: 'commandEdit', width: 150, displayName: '',
              enableColumnMenu: false, enableSorting: false,              
              cellTemplate: '<div><button ng-click="grid.appScope.clickEdit(row.entity._id)">Edit</button></div>'              
          }
        ]

    };

    $scope.loadListUsers = function (ActionController) {
        UserService.UserList(ActionController, function (RequestData) {
            $scope.gridLstUsersOptions.data = RequestData;
        });
    }
    
    $scope.EditUser = function (IdUser) {
        UserService.EditUser(IdUser, function (data) {
            alert("To edit ID: " + data);
        })
    }

}]);



app.controller('GateKprCtrlEditUsers', ['$scope', '$http', 'UserService', function ($scope, $http, UserService) {
    $scope.User = { name: "", userName: "", password: "" };

}]);
//----------------------------------//


//-------SERVICES-------------------

//Service to manage the Users
app.service('UserService', function ($http) {

    //Create user
    this.CreateUser = function (newUser, ActionController, RequestDataCallback) {
        var url = ActionController;
        var RequestResult = { alertType: "", alertTypeText: "", alertMessage: "" };

        //Invoke the server database method
        $http.post(url, newUser)
        .then(
            function (reponse) {
                if (reponse.data.RequestResultType == "Failure") {
                    RequestResult.alertType = "alert-danger";
                    RequestResult.alertTypeText = "Error!! ";
                }
                else {
                    RequestResult.alertType = "alert-success";
                    RequestResult.alertTypeText = "Success!! ";
                }                
                RequestResult.alertMessage = reponse.data.RequestResultMessage;

                RequestDataCallback(RequestResult);
            },
            function Error(reponse) {
                RequestResult.alertType = "alert-danger";
                RequestResult.alertMessage = reponse.data.RequestResultMessage;

                RequestDataCallback(RequestResult);
            }
        );
    }

    //List all users
    this.UserList = function (ActionController, RequestDataCallback) {        
        $http.get(ActionController)
        .success(function (data) {
            RequestDataCallback(data);            
        });
    }

    this.EditUser = function (IdUser, RequestDataCallback) {
        RequestDataCallback(IdUser);
    }

});