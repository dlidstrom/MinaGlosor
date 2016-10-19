(function () {
    'use strict';

    angular.module('pages.progress').factory('ProgressService', ProgressService);

    ProgressService.$inject = ['$http', '$q'];
    function ProgressService($http, $q) {
        var url = '/api/progress';
        return {
            getAll: function (page) {
                // needs lower-level promise for routes TODO Still necessary?
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