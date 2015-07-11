(function () {
    'use strict';

    angular.module('mgApp').directive('closeNavbar', CloseNavbar);

    function CloseNavbar() {
        return {
            restrict: 'A',
            link: function (scope, elem) {
                angular.element(elem).on('submit', function () {
                    angular.element('#navbar-collapse').collapse('hide');
                });
            }
        };
    }
})();