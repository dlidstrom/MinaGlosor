(function () {
    'use strict';

    angular.module('mgApp').directive('gravatarImage', GravatarImage);

    function GravatarImage() {
        return {
            restrict: 'E',
            scope: {
                hash: '=',
                owner: '@',
                size: '@'
            },
            replace: false,
            templateUrl: '/wwwroot/app/shared/gravatarImage/gravatarImage.html'
        };
    }
})();