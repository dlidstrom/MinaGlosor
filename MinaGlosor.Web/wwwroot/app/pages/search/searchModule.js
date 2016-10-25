(function () {
    'use strict';

    angular.module('pages.search', ['ui.router'])
        .config(Config);

    Config.$inject = ['$stateProvider'];
    function Config($stateProvider) {
        $stateProvider.state(
            {
                name: 'search',
                url: '/search?q',
                component: 'searchList',
                params: {
                    q: {
                        value: '',
                        squash: true
                    }
                },
                reloadOnSearch: false,
                resolve: {
                    model: [
                        '$stateParams',
                        'SearchService',
                        function ($stateParams, searchService) {
                            return searchService.search($stateParams.q);
                        }
                    ]
                }
            });
    }
})();