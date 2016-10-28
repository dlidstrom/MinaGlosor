(function () {
    'use strict';

    angular.module('mgApp').controller('NavbarController', NavbarController);

    NavbarController.$inject = ['$rootScope'];
    function NavbarController($rootScope) {
        var $ctrl = this;
        $ctrl.isCollapsed = true;
        $rootScope.$on('$locationChangeSuccess', function () {
            $ctrl.isCollapsed = true;
        });
    }
})();