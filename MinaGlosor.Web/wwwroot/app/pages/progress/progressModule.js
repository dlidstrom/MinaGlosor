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
                controller: 'ProgressController',
                controllerAs: 'controller',
                templateUrl: '/wwwroot/app/pages/progress/progress.html?v=2',
                resolve: {
                    Result: [
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