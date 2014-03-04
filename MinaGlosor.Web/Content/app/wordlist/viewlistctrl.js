(function (app) {
    'use strict';

    app.controller(
        'ViewListCtrl',
        [
            '$scope',
            'Words',
            function ($scope, words) {
                $scope.words = words;
            }
        ]);
})(window.App);