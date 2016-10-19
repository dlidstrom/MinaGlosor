(function () {
    'use strict';

    angular.module('pages.browse', ['ui.router'])
        .config(Config);

    Config.$inject = ['$stateProvider'];
    function Config($stateProvider) {
        $stateProvider.state(
            {
                name: 'browse',
                url: '/browse?page',
                component: 'browseList',
                resolve: {
                    model: [
                        '$stateParams',
                        'BrowseService',
                        function ($stateParams, browseService) {
                            return browseService.search($stateParams.page);
                        }
                    ]
                }
            });
    }
})();