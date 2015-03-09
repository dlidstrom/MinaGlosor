(function () {
    'use strict';

    angular.module('mgApp').controller('WordListController', WordListController);

    WordListController.$inject = ['UserWordLists'];
    function WordListController(userWordLists) {
        var wordLists = this;

        wordLists.userWordLists = userWordLists;
    };
})();