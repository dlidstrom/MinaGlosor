(function () {
    'use strict';

    angular.module('pages.practice').factory('PracticeWordService', PracticeWordService);

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
            submit: function (practiceSessionId, practiceWordId, confidenceLevel) {
                var deferred = $q.defer();
                $http.post(
                    '/api/wordconfidence',
                    {
                        practiceSessionId: practiceSessionId,
                        practiceWordId: practiceWordId,
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