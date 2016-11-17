(function () {
    'use strict';

    angular.module('pages.progress')
        .component(
            'progressItemDescription',
            {
                bindings: {
                    item: '<'
                },
                templateUrl: '/wwwroot/app/pages/progress/progressItem/progressItemDescription/progressItemDescription.html'
            });
})();