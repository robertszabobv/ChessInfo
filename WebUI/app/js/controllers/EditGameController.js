'use strict';

chessApp.controller('EditGameController',
    function EditGameController($scope, playersService, $routeParams, $location, $log, gamesService) {
        $scope.resultOptions = [
            {
                "resultType": 1,
                "display": "1-0"
            },
            {
                "resultType": 2,
                "display": "0.5-0.5"
            },
            {
                "resultType": 3,
                "display": "0-1"
            },
        ];

        getDefaultGame();

        playersService.getPlayersForSelection()
        .then(response => {
            $scope.players = response.data;
            initDefaultGamePlayers(response.data);
            if($routeParams.gameId === undefined) {
                return;
            }
            initGame();
        })
        .catch(error => $log.log(error));
       
        function initGame() {
            gamesService.getGame($routeParams.gameId,
                function onGameLoaded(game) {
                    var gameDateString = game.gameDate;
                    game.gameDate = new Date(gameDateString);
                    $scope.game = game;                  
                }
            );
        }

        function initDefaultGamePlayers(players) {
            $scope.game.whitePlayer = players[0];
            $scope.game.blackPlayer = players[1];
        }
        
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
            $scope.game.gameId = 0;
            $scope.canCreateGame = false
        }
    }
);