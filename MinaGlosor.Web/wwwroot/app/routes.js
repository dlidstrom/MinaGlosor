(function () {
    'use strict';

    angular.module('mgApp').config(Config);

    Config.$inject = ['$routeProvider', '$locationProvider'];
    function Config($routeProvider, $locationProvider) {
        $locationProvider.html5Mode(true);
        $routeProvider
            .when(
                '/wordlist',
                {
                    templateUrl: '/wwwroot/app/wordlist/index.html',
                    controller: 'WordListController',
                    controllerAs: 'wordLists',
                    resolve: {
                        UserWordLists:
                        [
                            'WordListService',
                            function (wordListService) {
                                return wordListService.getAll();
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
                '/wordlist/:id',
                {
                    templateUrl: '/wwwroot/app/wordlist/view.html',
                    controller: 'ViewWordListController',
                    controllerAs: 'viewer',
                    resolve: {
                        WordListId: [
                            '$route',
                            function ($route) {
                                return $route.current.params.id;
                            }
                        ],
                        Words:
                        [
                            '$route',
                            'WordService',
                            function ($route, wordService) {
                                return wordService.getAll($route.current.params.id);
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
                        UnfinishedPracticeSessions:
                            [
                                '$route',
                                'PracticeService',
                                function ($route, practiceService) {
                                    return practiceService.getUnfinished($route.current.params.wordListId);
                                }
                            ],
                        WordList:
                            [
                                '$route',
                                'WordListService',
                                function ($route, wordListService) {
                                    return wordListService.getById($route.current.params.wordListId);
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
            .otherwise({
                redirectTo: '/wordlist'
            });
    }
})();