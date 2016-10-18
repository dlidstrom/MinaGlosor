(function () {
    'use strict';

    angular.module('pages.progress')
        .component(
            'progressFavourites',
            {
                bindings: {
                    numberOfFavourites: '<'
                },
                templateUrl: '/wwwroot/app/pages/progress/progressFavourites/progressFavourites.html'
            });
})();