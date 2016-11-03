(function () {
    'use strict';

    angular.module('pages.wordlist')
        .component(
            'editWord',
            {
                bindings: {
                    model: '<',
                    returnState: '<'
                },
                controller: 'EditWordController',
                templateUrl: '/wwwroot/app/pages/wordlist/editWord/editWord.html'
            })
        .controller('EditWordController', EditWordController);

    EditWordController.$inject = ['$state', 'WordService'];
    function EditWordController($state, wordService) {
        var $ctrl = this;

        $ctrl.update = update;

        function update() {
            wordService.update($ctrl.model.wordId, $ctrl.model.text, $ctrl.model.definition)
                .then(function () {
                    $state.go($ctrl.returnState.name, $ctrl.returnState.params);
                });
        }
    }
})();