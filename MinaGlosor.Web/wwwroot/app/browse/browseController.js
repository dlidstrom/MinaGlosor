(function () {
    'use strict';

    angular.module('mgApp').controller('BrowseController', BrowseController);

    BrowseController.$inject = ['$location', 'BrowseService', 'result'];
    function BrowseController($location, browseService, result) {
        var controller = this;

        controller.wordLists = result.wordLists;
        controller.paging = result.paging;
        controller.pageChanged = pageChanged;

        function pageChanged() {
            $location.path('/browse').search('page', controller.paging.currentPage);
        }
    }
})();