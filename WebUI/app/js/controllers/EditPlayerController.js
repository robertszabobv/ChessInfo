'use strict';

chessApp.controller('EditPlayerController',
    function EditPlayerController($scope, playersService, $routeParams, $location, $log) {       
        $scope.player = getPlayer();

        $scope.savePlayer = function(player, playerForm) {
            if(playerForm.$valid) {
                playersService.savePlayer(player)
                .then(result => {
                    window.alert('player ' + player.firstName + ' ' + player.lastName + ' saved!');
                    $location.url("/players");
                })
                .catch(error => $log.warn(error));
            } 
        };        

        $scope.cancel = function() {
            $location.url("/players");
        };  
        
        function getPlayer() {
            if($routeParams.playerId === undefined) {
                return {};
            }
            playersService.getPlayer($routeParams.playerId,
                function onPlayerLoaded(player) {
                    $scope.player = player
                });
        };
    }
);
