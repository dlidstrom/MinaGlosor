(function () {
    'use strict';

    angular.module('mgApp').controller('ViewFavouritesController', ViewFavouritesController);

    ViewFavouritesController.$inject = ['$location', 'result'];
    function ViewFavouritesController($location, result) {
        var viewer = this;

        viewer.wordListId = 0;
        viewer.wordListName = result.wordListName || 'Favoriter';
        viewer.words = result.words.map(function (x) {
            return {
                id: x.id,
                isFavourite: true,
                text: x.text,
                definition: x.definition
            };
        });
        viewer.canPractice = false;
        viewer.canAdd = result.canAdd;
        viewer.canEdit = result.canEdit;
        viewer.paging = result.paging;
        viewer.returnUrl = $location.url();
    }
})();