(function () {
    'use strict';

    angular.module('mgApp').controller('EditWordController', EditWordController);

    EditWordController.$inject = ['$location', '$routeParams', 'ReturnUrl', 'WordService', 'Word'];
    function EditWordController($location, $routeParams, returnUrl, wordService, word) {
        var editor = this;

        editor.saving = false;
        editor.entry = word;
        editor.update = update;
        editor.returnUrl = returnUrl;

        function update(entry) {
            editor.saving = true;
            wordService.update(word.wordId, entry.text, entry.definition)
                .then(function () {
                    $location.url(returnUrl);
                }, function () {
                    editor.saving = false;
                });
        };
    }
})();