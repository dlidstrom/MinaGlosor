(function () {
    'use strict';

    angular.module('mgApp').controller('AddWordController', AddWordController);

    AddWordController.$inject = ['WordService', 'WordList'];
    function AddWordController(wordService, wordList) {
        var editor = this;

        editor.wordList = wordList;
        editor.canPractice = wordList.numberOfWords > 0;

        editor.add = add;

        function add(form, entry) {
            wordService.create(wordList.wordListId, entry.text, entry.definition)
                .then(function () {
                    editor.canPractice = true;
                    entry.text = null;
                    entry.definition = null;
                    form.$setPristine();
                });
        };
    }
})();