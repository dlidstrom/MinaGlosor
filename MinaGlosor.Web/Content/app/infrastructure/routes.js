﻿// @reference app.js
(function (app) {
    'use strict';

    app.config([
        '$routeProvider',
        '$locationProvider',
        function ($routeProvider, $locationProvider) {
            $locationProvider.html5Mode(true);
            $routeProvider
                .when(
                    '/wordlist',
                    {
                        templateUrl: '/Content/app/wordlist/index.html',
                        controller: 'WordListCtrl',
                        resolve: {
                            WordLists:
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
                        templateUrl: '/Content/app/wordlist/addword.html',
                        controller: 'AddWordCtrl',
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
                    '/wordlist/:id',
                    {
                        templateUrl: '/Content/app/wordlist/view.html',
                        controller: 'ViewListCtrl',
                        resolve: {
                            Words:
                            [
                                '$route',
                                'WordService',
                                function ($route, wordService) {
                                    return wordService.getAll($route.current.params.id);
                                }
                            ]
                        }
                    }
                )
                .when(
                    '/wordlist/new',
                    {
                        templateUrl: '/Content/app/wordlist/new.html'
                    })
                .otherwise({
                    redirectTo: '/wordlist'
                });
        }
    ]);
})(window.App);