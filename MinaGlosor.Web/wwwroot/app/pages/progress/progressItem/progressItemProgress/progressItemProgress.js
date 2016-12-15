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

        $ctrl.shouldShowDone = shouldShowDone;
        $ctrl.getDoneStyle = getDoneStyle;
        $ctrl.getExpiredStyle = getExpiredStyle;
        $ctrl.getPercentDoneForDisplay = getPercentDoneForDisplay;

        function shouldShowDone(item) {
            var result = item.percentDone - item.percentExpired > 0;
            return result;
        }

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

        function getPercentDoneForDisplay(item) {
            return item.percentDone - item.percentExpired;
        }
    }
})();