(function (app) {
    'use strict';

    app.controller(
        'WordListCtrl',
        [
            '$scope',
            'WordLists',
            function ($scope, wordLists) {
                $scope.wordLists = wordLists;
            }
        ]);
})(window.App);