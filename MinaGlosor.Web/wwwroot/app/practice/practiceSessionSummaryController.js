(function () {
    'use strict';

    angular.module('mgApp').controller('PracticeSessionSummaryController', PracticeSessionSummaryController);

    PracticeSessionSummaryController.$inject = ['WordListId'];
    function PracticeSessionSummaryController(wordListId) {
        var summary = this;

        // variables
        summary.wordListId = wordListId;
    };
})();