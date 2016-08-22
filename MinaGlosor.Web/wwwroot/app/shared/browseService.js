(function () {
    'use strict';

    angular.module('mgApp').factory('BrowseService', BrowseService);

    BrowseService.$inject = ['$http', '$q'];
    function BrowseService($http, $q) {
        var url = '/api/browse';
        return {
            search: function () {
                var deferred = $q.defer();
                $http.get(url)
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