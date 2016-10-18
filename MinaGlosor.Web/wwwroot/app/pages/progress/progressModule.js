(function () {
    'use strict';

    angular.module('pages.progress', ['ui.router'])
        .config(Config);

    Config.$inject = ['$stateProvider'];
    function Config($stateProvider) {
        $stateProvider.state(
            {
                name: 'progress',
                url: '/progress?page',
                component: 'progressListComponent',
                resolve: {
                    progressResult: [
                        '$stateParams',
                        'ProgressService',
                        function ($stateParams, progressService) {
                            return progressService.getAll($stateParams.page);
                        }
                    ]
                }
            });
    }
})();