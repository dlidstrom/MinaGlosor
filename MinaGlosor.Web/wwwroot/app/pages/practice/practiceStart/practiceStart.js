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
                templateUrl: '/wwwroot/app/pages/practice/practiceStart/practiceStart.html'
            })
        .controller('PracticeStartController', PracticeStartController);

    PracticeStartController.$inject = ['$state', 'PracticeService'];
    function PracticeStartController($state, practiceService) {
        var $ctrl = this;

        $ctrl.newSession = newSession;

        function newSession() {
            practiceService.create(wordListId).then(function (practiceId) {
                $state.go('practice-word-text', { wordListId: wordListId, practiceId: practiceId });
            });
        }
    }
})();