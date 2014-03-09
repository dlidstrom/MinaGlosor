(function (app) {
    'use strict';

    app.controller(
        'ViewListCtrl',
        [
            '$scope',
            '$routeParams',
            'Words',
            function ($scope, $routeParams, words) {
                $scope.words = words;
                $scope.wordListId = $routeParams.id;
            }
        ]);
})(window.App);