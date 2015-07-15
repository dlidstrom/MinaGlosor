(function () {
    'use strict';

    angular.module('mgApp').directive('spinner', Spinner);

    Spinner.$inject = ['$rootScope', '$timeout'];

    function Spinner($rootScope, $timeout) {
        var $spinnerElement = angular.element('#spinner-parent');
        var timeoutPromise;
        return function () {
            $rootScope.$on('events:showSpinner', showSpinner);
            $rootScope.$on('events:hideSpinner', hideSpinner);

            function showSpinner() {
                if (timeoutPromise) return;
                timeoutPromise = $timeout(function () { $spinnerElement.show(); }, 400);
            }

            function hideSpinner() {
                $spinnerElement.hide();
                if (timeoutPromise) {
                    $timeout.cancel(timeoutPromise);
                    timeoutPromise = null;
                }
            }
        }
    }
})();