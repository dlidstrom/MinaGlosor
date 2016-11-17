(function () {
    'use strict';

    angular.module('pages.progress')
        .component(
            'progressItemPercentages',
            {
                bindings: {
                    item: '<'
                },
                templateUrl: '/wwwroot/app/pages/progress/progressItem/progressItemPercentages/progressItemPercentages.html'
            });
})();