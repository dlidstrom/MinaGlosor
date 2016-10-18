(function () {
    'use strict';

    angular.module('pages.test', ['ui.router'])
        .config(Config);

    Config.$inject = ['$stateProvider'];
    function Config($stateProvider) {
        $stateProvider.state(
        {
            name: 'test',
            url: '/test',
            template: '<h1>test</h1>'
        });
    }
})();