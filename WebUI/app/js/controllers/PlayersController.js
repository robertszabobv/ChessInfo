'use strict';

chessApp.controller('PlayersController', 
    function PlayersController($scope, playersService) {
        $scope.players = [];
        $scope.filter = "";
    
        loadPlayers();

        $scope.filterPlayersByLastName = function() {
            loadPlayers();
        } ;
    
        function loadPlayers() {
            playersService.getPlayers(
                function onPlayersLoaded(playersFiltered) {
                    $scope.players = playersFiltered;
                }, 
                function onPlayersNotFound() {
                    $scope.players = [];
                },
                $scope.filter);
        }

        $scope.deletePlayer = function(player) {
            playerService.deletePlayer(
                function onPlayerDeleted() {

                },
                function onDeleteFailed() {
                    
                },
                player.playerId
            );
        }
});