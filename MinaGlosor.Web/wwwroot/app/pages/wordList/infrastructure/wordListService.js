(function () {
    'use strict';

    angular.module('mgApp').factory('WordListService', WordListService);

    WordListService.$inject = ['$http', '$q'];
    function WordListService($http, $q) {
        var url = '/api/wordlist';
        var updateWordListNameUrl = '/api/updatewordlistname';
        var publishUrl = '/api/publishwordlist';
        return {
            getById: function (wordListId) {
                // needs lower-level promise for routes
                var deferred = $q.defer();
                $http.get(url, { params: { wordListId: wordListId } })
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            },
            create: function (name) {
                var promise = $http.post(
                    url,
                    {
                        name: name
                    });
                return promise;
            },
            updateName: function (wordListId, wordListName) {
                var promise = $http.post(
                    updateWordListNameUrl,
                    {
                        wordListId: wordListId,
                        wordListName: wordListName
                    });
                return promise;
            },
            publish: function (wordListId, published) {
                var promise = $http.post(
                    publishUrl,
                    {
                        wordListId: wordListId,
                        publish: published
                    });
                return promise;
            }
        };
    }
})();