chessApp.factory('gamesService', function($http, $log) {
    return {
        saveGame: function(game, successCallback) {
            $log.info(game);
            var gameDto = createDtoFrom(game);
            $http.post('/api/games', gameDto)
            .success(function(data, status, headers, config) {
                successCallback(data);          
            })
            .error(function(data, status, headers, config) {
                $log.warn(data, status, headers, config);          
            });
        }
        
    };

    function createDtoFrom(game) {
        return {
            whitePlayer: game.whitePlayer.playerId,
            blackPlayer: game.blackPlayer.playerId,
            gameDate: game.gameDate,
            openingClassification: game.openingClassification,
            result: game.result
        };
    }
})