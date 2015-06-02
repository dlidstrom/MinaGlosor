(function () {
    'use strict';

    angular.module('mgApp').controller('SearchIndexController', SearchIndexController);

    SearchIndexController.$inject = ['$location', 'q'];
    function SearchIndexController($location) {
        var searchIndex = this;

        searchIndex.submit = submit;

        function submit(newQ) {
            $location.path('/search').search('q', newQ);
        };
    }
})();