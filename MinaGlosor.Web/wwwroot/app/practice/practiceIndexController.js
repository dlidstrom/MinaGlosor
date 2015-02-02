(function () {
    'use strict';

    angular.module('mgApp').controller('PracticeIndexController', PracticeIndexController);

    PracticeIndexController.$inject = ['$location', 'WordListId', 'PracticeService', 'UnfinishedPracticeSessions'];
    function PracticeIndexController($location, wordListId, practiceService, unfinishedPracticeSessions) {
        var practiceIndex = this;

        practiceIndex.saving = false;

        practiceIndex.wordListId = wordListId;
        practiceIndex.unfinishedPracticeSessions = unfinishedPracticeSessions;
        practiceIndex.startNew = startNew;

        function startNew() {
            practiceIndex.saving = true;
            practiceService.create(wordListId).then(function (practiceId) {
                $location.path('/wordlist/' + wordListId + '/practice/' + practiceId);
            });
        }
    };
})();