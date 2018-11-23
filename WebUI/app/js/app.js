'use strict';

var chessApp = angular.module('chessApp', ['ngRoute'])
.config(function($routeProvider) {
    $routeProvider.when('/players',
    {
        templateUrl: "templates/Players.html",
        controller: "PlayersController"
    });
});
