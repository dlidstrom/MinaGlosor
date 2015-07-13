(function () {
    'use strict';

    angular.module('mgApp').directive('spinner', Spinner);

    Spinner.$inject = ['$rootScope', 'usSpinnerService'];

    function Spinner($rootScope, usSpinnerService) {
        return function () {
            $rootScope.$on('events:showSpinner', showSpinner);
            $rootScope.$on('events:hideSpinner', hideSpinner);

            function showSpinner() {
                usSpinnerService.spin('spinner-1');
            }

            function hideSpinner() {
                usSpinnerService.stop('spinner-1');
            }
        }
    }
})();