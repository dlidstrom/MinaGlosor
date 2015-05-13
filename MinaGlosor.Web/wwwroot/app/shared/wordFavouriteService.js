(function () {
    'use strict';

    angular.module('mgApp').factory('WordFavouriteService', WordFavouriteService);

    WordFavouriteService.$inject = ['$http', '$q'];
    function WordFavouriteService($http, $q) {
        var url = '/api/wordfavourite';
        var service = {
            submit: function (wordId) {
                return $http.post(
                    url,
                    {
                        wordId: wordId
                    });
            },
            getAll: function () {
                var deferred = $q.defer();
                $http.get(url)
                    .success(function (data) {
                        deferred.resolve(data);
                    }).error(function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            }
        };

        return service;
    }
})();