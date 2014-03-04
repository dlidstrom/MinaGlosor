(function (app) {
    'use strict';

    app.factory(
        'WordListService',
        [
            '$http',
            '$q',
            function ($http, $q) {
                var url = '/api/wordlist';
                return {
                    getAll: function () {
                        var deferred = $q.defer();
                        $http.get(url)
                            .success(function (data) {
                                deferred.resolve(data);
                            })
                            .error(function (response) {
                                deferred.reject(response);
                            });
                        return deferred.promise;
                    },
                    getById: function (id) {
                        var deferred = $q.defer();
                        $http.get(
                                url,
                                {
                                    params: { id: id }
                                })
                            .success(function (data) {
                                deferred.resolve(data);
                            })
                            .error(function (response) {
                                deferred.reject(response);
                            });
                        return deferred.promise;
                    },
                    create: function (wordListName) {
                        return $http.post(
                            url,
                            {
                                wordListName: wordListName
                            });
                    }
                };
            }
        ]);
})(window.App);