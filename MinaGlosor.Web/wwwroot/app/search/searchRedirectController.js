(function () {
    'use strict';

    angular.module('mgApp').controller('SearchRedirectController', SearchRedirectController);

    SearchRedirectController.$inject = ['$location'];
    function SearchRedirectController($location) {
        var search = this;

        search.submit = submit;

        function submit(q) {
            $location.path('/search').search('q', q);
        };
    }
})();