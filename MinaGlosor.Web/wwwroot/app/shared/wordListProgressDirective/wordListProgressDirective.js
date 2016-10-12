(function () {
    'use strict';

    angular.module('mgApp')
        .directive('wordListProgress', WordListProgressDirective)
        .controller('WordListProgressDirectiveController', WordListProgressDirectiveController);

    function WordListProgressDirective() {
        return {
            restrict: 'E',
            scope: {
                item: '='
            },
            replace: false,
            templateUrl: '/wwwroot/app/shared/wordListProgressDirective/wordListProgress.html?v=3',
            controller: 'WordListProgressDirectiveController'
        };
    }

    WordListProgressDirectiveController.$inject = ['$scope'];
    function WordListProgressDirectiveController($scope) {
        $scope.getDoneStyle = getDoneStyle;

        function getDoneStyle(item) {
            return {
                width: item.percentDone + '%',
                'min-width': '3rem'
            };
        }
    }
})();