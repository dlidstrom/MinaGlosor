(function () {
    'use strict';

    angular.module('mgApp').controller('SearchIndexController', SearchIndexController);

    SearchIndexController.$inject = ['$location', 'SearchService', 'q', 'result'];
    function SearchIndexController($location, searchService, q, result) {
        var searchIndex = this;

        searchIndex.searchService = searchService;
        searchIndex.q = q;
        searchIndex.submit = submit;
        searchIndex.words = result.words;

        function submit(newQ) {
            searchService.search(newQ).then(function (newResult) {
                searchIndex.words = newResult.words;
                $location.search('q', newQ);
            });
        };
    }
})();