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
                    update: function (id, text, definition) {
                        var deferred = $q.defer();
                        $http.put(
                            url,
                            {
                                id: id,
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
                    get: function (id) {
                        var deferred = $q.defer();
                        $http.get(
                            url,
                            {
                                params: {
                                    id: id
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