(function () {
    'use strict';

    angular.module('pages.search')
        .component(
            'navbarSearch',
            {
                bindings: {
                    dataNavbarTarget: '@'
                },
                controller: 'NavbarSearchController',
                templateUrl: '/wwwroot/app/pages/search/navbarSearch/navbarSearch.html'
            })
        .controller('NavbarSearchController', NavbarSearchController);

    NavbarSearchController.$inject = ['$state'];
    function NavbarSearchController($state) {
        var $ctrl = this;

        $ctrl.submit = submit;

        function submit(q) {
            $state.go('search', {q: q});
            $ctrl.q = '';
        };
    }
})();