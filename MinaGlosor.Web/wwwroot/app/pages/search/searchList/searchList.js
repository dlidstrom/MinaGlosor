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

    SearchListController.$inject = ['$scope', '$state', '$sce', '$location', 'SearchService'];
    function SearchListController($scope, $state, $sce, $location, searchService) {
        var $ctrl = this;

        $ctrl.q = $state.params.q;
        $ctrl.submit = submit;
        $ctrl.words = trustHtml($ctrl.model.words);
        $ctrl.returnUrl = encodeURIComponent($location.url());

        $scope.$on('$locationChangeSuccess', function () {
            $ctrl.q = $state.params.q;
        });

        function submit(newQ) {
            searchService.search(newQ)
                .then(function (result) {
                    $ctrl.words = trustHtml(result.words);
                });
            $state.go('.', { q: newQ }, { notify: false });
        }

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