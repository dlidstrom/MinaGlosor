(function () {
    'use strict';

    angular.module('mgApp').controller('NavbarController', NavbarController);

    NavbarController.$inject = ['$scope'];
    function NavbarController($scope) {
        var $ctrl = this;
        $scope.$on('$locationChangeSuccess', function () {
            $ctrl.isCollapsed = true;
        });
    }
})();