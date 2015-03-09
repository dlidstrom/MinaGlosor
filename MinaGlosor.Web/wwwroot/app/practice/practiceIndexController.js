(function () {
    'use strict';

    angular.module('mgApp').controller('PracticeIndexController', PracticeIndexController);

    PracticeIndexController.$inject = ['$location', 'WordList', 'PracticeService', 'UnfinishedPracticeSessions'];
    function PracticeIndexController($location, wordList, practiceService, unfinishedPracticeSessions) {
        var practiceIndex = this;

        practiceIndex.saving = false;

        practiceIndex.wordList = wordList;
        practiceIndex.unfinishedPracticeSessions = unfinishedPracticeSessions;
        practiceIndex.startNew = startNew;
        practiceIndex.formatDate = formatDate;

        function startNew() {
            practiceIndex.saving = true;
            var wordListId = practiceIndex.wordList.wordListId;
            practiceService.create(wordListId).then(function (practiceId) {
                $location.path('/wordlist/' + wordListId + '/practice/' + practiceId);
            });
        }

        function formatDate(dateString) {
            var date = new Date(dateString);
            var fullYear = date.getFullYear();
            var month = date.getMonth() + 1;
            var d = date.getDate();
            var formattedDate = fullYear + '-' + (month < 9 ? '0' + month : month) + '-' + d;
            return formattedDate;
        }
    };
})();