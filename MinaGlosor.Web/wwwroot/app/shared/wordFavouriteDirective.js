(function () {
    'use strict';

    angular.module('mgApp')
        .directive('wordFavourite', WordFavouriteDirective)
        .controller('WordFavouriteController', WordFavouriteController);

    function WordFavouriteDirective() {
        var template
            = '<span class="favourite" style="float: right">'
            + '  <i class="glyphicon glyphicon-star" ng-class="{\'mg-spin\': inProgress, \'glyphicon-star\': isFavourite, \'glyphicon-star-empty\': !isFavourite,  yes: isFavourite, no: !isFavourite}" ng-click="toggle()"></i>'
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