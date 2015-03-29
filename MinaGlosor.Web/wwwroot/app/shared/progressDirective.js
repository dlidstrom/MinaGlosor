(function () {
    'use strict';

    angular.module('mgApp').directive('progress', Progress);

    function Progress() {
        return {
            restrict: 'E',
            scope: {
                green: "@green",
                yellow: "@yellow",
                done: "@done"
            },
            replace: true,
            template: '<div class="progress progress-flat">'
                + '<div class="progress-bar progress-bar-success" style="{{\'width: \' + green + \'%\'}}"></div>'
                + '<div class="progress-bar progress-bar-warning" style="{{\'width: \' + yellow + \'%\'}}"></div>'
                + '<span class="progress-bar-label">{{done + \'%\'}}</span>'
                + '</div>'
        };
    }
})();