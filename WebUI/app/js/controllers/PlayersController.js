'use strict';

chessApp.controller('PlayersController', 
    function PlayersController($scope, playersService) {
        $scope.players = {};

        // playersService.getPlayers(function(players) {
        //     $scope.players = players;
        // });
        $scope.players = playersService.getPlayers();
});