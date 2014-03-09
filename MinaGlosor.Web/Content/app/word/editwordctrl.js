(function (app) {
    'use strict';

    app.controller(
        'EditWordCtrl',
        [
            '$scope',
            '$location',
            'toaster',
            'ErrorHandler',
            'WordService',
            'Word',
            function ($scope, $location, toaster, errorHandler, wordService, word) {
                $scope.entry = word;

                $scope.update = function (entry) {
                    $scope.saving = true;
                    wordService.update(word.id, entry.text, entry.definition)
                        .then(function () {
                            // success
                            toaster.pop('success', 'Ändra ord', 'Uppdateringen genomfördes');
                            $location.path('/wordlist/' + word.wordListId);
                        }, function (response) {
                            // failure
                            $scope.saving = false;
                            errorHandler(response);
                        });
                };
            }
        ]);
})(window.App);