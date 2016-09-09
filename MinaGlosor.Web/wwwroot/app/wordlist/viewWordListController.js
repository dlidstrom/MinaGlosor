(function () {
    'use strict';

    angular.module('mgApp')
        .controller('ViewWordListController', ViewWordListController)
        .controller('ModalInstanceController', ModalInstanceController);

    ViewWordListController.$inject = ['$location', '$route', '$uibModal', 'wordListId', 'result'];
    function ViewWordListController($location, $route, $uibModal, wordListId, result) {
        var viewer = this;

        viewer.wordListId = wordListId;
        viewer.wordListName = result.wordListName;
        viewer.words = result.words;
        viewer.canPractice = result.words.length > 0; // TODO: Flytta till backend
        viewer.canAdd = result.canAdd;
        viewer.canEdit = result.canEdit;
        viewer.paging = result.paging;
        viewer.returnUrl = $location.url();

        viewer.validateName = validateName;
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
                        return viewer.wordListName;
                    }
                }
            });

            modalInstance.result.then(function () {
                $route.reload();
            });
        };

        function validateName(data) {
            if (data.length === 0) {
                return 'Tomt namn är inte tillåtet';
            }
        }
    }

    ModalInstanceController.$inject = ['$uibModalInstance', 'wordListName'];
    function ModalInstanceController($uibModalInstance, wordListName) {
        var modal = this;
        modal.wordListName = wordListName;

        modal.ok = function () {
            $uibModalInstance.close();
        };

        modal.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
})();