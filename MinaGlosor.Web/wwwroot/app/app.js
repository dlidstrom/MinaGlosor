(function () {
    'use strict';

    var appVersion = $('meta[name="appVersion"]').attr('content');
    var deps = [
        'ngRoute',
        'ngMessages',
        'ngSanitize',
        'toaster',
        'ui.bootstrap',
        'xeditable'];
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

    Run.$inject = ['editableOptions'];
    function Run(editableOptions) {
        editableOptions.theme = 'bs3';
    }
})();