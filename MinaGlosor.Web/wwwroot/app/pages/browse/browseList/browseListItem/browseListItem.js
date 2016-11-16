(function () {
    'use strict';

    angular.module('pages.browse')
        .component(
            'browseListItem',
            {
                bindings: {
                    item: '<'
                },
                templateUrl: '/wwwroot/app/pages/browse/browseList/browseListItem/browseListItem.html'
            });
})();