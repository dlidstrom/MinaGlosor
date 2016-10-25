(function () {
    'use strict';

    angular.module('mgApp').controller('PracticeIndexController', PracticeIndexController);

    PracticeIndexController.$inject = ['$location', 'WordListId', 'PracticeService', 'Result'];
    function PracticeIndexController($location, wordListId, practiceService, result) {
        var practiceIndex = this;

        practiceIndex.wordListName = result.wordListName;
        practiceIndex.unfinishedPracticeSessions = result.unfinishedPracticeSessions;
        practiceIndex.newSession = startNew;
        practiceIndex.formatDate = formatDate;

        function startNew() {
            practiceService.create(wordListId).then(function (practiceId) {
                $location.path('/wordlist/' + wordListId + '/practice/' + practiceId);
            });
        }

        function formatDate(dateString) {
            var date = new Date(dateString);
            var fullYear = date.getFullYear();
            var month = date.getMonth() + 1;
            var d = date.getDate();
            var formattedDate = fullYear + '-' + (month < 10 ? '0' + month : month) + '-' + (d < 10 ? '0' + d : d);
            return formattedDate;
        }
    };
})();