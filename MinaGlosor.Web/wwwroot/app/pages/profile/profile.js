(function () {
    'use strict';

    angular.module('pages.profile', ['ui.router'])
        .config(Config);

    Config.$inject = ['$stateProvider'];
    function Config($stateProvider) {
        $stateProvider.state(
        {
            name: 'profile',
            url: '/',
            template: '<h1>profile</h1>'
        });
    }
})();