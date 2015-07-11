(function () {
    'use strict';

    angular.module('mgApp').controller('SearchIndexController', SearchIndexController);

    SearchIndexController.$inject = ['$location', '$sce', 'SearchService', 'q', 'result'];
    function SearchIndexController($location, $sce, searchService, q, result) {
        var searchIndex = this;

        searchIndex.searchService = searchService;
        searchIndex.q = q;
        searchIndex.submit = submit;
        searchIndex.words = trustHtml(result.words);
        searchIndex.returnUrl = encodeURIComponent($location.url());

        function submit(newQ) {
            searchService.search(newQ).then(function (newResult) {
                searchIndex.words = trustHtml(newResult.words);
                $location.search('q', newQ);
                searchIndex.returnUrl = encodeURIComponent($location.url());
            });
        };

        function trustHtml(words) {
            var trusted = words.map(function (w) {
                var text = $sce.trustAsHtml(w.text);
                var definition = $sce.trustAsHtml(w.definition);
                var wordId = w.wordId;
                return {
                    text: text,
                    definition: definition,
                    wordId: wordId
                };
            });

            return trusted;
        }
    }
})();