(function () {
    'use strict';

    angular.module('pages.practice')
        .component(
            'unfinishedPracticeSessions', {
                bindings: {
                    sessions: '<',
                    wordListId: '<'
                },
                controller: 'UnfinishedPracticeSessionsController',
                templateUrl: '/wwwroot/app/pages/practice/practiceStart/unfinishedPracticeSessions/unfinishedPracticeSessions.html'
            })
        .controller('UnfinishedPracticeSessionsController', UnfinishedPracticeSessionsController);

    function UnfinishedPracticeSessionsController() {
        var $ctrl = this;

        $ctrl.formatDate = formatDate;

        function formatDate(dateString) {
            var date = new Date(dateString);
            var fullYear = date.getFullYear();
            var month = date.getMonth() + 1;
            var d = date.getDate();
            var formattedDate = fullYear + '-' + (month < 10 ? '0' + month : month) + '-' + (d < 10 ? '0' + d : d);
            return formattedDate;
        }
    }
})();