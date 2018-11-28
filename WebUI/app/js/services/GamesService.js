chessApp.factory('gamesService', function($http, $log) {
    return {
        saveGame: function(game, successCallback) {
            return game.gameId === 0 
                ? $http.post('/api/games', createDtoFrom(game))
                : $http.put('/api/games', createDtoFrom(game));            
        },

        getGames: function(successCallback, notFoundCallback, playerFilter, openingFilter) {
            return $http.get(getGamesUrl(playerFilter, openingFilter))
            .success(function(data, status, headers, config) {
                successCallback(data);          
            })
            .error(function(data, status, headers, config) {
                if(status === 404) {
                    notFoundCallback();
                }
                else {
                    $log.warn(data, status, headers, config);
                };                       
            });
        },

        getGame: function(gameId, successCallback) {           
            $http.get( '/api/games/' + gameId)
            .success(function(data, status, headers, config) {
                successCallback(data);          
            })
            .error(function(data, status, headers, config) {
                $log.warn(data, status, headers, config);                       
            });
        },     

        deleteGame: function(successCallback, failedCallback, gameId) {
            return $http.delete('/api/games/' + gameId)
            .success(function(data, status, headers, config) {
                successCallback(data);
            })
            .error(function(data, status, headers, config) {                
                $log.warn(data, status, headers, config);
                failedCallback();                
            });
        }
    };

    function createDtoFrom(game) {
        return {
            gameId: game.gameId,
            whitePlayerId: game.whitePlayer.playerId,
            blackPlayerId: game.blackPlayer.playerId,
            gameDate: game.gameDate,
            openingClassification: game.openingClassification,
            resultDetail: game.resultDetail
        };
    }

    function getGamesUrl(playerFilter, openingFilter) {
        if((playerFilter === null || playerFilter === "") && (openingFilter === null || openingFilter === "")) {
            return "api/games";
        }
        var playerFilterValue = playerFilter === null ? '' : "playerLastName=" + playerFilter;
        var openingFilterValue = openingFilter === null ? '' : "&openingClassification=" + openingFilter;
        return encodeURI("api/games?" + playerFilterValue + openingFilterValue);
    }
})