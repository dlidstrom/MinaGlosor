(function () {
    'use strict';

    angular.module('mgApp')
        .directive('progressListItem', ProgressListItem)
        .controller('ProgressListItemController', ProgressListItemController);

    function ProgressListItem() {
        return {
            restrict: 'E',
            scope: {
                item: '='
            },
            replace: false,
            templateUrl: '/wwwroot/app/shared/progressListItem/progressListItem.html?v=4',
            controller: 'ProgressListItemController'
        };
    }

    ProgressListItemController.$inject = ['$scope'];
    function ProgressListItemController($scope) {
        $scope.getDoneStyle = getDoneStyle;

        function getDoneStyle(item) {
            return {
                width: item.percentDone + '%',
                'min-width': '3rem'
            };
        }
    }
})();