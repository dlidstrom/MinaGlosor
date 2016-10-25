(function () {
    'use strict';

    angular.module('pages.practice').factory('PracticeService', PracticeService);

    PracticeService.$inject = ['$http', '$q'];
    function PracticeService($http, $q) {
        var service = {
            create: function (wordListId) {
                var deferred = $q.defer();
                $http.post(
                    '/api/practicesession',
                    {
                        wordListId: wordListId,
                        practiceMode: 'Default'
                    })
                    .success(function (result) {
                        deferred.resolve(result.practiceSessionId);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            },
            getUnfinished: function (wordListId) {
                var deferred = $q.defer();
                $http.get(
                    '/api/unfinishedpracticesession',
                    {
                        params:
                        {
                            wordListId: wordListId
                        }
                    })
                    .success(function (result) {
                        deferred.resolve(result);
                    })
                    .error(function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            }
        };

        return service;
    }
})();