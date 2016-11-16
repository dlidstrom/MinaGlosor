(function () {
    'use strict';

    angular.module('pages.progress')
        .component(
            'publishedState',
            {
                bindings: {
                    published: '<'
                },
                templateUrl: '/wwwroot/app/pages/progress/progressItem/publishedState/publishedState.html'
            });
})();