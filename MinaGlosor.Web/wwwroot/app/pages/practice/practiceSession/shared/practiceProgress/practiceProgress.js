(function () {
    'use strict';

    angular.module('mgApp')
        .component(
            'practiceProgress',
            {
                bindings: {
                    green: '@',
                    blue: '@',
                    yellow: '@'
                },
                controller: 'PracticeProgressController',
                templateUrl: '/wwwroot/app/pages/practice/practiceSession/shared/practiceProgress/practiceProgress.html'
            })
        .controller('PracticeProgressController', PracticeProgressController);

    function PracticeProgressController() {
        var $ctrl = this;

        $ctrl.getSuccessStyle = getSuccessStyle;
        $ctrl.getInfoStyle = getInfoStyle;
        $ctrl.getWarningStyle = getWarningStyle;

        function getSuccessStyle() {
            return {
                width: $ctrl.green + '%'
            };
        }

        function getInfoStyle() {
            return {
                width: $ctrl.blue + '%'
            };
        }

        function getWarningStyle() {
            return {
                width: $ctrl.yellow + '%'
            };
        }
    }
})();