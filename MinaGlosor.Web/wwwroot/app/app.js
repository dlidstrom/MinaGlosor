(function () {
    'use strict';

    var appVersion = $('meta[name="appVersion"]').attr('content');
    var deps = [
        'ngRoute',
        'ngMessages',
        'ngSanitize',
        'toaster',
        'ui.bootstrap',
        'xeditable',
        'ui.router',
        'pages.profile',
        'pages.test'];
    angular.module('mgApp', deps)
        .value('AppVersion', appVersion)
        .config(Config)
        .run(Run);

    Config.$inject = ['$httpProvider', '$compileProvider'];
    function Config($httpProvider, $compileProvider) {
        $httpProvider.interceptors.push('CustomHttpInterceptor');
        var isDebuggingEnabled = $('meta[name="isDebuggingEnabled"]').attr('content') === 'true';
        $compileProvider.debugInfoEnabled(isDebuggingEnabled);
    }

    Run.$inject = ['$rootScope', '$state', 'editableOptions'];
    function Run($rootScope, $state, editableOptions) {
        editableOptions.theme = 'bs3';
        $rootScope.$on('$stateChangeError', console.log.bind(console));
        //$trace.enable('TRANSITION');
        $rootScope.$on('$stateChangeStart', function () {
            console.log('$stateChangeStart');
        });
        $rootScope.$on('$stateChangeSuccess', function () {
            console.log('$stateChangeSuccess');
        });
        $rootScope.$on('$stateChangeError', function () {
            console.log('$stateChangeError');
        });
    }
})();