(function () {
    'use strict';

    angular.module('mgApp').factory('WordService', WordService);

    WordService.$inject = ['$http', '$q'];
    function WordService($http, $q) {
        var mainUrl = '/api/word';
        var updateUrl = '/api/updateword';
        return {
            create: function (wordListId, text, definition) {
                var deferred = $q.defer();
                $http.post(
                    mainUrl,
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
                $http.post(
                    updateUrl,
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
                    mainUrl,
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
                    mainUrl,
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