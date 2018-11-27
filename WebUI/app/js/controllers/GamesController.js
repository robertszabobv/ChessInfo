'use strict';

chessApp.controller('GamesController',
    function GamesController($scope, $log, gamesService) {
        $scope.playerFilter = "";
        $scope.openingFilter = "";

        

        $scope.loadGames = function() {
            gamesService.getGames(function onGamesLoaded(gamesFiltered) {
                $scope.games = gamesFiltered;
            },
            function onGamesNotFound() {
                $scope.games = [];
            },
            $scope.playerFilter,
            $scope.openingFilter)
        }

        $scope.loadGames();        
});