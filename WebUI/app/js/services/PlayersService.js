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

        getPlayers: function() {
            return [
                {
                    "playerId": 1,
                    "firstName": "John",
                    "lastName": "Doe",
                    "rating": 100
                },
                {
                    "playerId": 2,
                    "firstName": "Mary",
                    "lastName": "Chick",
                    "rating": 222
                },
                {
                    "playerId": 3,
                    "firstName": "Vincent",
                    "lastName": "Vega",
                    "rating": 666
                }
            ];
        }

    };
});