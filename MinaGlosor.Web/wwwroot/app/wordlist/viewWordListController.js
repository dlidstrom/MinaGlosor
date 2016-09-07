(function () {
    'use strict';

    angular.module('mgApp').controller('ViewWordListController', ViewWordListController);

    ViewWordListController.$inject = ['$location', 'wordListId', 'result'];
    function ViewWordListController($location, wordListId, result) {
        var viewer = this;

        viewer.wordListId = wordListId;
        viewer.wordListName = result.wordListName;
        viewer.words = result.words;
        viewer.canPractice = result.words.length > 0;
        viewer.canAdd = result.canAdd;
        viewer.canEdit = result.canEdit;
        viewer.paging = result.paging;
        viewer.returnUrl = $location.url();
    }
})();