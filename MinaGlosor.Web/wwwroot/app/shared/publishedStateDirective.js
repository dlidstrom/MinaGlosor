(function () {
    'use strict';

    angular.module('mgApp').directive('publishedState', PublishedState);

    function PublishedState() {
        return {
            restrict: 'E',
            scope: {
                published: '@'
            },
            replace: false,
            template:
                    '<div ng-hide="published">'
                    + '<span class="label label-default">'
                    + '<i class="glyphicon glyphicon-eye-close"></i>PRIVAT</span>'
                    + '<span>bara du kan se den här ordlistan</span>'
                    + '</div>'
                    + '<div ng-show="published">'
                    + '<span class="label label-default">'
                    + '<i class="glyphicon glyphicon-eye-open"></i>'
                    + 'PUBLICERAD</span>'
                    + '<span>andra medlemmar kan se den här ordlistan.'
                    + '</span>'
                    + '</div>'
        };
    }
})();