(function () {
    'use strict';

    angular.module('mgApp').config(Config);

    Config.$inject = ['$routeProvider', '$locationProvider', '$stateProvider', '$urlRouterProvider'];
    function Config($routeProvider, $locationProvider, $stateProvider, $urlRouterProvider) {
        $locationProvider.html5Mode(true);
        $urlRouterProvider.otherwise('/');
        //$stateProvider.state(
        //    'profile',
        //    {
        //        url: '/',
        //        redirectTo: 'test'
        //    });
        return;
        $routeProvider
            .when(
                '/progress',
                {
                    templateUrl: '/wwwroot/app/progress/index.html',
                    controller: 'ProgressController',
                    controllerAs: 'progress',
                    resolve: {
                        result: [
                            '$route',
                            'ProgressService',
                            function ($route, progressService) {
                                return progressService.getAll($route.current.params.page);
                            }
                        ]
                    }
                })
            .when(
                '/wordlist/:id/add',
                {
                    templateUrl: '/wwwroot/app/wordlist/addWordEditor.html',
                    controller: 'AddWordController',
                    controllerAs: 'editor',
                    resolve: {
                        WordList:
                        [
                            '$route',
                            'WordListService',
                            function ($route, wordListService) {
                                return wordListService.getById($route.current.params.id);
                            }
                        ]
                    }
                })
            .when(
                '/wordlist/new',
                {
                    templateUrl: '/wwwroot/app/wordlist/new.html'
                })
            .when(
                '/wordlist/favourites',
                {
                    templateUrl: '/wwwroot/app/wordlist/view.html',
                    controller: 'ViewFavouritesController',
                    controllerAs: 'viewer',
                    resolve: {
                        result:
                        [
                            '$route',
                            'WordFavouriteService',
                            function ($route, wordFavouriteService) {
                                return wordFavouriteService.getAll($route.current.params.page);
                            }
                        ]
                    }
                })
            .when(
                '/wordlist/:id',
                {
                    templateUrl: '/wwwroot/app/wordlist/view.html',
                    controller: 'ViewWordListController',
                    controllerAs: 'viewer',
                    resolve: {
                        wordListId: [
                            '$route',
                            function ($route) {
                                return $route.current.params.id;
                            }
                        ],
                        result:
                        [
                            '$route',
                            'WordService',
                            function ($route, wordService) {
                                return wordService.getAll($route.current.params.id, $route.current.params.page);
                            }
                        ]
                    }
                })
            .when(
                '/word/:id/edit',
                {
                    templateUrl: '/wwwroot/app/word/edit.html',
                    controller: 'EditWordController',
                    controllerAs: 'editor',
                    resolve: {
                        Word:
                        [
                            '$route',
                            'WordService',
                            function ($route, wordService) {
                                return wordService.get($route.current.params.id);
                            }
                        ],
                        ReturnUrl:
                        [
                            '$route',
                            function ($route) {
                                return $route.current.params.returnUrl;
                            }
                        ]
                    }
                })
            .when(
                '/wordlist/:wordListId/practice/',
                {
                    templateUrl: '/wwwroot/app/practice/index.html',
                    controller: 'PracticeIndexController',
                    controllerAs: 'practiceIndex',
                    resolve: {
                        Result:
                        [
                            '$route',
                            'PracticeService',
                            function ($route, practiceService) {
                                return practiceService.getUnfinished($route.current.params.wordListId);
                            }
                        ],
                        WordListId:
                        [
                            '$route',
                            function ($route) {
                                return $route.current.params.wordListId;
                            }
                        ]
                    }
                })
            .when(
                '/wordlist/:wordListId/practice/:practiceSessionId/summary',
                {
                    templateUrl: '/wwwroot/app/practice/practiceSessionSummary.html',
                    controller: 'PracticeSessionSummaryController',
                    controllerAs: 'summary',
                    resolve: {
                        WordListId:
                            [
                                '$route',
                                function ($route) {
                                    return $route.current.params.wordListId;
                                }
                            ]
                    }
                })
            .when(
                '/wordlist/:wordListId/practice/:practiceSessionId',
                {
                    templateUrl: '/wwwroot/app/practice/practiceSession.html',
                    controller: 'PracticeSessionController',
                    controllerAs: 'practiceSession',
                    resolve: {
                        PracticeWord:
                            [
                                '$route',
                                'PracticeWordService',
                                function ($route, practiceWordService) {
                                    return practiceWordService.getNext($route.current.params.practiceSessionId);
                                }
                            ]
                    }
                })
            .when(
                '/wordlist/:wordListId/practice/:practiceSessionId/:practiceWordId',
                {
                    templateUrl: '/wwwroot/app/practice/practiceSessionMeaning.html',
                    controller: 'PracticeSessionMeaningController',
                    controllerAs: 'practiceSession',
                    resolve: {
                        PracticeWord:
                            [
                                '$route',
                                'PracticeWordService',
                                function ($route, practiceWordService) {
                                    return practiceWordService.getById($route.current.params.practiceSessionId, $route.current.params.practiceWordId);
                                }
                            ]
                    }
                })
            .when(
                '/help',
                {
                    templateUrl: '/wwwroot/app/help/index.html'
                })
            .when(
                '/search',
                {
                    templateUrl: '/wwwroot/app/search/index.html',
                    controller: 'SearchIndexController',
                    controllerAs: 'searchIndex',
                    reloadOnSearch: false,
                    resolve: {
                        q:
                        [
                            '$route',
                            function ($route) {
                                return $route.current.params.q;
                            }
                        ],
                        result: [
                            '$route',
                            'SearchService',
                            function ($route, searchService) {
                                return searchService.search($route.current.params.q);
                            }
                        ]
                    }
                })
            .when(
                '/browse',
                {
                    templateUrl: '/wwwroot/app/browse/index.html',
                    controller: 'BrowseController',
                    controllerAs: 'controller',
                    resolve: {
                        result: [
                            '$route',
                            'BrowseService',
                            function ($route, browseService) {
                                return browseService.search($route.current.params.page);
                            }
                        ]
                    }
                })
            .otherwise({
                redirectTo: '/progress'
            });
    }
})();