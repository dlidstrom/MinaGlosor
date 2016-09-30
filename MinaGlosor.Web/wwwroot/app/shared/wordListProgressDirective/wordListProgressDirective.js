(function () {
    'use strict';

    angular.module('mgApp').directive('wordListProgress', WordListProgressDirective);

    function WordListProgressDirective() {
        return {
            restrict: 'E',
            scope: {
                item: '='
            },
            replace: false,
            templateUrl: '/wwwroot/app/shared/wordListProgressDirective/template.html'
        };
    }
})();