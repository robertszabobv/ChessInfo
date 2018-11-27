'use strict';

chessApp.controller('EditGameController',
    function EditGameController($scope, playersService, $routeParams, $location, $log, gamesService) {
        

        
        getDefaultGame();

        playersService.getPlayers(function OnPlayersLoaded(players) {
            $scope.players = players;
            $scope.game.whitePlayer = players[0];
            $scope.game.blackPlayer = players[1];

            if($routeParams.gameId === undefined) {
                return;
            }
            gamesService.getGame($routeParams.gameId,
                function onGameLoaded(game) {
                    $scope.game = game;
                    // $scope.game.whitePlayer = game.whitePlayer;
                    // $scope.game.blackPlayer = game.blackPlayer;
                }
            );
        }) ;
        
        $scope.canCreateGame = function() {
            return areAtLeastTwoPlayers();
        } 

        function areAtLeastTwoPlayers() {
            if($scope.players === undefined || $scope.players === null) {
                return false;
            }
            return $scope.players.length > 1;
        }

        function areWhiteAndBlackDifferentPlayers() {
            return $scope.game.whitePlayer !== $scope.game.blackPlayer;
        }
        
        $scope.saveGame = function(game, gameForm) {
            if(gameForm.$valid && areWhiteAndBlackDifferentPlayers()) {
                gamesService.saveGame(game)
                .success(function(data, status) {
                    window.alert('game ' 
                        + game.whitePlayer.firstName + ' ' + game.whitePlayer.lastName 
                        + ' vs. ' 
                        + game.blackPlayer.firstName + ' ' + game.blackPlayer.lastName 
                        + ' saved!');
                    $location.url("/games");
                })
                .error(function(data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                });;
            }
        }

        $scope.cancel = function() {
            $location.url("/games");
        }

        function getGame() {
            if($routeParams.gameId === undefined) {
                return getDefaultGame();
            }
        }

        function getDefaultGame() {
            $scope.game = {};
            $scope.game.gameResult = "1";
            $scope.canCreateGame = false
        }
    }
);