(function () {
    'use strict';

    angular.module('mgApp').factory('CustomHttpInterceptor', HttpInterceptor);

    HttpInterceptor.$inject = ['$q', '$rootScope', 'toaster', 'AppVersion'];
    function HttpInterceptor($q, $rootScope, toaster, appVersion) {
        var needUpgradeWarned = false;
        var interceptor = {
            request: function (config) {
                config.url += '?' + 'v=' + appVersion;
                return config;
            },
            responseError: function (rejection) {
                if (rejection.status == 426 && !needUpgradeWarned) {
                    toaster.pop('error', "Uppdatering krävs", rejection.data.message, 30000);
                    needUpgradeWarned = true;
                    $rootScope.upgradeRequired = true;
                }

                return $q.reject(rejection);
            }
        };
        return interceptor;
    }
})();