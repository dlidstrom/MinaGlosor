(function () {
    'use strict';

    angular.module('pages.wordlist')
        .component(
            'viewFavourites',
            {
                bindings: {
                    model: '<'
                },
                controller: 'ViewFavouritesController',
                templateUrl: '/wwwroot/app/pages/wordlist/viewFavourites/viewFavourites.html'
            })
        .controller('ViewFavouritesController', ViewFavouritesController);

    ViewFavouritesController.$inject = [];
    function ViewFavouritesController() {
        //var viewer = this;

        //viewer.wordListId = 0;
        //viewer.wordListName = result.wordListName || 'Favoriter';
        //viewer.words = result.words.map(function (x) {
        //    return {
        //        id: x.id,
        //        isFavourite: true,
        //        text: x.text,
        //        definition: x.definition
        //    };
        //});
        //viewer.canPractice = false;
        //viewer.canAdd = result.canAdd;
        //viewer.canEdit = result.canEdit;
        //viewer.paging = result.paging;
        //viewer.returnUrl = $location.url();
    }
})();