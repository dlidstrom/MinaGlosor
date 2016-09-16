(function () {
    'use strict';

    angular.module('mgApp').directive('spinner', Spinner);

    Spinner.$inject = ['$rootScope', '$timeout'];
    function Spinner($rootScope, $timeout) {
        var $spinnerElement = angular.element('#spinner-parent');
        var showPromise;
        var hidePromise;
        return function (scope, element) {
            $rootScope.$on('events:beginRequest', showSpinner);
            $rootScope.$on('events:endRequest', hideSpinner);

            angular.element(document).on('scroll', onScroll);

            function showSpinner() {
                if (showPromise) {
                    return;
                }

                if (hidePromise) {
                    $timeout.cancel(hidePromise);
                    hidePromise = null;
                }

                showPromise = $timeout(function () {
                    $spinnerElement.show();
                    showPromise = null;
                }, 400);
            }

            function hideSpinner() {
                if (showPromise) {
                    $timeout.cancel(showPromise);
                    showPromise = null;
                }

                hidePromise = $timeout(function () {
                    $spinnerElement.hide();
                    hidePromise = null;
                }, 100);
            }

            function onScroll() {
                var scrollTop = angular.element(this).scrollTop();
                var top = 100 + scrollTop;
                var spinnerElement = element.find('.spinner');
                spinnerElement.css({ 'margin-top': top });
            }
        }
    }
})();