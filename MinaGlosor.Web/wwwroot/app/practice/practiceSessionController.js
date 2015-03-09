(function () {
    'use strict';

    angular.module('mgApp').controller('PracticeSessionController', PracticeSessionController);

    PracticeSessionController.$inject = ['$location', 'PracticeWordService', 'WordListId', 'PracticeSessionId', 'PracticeWord'];
    function PracticeSessionController($location, practiceWordService, wordListId, practiceSessionId, practiceWord) {
        var practiceSession = this;

        // variables
        practiceSession.saving = null;
        practiceSession.showWord = true;
        practiceSession.practiceWord = practiceWord;

        // functions
        practiceSession.showMeaning = showMeaning;
        practiceSession.submit = submit;

        function showMeaning() {
            practiceSession.showWord = false;
        }

        function submit(confidenceLevel) {
            practiceSession.saving = confidenceLevel;
            practiceSession.redirecting = false;
            practiceWordService.submit(practiceSessionId, practiceSession.practiceWord.practiceWordId, confidenceLevel)
                .then(function (data) {
                    if (data.isFinished) {
                        $location.path('/wordlist/' + wordListId + '/practice/' + practiceSessionId + '/summary');
                        practiceSession.redirecting = true;
                    } else {
                        practiceWordService.getNext(practiceSessionId).then(function (newPracticeWord) {
                            practiceSession.practiceWord = newPracticeWord;
                            practiceSession.showWord = true;
                        });
                    }
                }).finally(function () {
                    if (!practiceSession.redirecting)
                        practiceSession.saving = null;
                });
        }
    };
})();