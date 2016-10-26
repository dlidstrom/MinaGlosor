(function () {
    'use strict';

    angular.module('pages.wordlist')
        .factory('PublishModal', PublishModal)
        .controller('PublishModalController', PublishModalController);

    PublishModal.$inject = ['$uibModal'];
    function PublishModal($uibModal) {
        var service = {
            publish: function (published, wordListName, size) {
                if (typeof published === 'undefined') {
                    throw 'Argument missing: "published"';
                }

                if (typeof wordListName === 'undefined') {
                    throw 'Argument missing: "wordListName"';
                }

                var modalInstance = $uibModal.open({
                    animation: false,
                    templateUrl: '/wwwroot/app/pages/wordlist/viewWordList/publishModal/publishModal.html',
                    controller: 'PublishModalController',
                    controllerAs: '$ctrl',
                    size: size,
                    resolve: {
                        wordListName: function () {
                            return wordListName;
                        },
                        published: published
                    }
                });

                return modalInstance.result;
            }
        };
        return service;
    }

    PublishModalController.$inject = ['$uibModalInstance', 'wordListName', 'published'];
    function PublishModalController($uibModalInstance, wordListName, published) {
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