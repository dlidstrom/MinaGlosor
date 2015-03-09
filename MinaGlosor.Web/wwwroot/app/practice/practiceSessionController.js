(function () {
    'use strict';

    angular.module('mgApp').controller('PracticeSessionController', PracticeSessionController);

    PracticeSessionController.$inject = ['$location', 'PracticeWordService', 'PracticeWord'];
    function PracticeSessionController($location, practiceWordService, practiceWord) {
        var practiceSession = this;

        // variables
        practiceSession.saving = null;
        practiceSession.showWord = true;
        practiceSession.practiceWord = practiceWord;
        practiceSession.practiceSessionId = practiceWord.practiceSessionId;
        practiceSession.wordListId = practiceWord.wordListId;
        practiceSession.wordListName = practiceWord.wordListName;

        // functions
        practiceSession.showMeaning = showMeaning;
        practiceSession.submit = submit;

        function showMeaning() {
            practiceSession.showWord = false;
        }

        function submit(confidenceLevel) {
            practiceSession.saving = confidenceLevel;
            practiceSession.redirecting = false;
            practiceWordService.submit(practiceSession.practiceWord, confidenceLevel)
                .then(function (data) {
                    if (data.isFinished) {
                        var path = '/wordlist/' + practiceSession.wordListId + '/practice/' + practiceSession.practiceSessionId + '/summary';
                        $location.path(path);
                        practiceSession.redirecting = true;
                    } else {
                        practiceWordService.getNext(practiceSession.practiceSessionId).then(function (newPracticeWord) {
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