'use strict';

chessApp.controller('EditGameController',
    function EditGameController($scope, playersService, $routeParams, $location, $log) {
        
        $scope.game = {};

        playersService.getPlayers(function OnPlayersLoaded(players) {
            $scope.players = players;
            $scope.selectedWhitePlayer = players[0];
            $scope.selectedBlackPlayer = players[1];
        }) ;

        
        
    }
);