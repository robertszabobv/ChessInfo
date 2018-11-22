chessApp.factory('playersService', function($http, $log) {
    function getPlayersUrl(lastName) {
            return (lastName == null)
            ? "/api/players"
            : "/api/players" + "?lastName=" + lastName;
    };

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

        getPlayers: function(successCallback, lastName) {          
            return $http.get(getPlayersUrl(lastName))
            .success(function(data, status, headers, config) {
                successCallback(data);
            })
            .error(function(data, status, headers, config){
                $log.warn(data, status, headers, config);
            });
        },              
    };
});