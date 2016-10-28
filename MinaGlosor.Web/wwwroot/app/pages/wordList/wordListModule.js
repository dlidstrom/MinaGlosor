(function () {
    'use strict';

    angular.module('pages.wordlist', ['ui.router'])
        .config(Config);

    Config.$inject = ['$stateProvider'];
    function Config($stateProvider) {
        $stateProvider.state(
            {
                name: 'wordlist',
                url: '/wordlist/:wordListId?page',
                component: 'viewWordList',
                params: {
                    page: {
                        value: '1',
                        squash: true
                    }
                },
                resolve: {
                    model: [
                        '$stateParams',
                        'WordService',
                        function ($stateParams, wordService) {
                            return wordService.getAll($stateParams.wordListId, $stateParams.page);
                        }
                    ]
                }
            });

        $stateProvider.state(
            {
                name: 'wordlist-addword',
                url: '/wordlist/:wordListId/word/new',
                component: 'addWord',
                resolve: {
                    model: [
                        '$stateParams',
                        'WordListService',
                        function ($stateParams, wordListService) {
                            return wordListService.getById($stateParams.wordListId);
                        }
                    ]
                }
            });
    }
})();