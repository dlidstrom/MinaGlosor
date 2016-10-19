(function () {
    'use strict';

    angular.module('pages.browse')
        .component(
            'browseList', {
                bindings: {
                    model: '<'
                },
                controller: 'BrowseListController',
                templateUrl: '/wwwroot/app/pages/browse/browseList/browseList.html'
            })
        .controller('BrowseListController', BrowseListController);

    BrowseListController.$inject = ['$state'];
    function BrowseListController($state) {
        var $ctrl = this;

        $ctrl.pageChanged = pageChanged;

        function pageChanged() {
            $state.go('browse', { page: $ctrl.model.paging.currentPage });
        }
    }
})();