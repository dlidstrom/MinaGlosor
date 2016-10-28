(function () {
    'use strict';

    angular.module('pages.wordlist')
        .component(
            'addWord',
            {
                bindings: {
                    model: '<'
                },
                controller: 'AddWordController',
                templateUrl: '/wwwroot/app/pages/wordlist/addWord/addWord.html'
            })
        .controller('AddWordController', AddWordController);

    AddWordController.$inject = ['WordService'];
    function AddWordController(wordService) {
        var $ctrl = this;

        $ctrl.add = add;

        function add(form, entry) {
            wordService.create($ctrl.model.wordListId, entry.text, entry.definition)
                .then(function () {
                    $ctrl.model.canPractice = true;
                    entry.text = null;
                    entry.definition = null;
                    form.$setPristine();
                });
        };
    }
})();