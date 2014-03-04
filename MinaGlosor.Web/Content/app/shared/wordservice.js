(function (app) {
    'use strict';

    app.factory(
        'WordService',
        [
            '$http',
            '$q',
            function ($http, $q) {
                var url = '/api/word';
                return {
                    create: function (wordListId, word, definition) {
                        var deferred = $q.defer();
                        $http.post(
                                url,
                                {
                                    wordListId: wordListId,
                                    word: word,
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
                    }
                };
            }
        ]);
})(window.App);