(function () {
    'use strict';

    angular.module('mgApp').factory('WordService', WordService);

    WordService.$inject = ['$http', '$q'];
    function WordService($http, $q) {
        var url = '/api/word';
        return {
            create: function (wordListId, text, definition) {
                var deferred = $q.defer();
                $http.post(
                    url,
                    {
                        wordListId: wordListId,
                        text: text,
                        definition: definition
                    })
                    .success(function () {
                        deferred.resolve();
                    })
                    .error(function () {
                        deferred.reject();
                    });
                return deferred.promise;
            },
            update: function (wordId, text, definition) {
                var deferred = $q.defer();
                $http.put(
                    url,
                    {
                        wordId: wordId,
                        text: text,
                        definition: definition
                    })
                    .success(function () {
                        deferred.resolve();
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            },
            getAll: function (wordListId) {
                var deferred = $q.defer();
                $http.get(
                    url,
                    {
                        params: {
                            wordListId: wordListId
                        }
                    })
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function () {
                        deferred.reject();
                    });
                return deferred.promise;
            },
            get: function (wordId) {
                var deferred = $q.defer();
                $http.get(
                    url,
                    {
                        params: {
                            wordId: wordId
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