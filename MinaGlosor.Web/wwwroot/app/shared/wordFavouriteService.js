(function () {
    'use strict';

    angular.module('mgApp').factory('WordFavouriteService', WordFavouriteService);

    WordFavouriteService.$inject = ['$http'];
    function WordFavouriteService($http) {
        var service = {
            submit: function (wordId) {
                return $http.post(
                    '/api/wordfavourite',
                    {
                        wordId: wordId
                    });
            }
        };

        return service;
    }
})();