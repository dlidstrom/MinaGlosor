(function (app) {
    'use strict';

    app.controller(
        'AddWordCtrl',
        [
            '$scope',
            'toaster',
            'ErrorHandler',
            'WordService',
            'WordList',
            function ($scope, toaster, errorHandler, wordService, wordList) {
                $scope.wordList = wordList;

                $scope.add = function (entry) {
                    $scope.saving = true;
                    wordService.create(wordList.id, entry.word, entry.definition)
                        .then(function () {
                            // success
                            toaster.pop('success', 'Nytt ord', 'Ordet sparades i ' + wordList.name);
                            $scope.saving = false;
                            $scope.entry = null;
                            $scope.form.$setPristine();
                        }, function (response) {
                            // failure
                            $scope.saving = false;
                            errorHandler(response);
                        });
                };
            }
        ]);
})(window.App);