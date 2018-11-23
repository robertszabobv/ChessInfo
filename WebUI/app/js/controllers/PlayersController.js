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
            playersService.deletePlayer(
                function onPlayerDeleted() {
                    var index = $scope.players.indexOf(player);
                    $scope.players.splice(index, 1);
                    alert(player.firstName + " " + player.lastName + " deleted.");
                },
                function onDeleteFailed() {
                    
                },
                player.playerId
            );
        }

        $scope.editPlayer = function(player) {
            alert('Edit: ' + player.firstName + " " + player.lastName);
        }
});