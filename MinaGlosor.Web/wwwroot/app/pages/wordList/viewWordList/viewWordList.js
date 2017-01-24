(function () {
    'use strict';

    angular.module('pages.wordlist')
        .component(
            'viewWordList', {
                bindings: {
                    model: '<'
                },
                controller: 'ViewWordListController',
                templateUrl: '/wwwroot/app/pages/wordList/viewWordList/viewWordList.html'
            })
        .controller('ViewWordListController', ViewWordListController);

    ViewWordListController.$inject = ['$location', '$state', 'PublishModal', 'WordListService'];
    function ViewWordListController($location, $state, publishModal, wordListService) {
        var $ctrl = this;

        $ctrl.canPractice = $ctrl.model.words.length > 0; // TODO: Flytta till backend
        $ctrl.published = $ctrl.model.publishState === 'Published'; // TODO Fix with TypeScript
        $ctrl.returnUrl = $location.url();

        $ctrl.updateName = updateName;
        $ctrl.publish = publish;

        function publish(size) {
            publishModal.publish($ctrl.published, $ctrl.model.wordListName, size).then(function (published) {
                wordListService.publish($ctrl.model.wordListId, published).then(function () {
                    $state.reload();
                });
            });
        }

        function updateName(newWordListName) {
            if (newWordListName.length === 0) {
                return 'Tomt namn är inte tillåtet';
            }

            if (newWordListName.length > 1024) {
                return 'Namn får vara högst 1024 tecken';
            }

            return wordListService.updateName($ctrl.model.wordListId, newWordListName);
        }
    }
})();