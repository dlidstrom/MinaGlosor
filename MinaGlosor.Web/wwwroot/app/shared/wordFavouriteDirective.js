(function () {
    'use strict';

    angular.module('mgApp')
        .directive('wordFavourite', WordFavouriteDirective)
        .controller('WordFavouriteController', WordFavouriteController);

    function WordFavouriteDirective() {
        var template
            = '<span style="float: right">'
            + '  <i class="glyphicon glyphicon-star" ng-show="isFavourite" ng-class="{\'mg-spin\': inProgress}" ng-click="toggle()"></i>'
            + '  <i class="glyphicon glyphicon-star-empty" ng-hide="isFavourite" ng-class="{\'mg-spin\': inProgress}" ng-click="toggle()"></i>'
            + '</span>';
        return {
            restrict: 'E',
            replace: true,
            template: template,
            controller: 'WordFavouriteController',
            scope: {
                wordId: "=",
                isFavourite: "="
            }
        };
    }

    WordFavouriteController.$inject = ['$scope', 'WordFavouriteService'];
    function WordFavouriteController($scope, wordFavouriteService) {
        var wordFavourite = $scope;

        wordFavourite.inProgress = false;
        wordFavourite.isFavourite = $scope.isFavourite;
        wordFavourite.toggle = toggle;

        function toggle() {
            wordFavourite.inProgress = true;
            wordFavouriteService.submit($scope.wordId).success(function (response) {
                wordFavourite.isFavourite = response.isFavourite;
            }).finally(function () {
                wordFavourite.inProgress = false;
            });
        }
    }
})();