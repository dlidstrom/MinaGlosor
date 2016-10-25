(function () {
    'use strict';

    angular.module('pages.search').directive('closeNavbar', CloseNavbar);

    CloseNavbar.$inject = ['$log'];
    function CloseNavbar($log) {
        return {
            restrict: 'A',
            link: function (scope, elem, attrs) {
                angular.element(elem).on('submit', function () {
                    var selector = scope.$eval(attrs.closeNavbar);
                    var target = angular.element(selector);
                    if (!target) {
                        var message = 'target ("'
                            + attrs.closeNavbar
                            + '") element not found: specify target using close-navbar="<target selector>"';
                        $log.error(message);
                    } else {
                        target.collapse('hide');
                    }
                });
            }
        };
    }
})();