(function () {
    'use strict';

    angular.module('mgApp').controller('ProgressController', ProgressController);

    ProgressController.$inject = ['$state', 'Result'];
    function ProgressController($state, result) {
        var controller = this;

        controller.progresses = result.progresses;
        controller.numberOfFavourites = result.numberOfFavourites;
        controller.paging = result.paging;
        controller.pageChanged = pageChanged;

        function pageChanged() {
            $state.go('progress', { page: controller.paging.currentPage });
        }
    };
})();