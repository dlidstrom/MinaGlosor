(function () {
    'use strict';

    angular.module('mgApp')
        .controller('ViewWordListController', ViewWordListController)
        .controller('ModalInstanceController', ModalInstanceController);

    ViewWordListController.$inject = [
        '$location',
        '$route',
        '$uibModal',
        'WordListService',
        'wordListId',
        'result'];
    function ViewWordListController(
        $location,
        $route,
        $uibModal,
        wordListService,
        wordListId,
        result) {
        var viewer = this;

        viewer.wordListId = wordListId;
        viewer.wordListName = result.wordListName;
        viewer.words = result.words;
        viewer.canPractice = result.words.length > 0; // TODO: Flytta till backend
        viewer.canAdd = result.canAdd;
        viewer.canEdit = result.canEdit;
        viewer.paging = result.paging;
        viewer.published = result.publishState === 'Published'; // TODO Fix with TypeScript
        viewer.returnUrl = $location.url();

        viewer.updateName = updateName;
        viewer.publish = publish;

        function publish(size) {
            var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceController',
                controllerAs: 'modal',
                size: size,
                resolve: {
                    wordListName: function () {
                        return result.wordListName;
                    },
                    published: result.published
                }
            });

            modalInstance.result.then(function (published) {
                wordListService.publish(wordListId, published).then(function () {
                    $route.reload(); // TODO: Fix with TypeScript + Redux
                });
            });
        };

        function updateName(data) {
            if (data.length === 0) {
                return 'Tomt namn är inte tillåtet';
            }
            return wordListService.updateName(wordListId, data);
        }
    }

    ModalInstanceController.$inject = ['$uibModalInstance', 'wordListName', 'published'];
    function ModalInstanceController($uibModalInstance, wordListName, published) {
        var modal = this;
        modal.wordListName = wordListName;
        modal.published = published;

        modal.publish = function () {
            $uibModalInstance.close(true);
        };

        modal.unpublish = function () {
            $uibModalInstance.close(false);
        };

        modal.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
})();