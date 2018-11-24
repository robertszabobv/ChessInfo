'use strict';

chessApp.controller('EditGameController',
    function EditGameController($scope, playersService, $routeParams, $location, $log, gamesService) {
        
        $scope.game = {};
        $scope.game.result = "1";
        $scope.canCreateGame = false

        playersService.getPlayers(function OnPlayersLoaded(players) {
            $scope.players = players;
            $scope.game.whitePlayer = players[0];
            $scope.game.blackPlayer = players[1];            
        }) ;
        
        $scope.canCreateGame = function() {
            return areAtLeastTwoPlayers();
        } 

        function areAtLeastTwoPlayers() {
            if($scope.players === undefined || $scope.players === null) {
                return false;
            }
            return $scope.players.length > 2;
        }

        function areWhiteAndBlackDifferentPlayers() {
            return $scope.game.whitePlayer !== $scope.game.blackPlayer;
        }
        
        $scope.saveGame = function(game, gameForm) {
            if(gameForm.$valid && areWhiteAndBlackDifferentPlayers()) {
                gamesService.saveGame(game);
            }
        }

        $scope.cancel = function() {
            $location.url("/players");
        }
    }
);