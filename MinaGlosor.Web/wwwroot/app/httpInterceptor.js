(function () {
    'use strict';

    angular.module('mgApp').factory('CustomHttpInterceptor', HttpInterceptor);

    HttpInterceptor.$inject = ['$q', '$rootScope', '$injector', 'toaster', 'AppVersion'];
    function HttpInterceptor($q, $rootScope, $injector, toaster, appVersion) {
        var needUpgradeWarned = false;
        var interceptor = {
            request: function (config) {
                $rootScope.$emit('events:showSpinner');
                config.url += '?' + 'v=' + appVersion;
                return config;
            },
            response: function (response) {
                var $http = $injector.get('$http');
                if ($http.pendingRequests.length === 0) {
                    $rootScope.$emit('events:hideSpinner');
                }

                return response || $q.when(response);
            },
            responseError: function (rejection) {
                var $http = $injector.get('$http');
                if ($http.pendingRequests.length === 0) {
                    $rootScope.$emit('events:hideSpinner');
                }

                if (rejection.status === 426 && !needUpgradeWarned) {
                    toaster.pop('error', 'Uppdatering krävs', rejection.data.message, 30000);
                    needUpgradeWarned = true;
                    $rootScope.upgradeRequired = true;
                }

                return $q.reject(rejection);
            }
        };
        return interceptor;
    }
})();