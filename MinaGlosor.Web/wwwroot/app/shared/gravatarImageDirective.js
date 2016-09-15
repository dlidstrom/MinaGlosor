(function () {
    'use strict';

    angular.module('mgApp').directive('gravatarImage', GravatarImage);

    function GravatarImage() {
        return {
            restrict: 'E',
            scope: {
                hash: '=',
                owner: '=',
                size: '@'
            },
            replace: false,
            template:
                    '<img src="https://www.gravatar.com/avatar/{{hash}}?s={{size}}"'
                    + ' alt="Skapad av {{owner}}'
                    + ' width="{{size}}"'
                    + ' height="{{size}}" />'
        };
    }
})();