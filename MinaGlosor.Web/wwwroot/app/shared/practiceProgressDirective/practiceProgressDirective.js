(function () {
    'use strict';

    angular.module('mgApp').directive('practiceProgress', PracticeProgress);

    function PracticeProgress() {
        return {
            restrict: 'E',
            scope: {
                green: '@',
                blue: '@',
                yellow: '@'
            },
            replace: true,
            template: '<div class="progress practice progress-flat">'
                + '<div class="progress-bar progress-bar-success" style="{{\'width: \' + green + \'%\'}}"></div>'
                + '<div class="progress-bar progress-bar-info" style="{{\'width: \' + blue + \'%\'}}"></div>'
                + '<div class="progress-bar progress-bar-warning" style="{{\'width: \' + yellow + \'%\'}}"></div>'
                + '</div>'
        };
    }
})();