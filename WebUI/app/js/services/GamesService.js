chessApp.factory('gamesService', function($http, $log) {
    return {
        saveGame: function(game) {
            $log.info(game);
        }
    };
})