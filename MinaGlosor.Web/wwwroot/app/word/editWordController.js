(function () {
    'use strict';

    angular.module('mgApp').controller('EditWordController', EditWordController);

    EditWordController.$inject = [
        '$location',
        //'toaster',
        //'ErrorHandler',
        'WordService',
        'Word'];

    function EditWordController(/*toaster, errorHandler,*/ $location, wordService, word) {
        var editor = this;

        editor.saving = false;
        editor.entry = word;

        editor.update = update;

        function update(entry) {
            editor.saving = true;
            wordService.update(word.wordId, entry.text, entry.definition)
                .then(function () {
                    // success
                    //toaster.pop('success', 'Ändra ord', 'Uppdateringen genomfördes');
                    $location.path('/wordlist/' + word.wordListId);
                }, function (/*response*/) {
                    // failure
                    editor.saving = false;
                    //errorHandler(response);
                });
        };
    }
})();