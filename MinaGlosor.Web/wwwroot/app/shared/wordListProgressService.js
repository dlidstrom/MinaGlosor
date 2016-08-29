(function () {
    'use strict';

    angular.module('mgApp').factory('WordListProgressService', WordListProgressService);

    WordListProgressService.$inject = ['$http', '$q'];
    function WordListProgressService($http, $q) {
        var url = '/api/wordlistprogress';
        return {
            getAll: function () {
                // needs lower-level promise for routes
                var deferred = $q.defer();
                $http.get(url)
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