(function () {
    'use strict';

    angular.module('pages.progress')
        .component(
            'progressItemProgress',
            {
                bindings: {
                    item: '<'
                },
                controller: 'ProgressItemProgressController',
                templateUrl: '/wwwroot/app/pages/progress/progressItem/progressItemProgress/progressItemProgress.html'
            })
        .controller('ProgressItemProgressController', ProgressItemProgressController);

    function ProgressItemProgressController() {
        var $ctrl = this;

        $ctrl.getDoneStyle = getDoneStyle;
        $ctrl.getExpiredStyle = getExpiredStyle;

        function getDoneStyle(item) {
            return {
                width: (item.percentDone - item.percentExpired) + '%',
                'min-width': '3rem'
            };
        }

        function getExpiredStyle(item) {
            return {
                width: item.percentExpired + '%',
                'min-width': '3rem'
            };
        }
    }
})();