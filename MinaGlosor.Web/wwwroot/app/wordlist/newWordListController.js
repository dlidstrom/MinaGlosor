(function () {
    'use strict';

    angular.module('mgApp').controller('NewWordListController', NewWordListController);

    NewWordListController.$inject = ['$location', 'WordListService'];

    function NewWordListController($location, wordListService) {
        var controller = this;
        controller.create = create;

        function create(wordListName) {
            wordListService.create(wordListName)
                .then(function (result) {
                    // success
                    $location.path('/wordlist/' + result.data.wordListId);
                });
        }
    }
})();