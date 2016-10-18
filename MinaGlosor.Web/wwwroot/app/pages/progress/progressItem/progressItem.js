(function () {
    'use strict';

    angular.module('mgApp')
        .directive('progressItem', ProgressListItem)
        .controller('ProgressItemController', ProgressItemController);

    function ProgressListItem() {
        return {
            restrict: 'E',
            scope: {
                item: '='
            },
            replace: false,
            templateUrl: '/wwwroot/app/pages/progress/progressItem/progressItem.html',
            controller: 'ProgressItemController'
        };
    }

    ProgressItemController.$inject = ['$scope'];
    function ProgressItemController($scope) {
        $scope.getDoneStyle = getDoneStyle;

        function getDoneStyle(item) {
            return {
                width: item.percentDone + '%',
                'min-width': '3rem'
            };
        }
    }
})();