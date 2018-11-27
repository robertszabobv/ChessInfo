'use strict';

var chessApp = angular.module('chessApp', ['ngRoute'])
.config(function($routeProvider) {
    $routeProvider.when('/players',
    {
        templateUrl: "templates/Players.html",
        controller: "PlayersController"
    });
    $routeProvider.when('/editPlayer',
    {
        templateUrl: "templates/EditPlayer.html",
        controller: "EditPlayerController"
    });
    $routeProvider.when('/editPlayer/:playerId',
    {
        templateUrl: "templates/EditPlayer.html",
        controller: "EditPlayerController"
    });
    $routeProvider.when('/editGame',
    {
        templateUrl: "templates/EditGame.html",
        controller: "EditGameController"
    });
    $routeProvider.when('/games',
    {
        templateUrl: "templates/Games.html",
        controller: "GamesController"
    });
    $routeProvider.otherwise({redirectTo: '/players'});
});
