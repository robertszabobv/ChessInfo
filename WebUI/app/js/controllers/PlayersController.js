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
            playersService.getPlayers($scope.filter)
            .then(response => $scope.players = response.data)
            .catch(error => {
                if(error.status === 404) {
                    $scope.players = [];
                }
                $log.warn(error);
            });
        }

        $scope.deletePlayer = function(player) {
            playersService.deletePlayer(player.playerId)
            .then(response => {
                var index = $scope.players.indexOf(player);
                $scope.players.splice(index, 1);
                alert(player.firstName + " " + player.lastName + " deleted.");
            })
            .catch(error => {
                if(error.status === 400) {
                    alert("Cannot delete " 
                        + player.firstName + " " + player.lastName
                        + ", because he/she is a player in one or more games. Please delete those games first.")
                }
                $log.warn(error);
            });;
        }                     
});