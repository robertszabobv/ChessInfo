'use strict';

chessApp.controller('PlayersController', 
    function PlayersController($scope, playersService) {
        $scope.players = {};
        $scope.filter = "";

        playersService.getPlayers(function(players) {
            $scope.players = players;
        });

        $scope.filterPlayersByLastName = function() {
            playersService.filterPlayers(function(playersFiltered) {
                $scope.players = playersFiltered;
            }, 
            $scope.filter);
            // $scope.players = playersService.filterPlayers($scope.filter);
        }
});