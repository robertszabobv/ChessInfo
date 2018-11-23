'use strict';

chessApp.controller('EditPlayerController',
    function EditPlayerController($scope, playersService, $routeParams, $location, $log) {       
        $scope.player = getPlayer();

        $scope.savePlayer = function(player, playerForm) {
            if(playerForm.$valid) {
                playersService.savePlayer(player)
                .success(function(data, status) {
                    window.alert('player ' + player.firstName + ' ' + player.lastName + ' saved!');
                    $location.url("/players");
                })
                .error(function(data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                });
            }
        }

        $scope.cancel = function() {
            $location.url("/players");
        }      
        
        function getPlayer() {
            if($routeParams.playerId === undefined) {
                return {};
            }
            playersService.getPlayer($routeParams.playerId,
                function onPlayerLoaded(player) {
                    $scope.player = player
                });
        }
    }
);
