(function () {
    'use strict';

    angular.module('mgApp').factory('PracticeWordService', PracticeWordService);

    PracticeWordService.$inject = ['$http', '$q'];
    function PracticeWordService($http, $q) {
        var service = {
            getNext: function (practiceSessionId) {
                var deferred = $q.defer();
                $http.get('/api/practiceword', { params: { practiceSessionId: practiceSessionId } })
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            },
            getById: function (practiceSessionId, practiceWordId) {
                var deferred = $q.defer();
                $http.get('/api/practiceword', { params: { practiceSessionId: practiceSessionId, practiceWordId: practiceWordId } })
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            },
            submit: function (practiceWord, confidenceLevel) {
                var deferred = $q.defer();
                $http.post(
                    '/api/wordconfidence',
                    {
                        practiceSessionId: practiceWord.practiceSessionId,
                        practiceWordId: practiceWord.practiceWordId,
                        confidenceLevel: confidenceLevel
                    }).success(function (data) {
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