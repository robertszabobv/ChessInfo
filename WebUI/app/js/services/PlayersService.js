chessApp.factory('playersService', function($http, $log) {
    function getPlayersUrl(lastName) {
            return (lastName == null)
            ? "/api/players"
            : "/api/players" + "?lastName=" + lastName;
    };

    return {
        savePlayer: function(player) {
            if (player.playerId !== undefined) {
                return $http.put('/api/players', player); 
            }
            return $http.post('/api/players', player);        
        },

        getPlayer: function(playerId) {           
            return $http.get( '/api/players/' + playerId);            
        },     
        
        getPlayers: function(lastName) {          
            return $http.get(getPlayersUrl(lastName));           
        }, 
       
        getPlayersForSelection: function(lastName) {
            return $http.get(getPlayersUrl(lastName));             
        },

        deletePlayer: function(playerId) {
            return $http.delete('/api/players/' + playerId);            
        }             
    };
});