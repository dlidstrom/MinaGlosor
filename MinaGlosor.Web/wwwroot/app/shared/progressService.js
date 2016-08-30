(function () {
    'use strict';

    angular.module('mgApp').factory('ProgressService', ProgressService);

    ProgressService.$inject = ['$http', '$q'];
    function ProgressService($http, $q) {
        var url = '/api/progress';
        return {
            getAll: function (page) {
                // needs lower-level promise for routes
                var deferred = $q.defer();
                $http.get(
                    url,
                    {
                        params: {
                            page: page || 1
                        }
                    })
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            }
        };
    }
})();