(function () {
    'use strict';

    angular.module('mgApp')
        .directive('practiceProgress', PracticeProgress)
        .controller('PracticeProgressController', PracticeProgressController);

    function PracticeProgress() {
        return {
            restrict: 'E',
            // TODO Fix these attribute names
            scope: {
                green: '@',
                blue: '@',
                yellow: '@'
            },
            replace: false,
            templateUrl: '/wwwroot/app/shared/practiceProgress/practiceProgress.html',
            controller: 'PracticeProgressController'
        };
    }

    PracticeProgressController.$inject = ['$scope'];
    function PracticeProgressController($scope) {
        var controller = $scope;

        controller.getSuccessStyle = getSuccessStyle;
        controller.getInfoStyle = getInfoStyle;
        controller.getWarningStyle = getWarningStyle;

        function getSuccessStyle() {
            return {
                width: controller.green + '%'
            };
        }

        function getInfoStyle() {
            return {
                width: controller.blue + '%'
            };
        }

        function getWarningStyle() {
            return {
                width: controller.yellow + '%'
            };
        }
    }
})();