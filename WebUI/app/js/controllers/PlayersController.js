'use strict';

chessApp.controller('PlayersController', 
    function PlayersController($scope, $log, playersService) {
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
            playersService.deletePlayer(
                function onPlayerDeleted() {
                    var index = $scope.players.indexOf(player);
                    $scope.players.splice(index, 1);
                    alert(player.firstName + " " + player.lastName + " deleted.");
                },
                function onPlayerHasGames() {
                    alert("Cannot delete " 
                        + player.firstName + " " + player.lastName
                        + ", because he/she is a player in one or more games. Please delete those games first.")
                },
                function onDeleteFailed(data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                },
                player.playerId
            );
        }

        $scope.editPlayer = function(player) {
            alert('Edit: ' + player.firstName + " " + player.lastName);
        }
});