(function () {
    'use strict';

    angular.module('mgApp')
        .directive('paging', PagingDirective)
        .controller('PagingDirectiveController', PagingDirectiveController);

    function PagingDirective() {
        return {
            restrict: 'E',
            scope: {
                paging: '='
            },
            replace: true,
            controller: 'PagingDirectiveController',
            template:
                '<div class="row" ng-show="paging.hasPages">'
                + '<div class="col-xs-12">'
                + '  <ul uib-pagination'
                + '    total-items="paging.totalItems"'
                + '    items-per-page="paging.itemsPerPage"'
                + '    ng-model="paging.currentPage"'
                + '    ng-change="pageChanged()"></ul>'
                + '  </div>'
                + '</div>'
        };
    }

    PagingDirectiveController.$inject = ['$scope', '$location'];
    function PagingDirectiveController($scope, $location) {
        var directive = $scope;
        directive.pageChanged = pageChanged;

        function pageChanged() {
            var url = $location.path();
            $location.path(url).search('page', directive.paging.currentPage);
        }
    }
})();