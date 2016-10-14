(function () {
    'use strict';

    angular.module('mgApp')
        .directive('wordListFavourites', WordListFavourites);

    function WordListFavourites() {
        return {
            restrict: 'E',
            replace: false,
            templateUrl: '/wwwroot/app/shared/wordListFavouritesDirective/wordListFavourites.html',
            scope: {
                numberOfFavourites: '='
            }
        };
    }
})();