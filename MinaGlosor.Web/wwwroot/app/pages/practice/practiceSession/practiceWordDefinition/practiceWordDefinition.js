(function () {
    'use strict';

    angular.module('pages.practice')
        .component(
            'practiceWordDefinition',
            {
                bindings: {
                    model: '<'
                },
                controller: 'PracticeWordDefinitionController',
                templateUrl: '/wwwroot/app/pages/practice/practiceSession/practiceWordDefinition/practiceWordDefinition.html'
            }
        )
        .controller('PracticeWordDefinitionController', PracticeWordDefinitionController);

    PracticeWordDefinitionController.$inject = ['$state', 'PracticeWordService'];
    function PracticeWordDefinitionController($state, practiceWordService) {
        var $ctrl = this;

        // variables
        $ctrl.saving = null;

        // functions
        $ctrl.submit = submit;

        function submit(confidenceLevel) {
            $ctrl.saving = confidenceLevel;
            practiceWordService.submit($ctrl.model.practiceSessionId, $ctrl.model.practiceWordId, confidenceLevel)
                .then(function (data) {
                    if (data.isFinished) {
                        $state.go(
                            'practice-summary',
                            {
                                wordListId: $ctrl.model.wordListId,
                                practiceSessionId: $ctrl.model.practiceSessionId
                            });
                    } else {
                        $state.go(
                            'practice-word-text',
                            {
                                wordListId: $ctrl.model.wordListId,
                                practiceSessionId: $ctrl.model.practiceSessionId
                            });
                    }
                });
        }
    };
})();