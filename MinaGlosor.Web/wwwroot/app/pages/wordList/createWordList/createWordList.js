(function () {
    'use strict';

    angular.module('pages.wordlist')
        .component(
            'createWordList',
            {
                controller: 'CreateWordListController',
                templateUrl: '/wwwroot/app/pages/wordlist/createWordList/createWordList.html'
            })
        .controller('CreateWordListController', CreateWordListController);

    CreateWordListController.$inject = ['$state', 'WordListService'];
    function CreateWordListController($state, wordListService) {
        var $ctrl = this;

        $ctrl.create = create;

        function create(wordListName) {
            wordListService.create(wordListName)
                .then(function (result) {
                    $state.go('wordlist', { wordListId: result.data.wordListId });
                });
        }
    }
})();