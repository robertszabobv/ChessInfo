'use strict';

chessApp.controller('PlayersController', 
    function PlayersController($scope, playersService) {
        $scope.players = [];
        $scope.filter = "";
        
        playersService.getPlayers(function(players) {
            $scope.players = players;
        });

        $scope.filterPlayersByLastName = function() {
            playersService.getPlayers(
                function onPlayersLoaded(playersFiltered) {
                    $scope.players = playersFiltered;
                }, 
                function onPlayersNotFound() {
                    $scope.players = [];
                },
                $scope.filter);
        }
});