(function () {
    'use strict';

    angular.module('pages.practice')
        .component(
            'practiceStart',
            {
                bindings: {
                    model: '<',
                    wordListId: '<'
                },
                controller: 'PracticeStartController',
                templateUrl: '/wwwroot/app/pages/practice/practiceStart/practiceStart.html'
            })
        .controller('PracticeStartController', PracticeStartController);

    PracticeStartController.$inject = ['$state', 'PracticeService'];
    function PracticeStartController($state, practiceService) {
        var $ctrl = this;

        $ctrl.newSession = newSession;

        function newSession() {
            practiceService.create($ctrl.wordListId).then(function (practiceSessionId) {
                $state.go('practice-word-text', { wordListId: $ctrl.wordListId, practiceSessionId: practiceSessionId });
            });
        }
    }
})();