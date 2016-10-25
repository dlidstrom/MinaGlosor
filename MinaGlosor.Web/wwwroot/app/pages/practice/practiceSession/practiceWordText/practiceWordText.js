(function () {
    'use strict';

    angular.module('pages.practice')
        .component(
            'practiceWordText',
            {
                bindings: {
                    model: '<'
                },
                controller: 'PracticeWordTextController',
                templateUrl: '/wwwroot/app/pages/practice/practiceSession/practiceWordText/practiceWordText.html'
            })
        .controller('PracticeWordTextController', PracticeWordTextController);

    PracticeWordTextController.$inject = ['$state'];
    function PracticeWordTextController($state) {
        var $ctrl = this;

        $ctrl.showMeaning = showMeaning;

        function showMeaning() {
            $state.go(
                'practice-word-definition',
                {
                    wordListId: $ctrl.model.wordListId,
                    practiceSessionId: $ctrl.model.practiceSessionId,
                    practiceWordId: $ctrl.model.practiceWordId
                });
        }
    };
})();