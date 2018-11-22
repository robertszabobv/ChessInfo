'use strict';

chessApp.controller('EditPlayerController',
    function EditPlayerController($scope, playersService) {
        $scope.player = {};

        $scope.savePlayer = function(player, playerForm) {
            if(playerForm.$valid) {
                playersService.savePlayer(player)
                .success(function(data, status) {
                    window.alert('player ' + player.firstName + ' ' + player.lastName + ' saved!');
                    //window.location = "";
                })
                .error(function(data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                });                                
            }            
        }

        $scope.cancel = function() {
            window.location = "http://www.google.com";
        }
    }
);
