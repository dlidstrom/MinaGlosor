(function () {
    'use strict';

    angular.module('pages.wordlist', ['ui.router'])
        .config(Config);

    Config.$inject = ['$stateProvider'];
    function Config($stateProvider) {
        $stateProvider.state(
            {
                name: 'wordlist',
                url: '/wordlist/{wordListId:[0-9]+}?page',
                params: {
                    page: {
                        value: '1',
                        squash: true
                    }
                },
                component: 'viewWordList',
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
                name: 'wordlist-create',
                url: '/wordlist/new',
                component: 'createWordList'
            });

        $stateProvider.state(
            {
                name: 'wordlist-addword',
                url: '/wordlist/{wordListId:[0-9]+}/word/new',
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

        $stateProvider.state(
            {
                name: 'wordlist-editword',
                url: '/wordlist/{wordListId:[0-9]+}/word/:wordId',
                component: 'editWord',
                resolve: {
                    model: [
                        '$stateParams',
                        'WordService',
                        function ($stateParams, wordService) {
                            return wordService.get($stateParams.wordId);
                        }
                    ],
                    returnState: [
                        '$state',
                        function ($state) {
                            return {
                                name: $state.current.name,
                                params: $state.params
                            };
                        }
                    ]
                }
            });

        $stateProvider.state(
            {
                name: 'wordlist-favourites',
                url: '/wordlist/favourites?page',
                params: {
                    page: {
                        value: '1',
                        squash: true
                    }
                },
                component: 'viewFavourites',
                resolve: {
                    model: [
                        '$stateParams',
                        'WordFavouriteService',
                        function ($stateParams, wordFavouriteService) {
                            return wordFavouriteService.getAll($stateParams.page);
                        }
                    ]
                }
            });
    }
})();