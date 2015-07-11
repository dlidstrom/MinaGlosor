(function () {
    'use strict';

    angular.module('mgApp').controller('ViewFavouritesController', ViewFavouritesController);

    ViewFavouritesController.$inject = ['$location', 'Words'];
    function ViewFavouritesController($location, words) {
        var viewer = this;

        viewer.wordListId = 0;
        viewer.wordListName = words.wordListName || 'Favoriter';
        viewer.words = words.words.map(function (x) {
            return {
                id: x.id,
                isFavourite: true,
                text: x.text,
                definition: x.definition
            };
        });
        viewer.canPractice = false;
        viewer.canAdd = false;
        viewer.returnUrl = $location.url();
    }
})();