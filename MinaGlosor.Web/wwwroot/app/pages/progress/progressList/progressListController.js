(function () {
    'use strict';

    angular.module('pages.progress')
        .component('progressListComponent', {
            bindings: {
                progressResult: '<'
            },
            templateUrl: '/wwwroot/app/pages/progress/progressList/progressList.html'
        })
        .controller('ProgressListController', ProgressListController);

    ProgressListController.$inject = ['$state'];
    function ProgressListController($state) {
        var $ctrl = this;

        $ctrl.pageChanged = pageChanged;

        function pageChanged() {
            $state.go('progress', { page: $ctrl.progressResult.paging.currentPage });
        }
    };
})();