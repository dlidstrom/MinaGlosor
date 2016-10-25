(function () {
    'use strict';

    angular.module('pages.practice', ['ui.router'])
        .config(Config);

    Config.$inject = ['$stateProvider'];
    function Config($stateProvider) {
        $stateProvider.state(
        {
            name: 'practice',
            url: '/practice/word-list-:wordListId',
            component: 'practiceStart',
            resolve: {
                model:
                [
                    '$stateParams',
                    'PracticeService',
                    function ($stateParams, practiceService) {
                        return practiceService.getUnfinished($stateParams.wordListId);
                    }
                ],
                wordListId:
                [
                    '$stateParams',
                    function ($stateParams) {
                        return $stateParams.wordListId;
                    }
                ]
            }
        });

        $stateProvider.state(
        {
            name: 'practice-word-text',
            url: '/practice/word-list-:wordListId/session-:practiceSessionId',
            component: 'practiceWordText',
            resolve: {
                model:
                [
                    '$stateParams',
                    'PracticeWordService',
                    function ($stateParams, practiceWordService) {
                        return practiceWordService.getNext($stateParams.practiceSessionId);
                    }
                ]
            }
        });

        $stateProvider.state(
        {
            name: 'practice-summary',
            url: '/practice/word-list-:wordListId/session-:practiceSessionId/summary',
            component: 'practiceSummary',
            resolve: {
                model:
                [
                    '$stateParams',
                    function ($stateParams) {
                        return {
                            wordListId: $stateParams.wordListId
                        };
                    }
                ]
            }
        });

        $stateProvider.state(
        {
            name: 'practice-word-definition',
            url: '/practice/word-list-:wordListId/session-:practiceSessionId/word-:practiceWordId',
            component: 'practiceWordDefinition',
            resolve: {
                model:
                [
                    '$stateParams',
                    'PracticeWordService',
                    function ($stateParams, practiceWordService) {
                        return practiceWordService.getById($stateParams.practiceSessionId, $stateParams.practiceWordId);
                    }
                ]
            }
        });
    }
})();