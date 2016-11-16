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

        function getDoneStyle(item) {
            return {
                width: item.percentDone + '%',
                'min-width': '3rem'
            };
        }
    }
})();