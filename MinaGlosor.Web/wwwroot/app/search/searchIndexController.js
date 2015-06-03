(function () {
    'use strict';

    angular.module('mgApp').controller('SearchIndexController', SearchIndexController);

    SearchIndexController.$inject = ['$location', 'q', 'result'];
    function SearchIndexController($location, q, result) {
        var searchIndex = this;

        searchIndex.q = q;
        searchIndex.submit = submit;
        searchIndex.words = result.words;

        function submit(newQ) {
            $location.path('/search').search('q', newQ);
        };
    }
})();