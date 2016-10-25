(function () {
    'use strict';

    angular.module('pages.search').factory('SearchService', SearchService);

    SearchService.$inject = ['$http', '$q'];
    function SearchService($http, $q) {
        var url = '/api/search2';
        return {
            search: function (q) {
                var deferred = $q.defer();
                $http.get(
                    url,
                    {
                        params: {
                            q: q
                        }
                    })
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function () {
                        deferred.reject();
                    });
                return deferred.promise;
            }
        };
    }
})();