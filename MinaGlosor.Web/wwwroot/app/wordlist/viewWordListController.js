(function () {
    'use strict';

    angular.module('mgApp').controller('ViewWordListController', ViewWordListController);

    ViewWordListController.$inject = ['WordListId', 'Words'];
    function ViewWordListController(wordListId, words) {
        var viewer = this;

        viewer.wordListId = wordListId;
        viewer.wordListName = words.wordListName;
        viewer.words = words.words;
    }
})();