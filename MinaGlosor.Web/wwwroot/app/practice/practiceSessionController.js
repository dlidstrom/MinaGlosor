(function () {
    'use strict';

    angular.module('mgApp').controller('PracticeSessionController', PracticeSessionController);

    PracticeSessionController.$inject = ['$location', 'PracticeWordService', 'PracticeWord'];
    function PracticeSessionController($location, practiceWordService, practiceWord) {
        var practiceSession = this;

        // variables
        practiceSession.practiceWord = practiceWord;
        practiceSession.practiceSessionId = practiceWord.practiceSessionId;
        practiceSession.wordListId = practiceWord.wordListId;
        practiceSession.wordListName = practiceWord.wordListName;

        // functions
        practiceSession.showMeaning = showMeaning;

        function showMeaning() {
            var path = '/wordlist/' +
                practiceSession.wordListId +
                '/practice/' +
                practiceSession.practiceSessionId +
                '/' +
                practiceWord.practiceWordId;
            $location.path(path);
        }
    };
})();