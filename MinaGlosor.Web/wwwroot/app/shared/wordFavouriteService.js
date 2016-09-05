﻿(function () {
    'use strict';

    angular.module('mgApp').factory('WordFavouriteService', WordFavouriteService);

    WordFavouriteService.$inject = ['$http', '$q'];
    function WordFavouriteService($http, $q) {
        var url = '/api/wordfavourite';
        var service = {
            submit: function (wordId, isFavourite) {
                return $http.post(
                    url,
                    {
                        wordId: wordId,
                        isFavourite: isFavourite
                    });
            },
            getAll: function (page) {
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
                    }).error(function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            }
        };

        return service;
    }
})();