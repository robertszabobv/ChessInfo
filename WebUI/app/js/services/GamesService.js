chessApp.factory('gamesService', function($http, $log) {
    return {
        saveGame: function(game, successCallback) {
            return $http.post('/api/games', createDtoFrom(game));            
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
        }        
    };

    function createDtoFrom(game) {
        return {
            whitePlayerId: game.whitePlayer.playerId,
            blackPlayerId: game.blackPlayer.playerId,
            gameDate: game.gameDate,
            openingClassification: game.openingClassification,
            gameResult: game.result
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