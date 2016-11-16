(function () {
    'use strict';

    angular.module('pages.progress')
        .component(
            'progressItem',
            {
                bindings: {
                    item: '<'
                },
                templateUrl: '/wwwroot/app/pages/progress/progressItem/progressItem.html'
            });
})();