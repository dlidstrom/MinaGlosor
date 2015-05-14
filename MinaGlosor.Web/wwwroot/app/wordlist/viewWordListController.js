(function () {
    'use strict';

    angular.module('mgApp').controller('ViewWordListController', ViewWordListController);

    ViewWordListController.$inject = ['$location', 'WordListId', 'Words'];
    function ViewWordListController($location, wordListId, words) {
        var viewer = this;

        viewer.wordListId = wordListId;
        viewer.wordListName = words.wordListName;
        viewer.words = words.words;
        viewer.canPractice = words.words.length > 0;
        viewer.canAdd = true;
        viewer.returnUrl = $location.url();
    }
})();