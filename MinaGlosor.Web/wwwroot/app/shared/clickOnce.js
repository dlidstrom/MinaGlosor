(function () {
    'use strict';
    angular.module('mgApp').directive('clickOnce', ClickOnce);
    ClickOnce.$inject = ['$timeout'];
    function ClickOnce($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var replacementText = attrs.clickOnce;

                element.bind('click', function () {
                    $timeout(function () {
                        if (replacementText) {
                            element.html(replacementText);
                        }
                        element.attr('disabled', true);
                    });
                });
            }
        };
    }
})();