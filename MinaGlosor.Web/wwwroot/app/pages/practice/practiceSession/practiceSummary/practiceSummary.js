(function () {
    'use strict';

    angular.module('pages.practice')
        .component(
            'practiceSummary', {
                bindings: {
                    model: '<'
                },
                templateUrl: '/wwwroot/app/pages/practice/practiceSession/practiceSummary/practiceSummary.html'
            });
})();