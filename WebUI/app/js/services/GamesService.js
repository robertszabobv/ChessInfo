chessApp.factory('gamesService', function($http, $log) {
    return {
        saveGame: function(game, successCallback) {
            return $http.post('/api/games', createDtoFrom(game));            
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
})