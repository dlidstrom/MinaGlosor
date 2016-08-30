(function () {
    'use strict';

    angular.module('mgApp').controller('ProgressController', ProgressController);

    ProgressController.$inject = ['$location', 'result'];
    function ProgressController($location, result) {
        var wordLists = this;

        wordLists.userWordLists = result.wordLists;
        wordLists.numberOfFavourites = result.numberOfFavourites;
        controller.paging = result.paging;
        controller.pageChanged = pageChanged;

        function pageChanged() {
            $location.path('/progress').search('page', controller.paging.currentPage);
        }
    };
})();