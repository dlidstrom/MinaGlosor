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
        'pages.browse',
        'pages.practice',
        'pages.progress',
        'pages.search'
    ];
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

    Run.$inject = ['$rootScope', '$state', '$log', 'editableOptions'];
    function Run($rootScope, $state, $log, editableOptions) {
        editableOptions.theme = 'bs3';
        $rootScope.$on('$stateChangeError', function (err) {
            $log.error(err);
        });
        $rootScope.$on('$stateChangeStart', function () {
            $log.info('$stateChangeStart');
        });
        $rootScope.$on('$stateChangeSuccess', function () {
            $log.info('$stateChangeSuccess');
        });
        $rootScope.$on('$stateChangeError', function () {
            $log.info('$stateChangeError');
        });
    }
})();