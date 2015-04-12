(function () {
    'use strict';

    var appVersion = $('meta[name="appVersion"]').attr('content');
    angular.module('mgApp', ['ngRoute', 'ngMessages', 'toaster'])
        .value('AppVersion', appVersion)
        .config(Config);

    Config.$inject = ['$httpProvider', '$compileProvider'];

    function Config($httpProvider, $compileProvider) {
        $httpProvider.interceptors.push('CustomHttpInterceptor');
        var isDebuggingEnabled = $('meta[name="isDebuggingEnabled"]').attr('content') === 'true';
        $compileProvider.debugInfoEnabled(isDebuggingEnabled);
    }
})();