'use strict';

chessApp.controller('EditGameController',
    function EditGameController($scope, playersService, $routeParams, $location, $log) {
        
        $scope.game = {};
        $scope.game.result = "1";
        $scope.canCreateGame = false

        playersService.getPlayers(function OnPlayersLoaded(players) {
            $scope.players = players;
            $scope.game.whitePlayer = players[0];
            $scope.game.blackPlayer = players[1];
            $scope.canCreateGame = areAtLeastTwoPlayers(players);
        }) ;

        function areAtLeastTwoPlayers(players) {
            if(players === undefined || players === null) {
                return false;
            }
            return players.length > 2;
        }
        
        $scope.saveGame = function(game, gameForm) {
            if(gameForm.$valid) {
                alert('ok');
            }
        }

        $scope.cancel = function() {
            $location.url("/players");
        }
    }
);