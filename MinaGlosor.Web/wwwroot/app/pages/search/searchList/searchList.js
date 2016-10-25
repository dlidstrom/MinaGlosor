(function () {
    'use strict';

    angular.module('pages.search')
        .component(
            'searchList',
            {
                bindings: {
                    q: '<',
                    model: '<'
                },
                controller: 'SearchListController',
                templateUrl: '/wwwroot/app/pages/search/searchList/searchList.html'
            })
        .controller('SearchListController', SearchListController);

    SearchListController.$inject = ['$scope', '$state', '$sce', '$location'];
    function SearchListController($scope, $state, $sce, $location) {
        var $ctrl = this;

        $ctrl.q = q;
        $ctrl.submit = submit;
        $ctrl.words = trustHtml($ctrl.model.words);
        $ctrl.returnUrl = encodeURIComponent($location.url());

        $scope.$on('$locationChangeSuccess', function () {
            $ctrl.q = $state.params.search;
        });

        function submit(newQ) {
            $state.go('.', { q: newQ }, { notify: false });
        };

        function trustHtml(words) {
            var trusted = words.map(function (w) {
                var text = $sce.trustAsHtml(w.text);
                var definition = $sce.trustAsHtml(w.definition);
                var wordId = w.wordId;
                return {
                    text: text,
                    definition: definition,
                    wordId: wordId,
                    isFavourite: false
                };
            });

            return trusted;
        }
    }
})();