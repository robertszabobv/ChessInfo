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

        $scope.deleteGame = function(game) {
            gamesService.deleteGame(
                function onGameDeleted() {
                    var index = $scope.games.indexOf(game);
                    $scope.games.splice(index, 1);
                    alert("game " + game.whitePlayer + " vs. " + game.blackPlayer + " deleted.");
                },
                function onDeleteFailed(data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                },
                game.gameId
            );
        }

        $scope.loadGames();
});