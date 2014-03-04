(function (app) {
    'use strict';

    app.controller(
        'NewCtrl',
        [
            '$scope',
            '$location',
            'WordListService',
            function ($scope, $location, wordListService) {
                $scope.create = function (wordListName) {
                    wordListService.create(wordListName)
                        .then(function () {
                            // success
                            $location.path('/wordlist');
                        }, function (response) {
                            // failure
                        });
                };
            }
        ]);
})(window.App);