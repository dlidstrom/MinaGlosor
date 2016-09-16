(function () {
    'use strict';

    angular.module('mgApp')
        .directive('wordFavourite', WordFavouriteDirective)
        .controller('WordFavouriteDirectiveController', WordFavouriteDirectiveController);

    function WordFavouriteDirective() {
        var template
            = '<span class="favourite">'
            + '  <i class="glyphicon glyphicon-star action" ng-class="{\'mg-spin\': inProgress, \'glyphicon-star\': isFavourite, \'glyphicon-star-empty\': !isFavourite,  yes: isFavourite, no: !isFavourite}" ng-click="toggle()"></i>'
            + '</span>';
        return {
            restrict: 'E',
            replace: true,
            template: template,
            controller: 'WordFavouriteDirectiveController',
            scope: {
                wordId: '=',
                isFavourite: '='
            }
        };
    }

    WordFavouriteDirectiveController.$inject = ['$scope', 'WordFavouriteService'];
    function WordFavouriteDirectiveController($scope, wordFavouriteService) {
        var wordFavourite = $scope;

        wordFavourite.inProgress = false;
        wordFavourite.isFavourite = $scope.isFavourite;
        wordFavourite.toggle = toggle;

        function toggle() {
            wordFavourite.inProgress = true;
            wordFavouriteService.submit($scope.wordId, !wordFavourite.isFavourite).success(function (response) {
                wordFavourite.isFavourite = response.isFavourite;
            }).finally(function () {
                wordFavourite.inProgress = false;
            });
        }
    }
})();