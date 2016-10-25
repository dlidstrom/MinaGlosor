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
                component: 'progressList',
                params: {
                    page: {
                        value: '1',
                        squash: true
                    }
                },
                resolve: {
                    model: [
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