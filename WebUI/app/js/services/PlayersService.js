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

        getPlayer: function(playerId, successCallback) {           
            $http.get( '/api/players/' + playerId)
            .success(function(data, status, headers, config) {
                successCallback(data);          
            })
            .error(function(data, status, headers, config) {
                $log.warn(data, status, headers, config);                       
            });
        },       

        getPlayers: function(successCallback, notFoundCallback, lastName) {          
            return $http.get(getPlayersUrl(lastName))
            .success(function(data, status, headers, config) {
                successCallback(data);
            })
            .error(function(data, status, headers, config){
                if(status === 404) {
                    notFoundCallback();
                }
                else {
                    $log.warn(data, status, headers, config);
                };                
            });
        }, 
        
        deletePlayer: function(successCallback, failedCallback, playerId) {
            return $http.delete('/api/players/' + playerId)
            .success(function(data, status, headers, config) {
                successCallback(data);
            })
            .error(function(data, status, headers, config) {
                $log.warn(data, status, headers, config);
                failedCallback();                
            });
        }
    };
});