(function () {
    'use strict';

    angular.module('mgApp')
        .directive('progressFavourites', ProgressFavourites);

    function ProgressFavourites() {
        return {
            restrict: 'E',
            replace: false,
            templateUrl: '/wwwroot/app/pages/progress/progressFavourites/progressFavourites.html',
            scope: {
                numberOfFavourites: '='
            }
        };
    }
})();