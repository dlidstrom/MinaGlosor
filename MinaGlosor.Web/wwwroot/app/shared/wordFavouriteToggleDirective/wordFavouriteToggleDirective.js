(function () {
    'use strict';

    angular.module('mgApp')
        .directive('wordFavouriteToggle', WordFavouriteToggleDirective)
        .controller('WordFavouriteToggleDirectiveController', WordFavouriteToggleDirectiveController);

    function WordFavouriteToggleDirective() {
        return {
            restrict: 'E',
            replace: false,
            templateUrl: '/wwwroot/app/shared/wordFavouriteToggleDirective/wordFavouriteToggle.html',
            controller: 'WordFavouriteToggleDirectiveController',
            scope: {
                wordId: '=',
                isFavourite: '='
            }
        };
    }

    WordFavouriteToggleDirectiveController.$inject = ['$scope', 'WordFavouriteService'];
    function WordFavouriteToggleDirectiveController($scope, wordFavouriteService) {
        var controller = $scope;

        controller.inProgress = false;
        controller.isFavourite = $scope.isFavourite;
        controller.toggle = toggle;

        function toggle() {
            controller.inProgress = true;
            wordFavouriteService.submit($scope.wordId, !controller.isFavourite).success(function (response) {
                controller.isFavourite = response.isFavourite;
            }).finally(function () {
                controller.inProgress = false;
            });
        }
    }
})();