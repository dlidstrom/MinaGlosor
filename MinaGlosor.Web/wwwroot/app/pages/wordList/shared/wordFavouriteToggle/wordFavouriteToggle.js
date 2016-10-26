(function () {
    'use strict';

    angular.module('pages.wordlist')
        .component(
            'wordFavouriteToggle', {
                bindings: {
                    wordId: '<',
                    isFavourite: '<'
                },
                controller: 'WordFavouriteToggleController',
                templateUrl: '/wwwroot/app/pages/wordList/shared/wordFavouriteToggle/wordFavouriteToggle.html'
            })
        .controller('WordFavouriteToggleController', WordFavouriteToggleController);

    WordFavouriteToggleController.$inject = ['WordFavouriteService'];
    function WordFavouriteToggleController(wordFavouriteService) {
        var $ctrl = this;

        $ctrl.inProgress = false;

        $ctrl.toggle = toggle;

        function toggle() {
            $ctrl.inProgress = true;
            wordFavouriteService.submit($ctrl.wordId, !$ctrl.isFavourite)
                .success(function (response) {
                    $ctrl.isFavourite = response.isFavourite;
                }).finally(function () {
                    $ctrl.inProgress = false;
                });
        }
    }
})();