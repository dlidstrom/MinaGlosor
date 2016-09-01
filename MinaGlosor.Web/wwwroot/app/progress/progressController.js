(function () {
    'use strict';

    angular.module('mgApp').controller('ProgressController', ProgressController);

    ProgressController.$inject = ['$location', 'result'];
    function ProgressController($location, result) {
        var progress = this;

        progress.progresses = result.progresses;
        progress.numberOfFavourites = result.numberOfFavourites;
        progress.paging = result.paging;
        progress.pageChanged = pageChanged;

        function pageChanged() {
            $location.path('/progress').search('page', progress.paging.currentPage);
        }
    };
})();