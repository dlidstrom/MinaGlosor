(function () {
    'use strict';

    angular.module('mgApp').controller('AddWordController', AddWordController);

    AddWordController.$inject = [/*'toaster', 'ErrorHandler',*/'WordService', 'WordList'];
    function AddWordController(/*toaster, errorHandler,*/ wordService, wordList) {
        var editor = this;

        editor.wordList = wordList;
        editor.canPractice = wordList.numberOfWords > 0;

        editor.add = add;

        function add(form, entry) {
            editor.saving = true;
            wordService.create(wordList.wordListId, entry.text, entry.definition)
                .then(function () {
                    // success
                    //toaster.pop('success', 'Nytt ord', 'Ordet sparades i ' + wordList.name);
                    editor.saving = false;
                    editor.canPractice = true;
                    entry.text = null;
                    entry.definition = null;
                    form.$setPristine();
                }, function (/*response*/) {
                    // failure
                    editor.saving = false;
                    //errorHandler(response);
                });
        };
    }
})();