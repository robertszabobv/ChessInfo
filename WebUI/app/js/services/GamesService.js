chessApp.factory('gamesService', function($http, $log) {
    return {
        saveGame: function(game, successCallback) {
            return game.gameId === 0 
                ? $http.post('/api/games', createDtoFrom(game))
                : $http.put('/api/games', createDtoFrom(game));            
        },

        getGames: function(playerFilter, openingFilter) {
            return $http.get(getGamesUrl(playerFilter, openingFilter));          
        },

        getGame: function(gameId) {
            return $http.get( '/api/games/' + gameId);
        },
 
        deleteGame: function(gameId) {
            return $http.delete('/api/games/' + gameId);           
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