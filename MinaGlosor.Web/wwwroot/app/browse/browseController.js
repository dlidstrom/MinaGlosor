(function () {
    'use strict';

    angular.module('mgApp').controller('BrowseController', BrowseController);

    BrowseController.$inject = ['result'];
    function BrowseController(result) {
        var controller = this;

        controller.wordLists = result.wordLists;
    }
})();