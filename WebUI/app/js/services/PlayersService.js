chessApp.factory('playersService', function($http, $log) {
    return {
        savePlayer: function(player) {      
            return $http.post('/api/players', player);        
        },
        getPlayer: function() {           
            $http( {method: 'GET', url: '/api/players/1'} )
            .success(function(data, status, headers, config) {
                $log.info(data, status);                
            })
            .error(function(data, status, headers, config) {
                $log.warn(data, status, headers, config);
                
            });
        },

        getPlayers: function(successCallback) {
            return $http.get("/api/players")
            .success(function(data, status, headers, config) {
                successCallback(data);
            })
            .error(function(data, status, headers, config){
                $log.warn(data, status, headers, config);
            });
        },

        filterPlayers: function(successCallback, lastName) {
            var filterPlayersUrl = "/api/players" + "?lastName=" + lastName;
            return $http.get(filterPlayersUrl)
            .success(function(data, status, headers, config) {
                successCallback(data);
            })
            .error(function(data, status, headers, config){
                $log.warn(data, status, headers, config);
            });           
        }
    };
});