(function () {
    'use strict';

    angular.module('pages.progress')
        .component(
            'progressItem',
            {
                bindings: {
                    item: '<'
                },
                controller: 'ProgressItemController',
                templateUrl: '/wwwroot/app/pages/progress/progressItem/progressItem.html'
            })
        .controller('ProgressItemController', ProgressItemController);

    function ProgressItemController() {
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