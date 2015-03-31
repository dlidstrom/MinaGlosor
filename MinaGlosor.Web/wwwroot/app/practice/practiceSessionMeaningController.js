(function () {
    'use strict';

    angular.module('mgApp').controller('PracticeSessionMeaningController', PracticeSessionMeaningController);

    PracticeSessionMeaningController.$inject = ['$location', 'PracticeWordService', 'PracticeWord'];
    function PracticeSessionMeaningController($location, practiceWordService, practiceWord) {
        var practiceSession = this;

        // variables
        practiceSession.saving = null;
        practiceSession.showWord = true;
        practiceSession.practiceWord = practiceWord;
        practiceSession.practiceSessionId = practiceWord.practiceSessionId;
        practiceSession.wordListId = practiceWord.wordListId;
        practiceSession.wordListName = practiceWord.wordListName;
        practiceSession.green = practiceWord.green;
        practiceSession.blue = practiceWord.blue;
        practiceSession.yellow = practiceWord.yellow;

        // functions
        practiceSession.submit = submit;

        function submit(confidenceLevel) {
            practiceSession.saving = confidenceLevel;
            practiceWordService.submit(practiceSession.practiceWord, confidenceLevel)
                .then(function (data) {
                    var path = '/wordlist/' + practiceSession.wordListId + '/practice/' + practiceSession.practiceSessionId;
                    if (data.isFinished) {
                        $location.path(path + '/summary');
                    } else {
                        $location.path(path);
                    }
                }).finally(function () {
                    if (!practiceSession.redirecting)
                        practiceSession.saving = null;
                });
        }
    };
})();