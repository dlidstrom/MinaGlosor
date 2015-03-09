(function () {
    'use strict';

    var appVersion = $('meta[name="appVersion"]').attr('content');
    angular.module('mgApp', ['ngRoute', 'ngMessages', 'toaster']).config(Config).value('AppVersion', appVersion);

    Config.$inject = ['$httpProvider'];

    function Config($httpProvider) {
        $httpProvider.interceptors.push('CustomHttpInterceptor');
    }
})();