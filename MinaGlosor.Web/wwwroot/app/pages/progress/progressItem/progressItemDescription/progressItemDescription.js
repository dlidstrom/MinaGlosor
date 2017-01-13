(function () {
    'use strict';

    angular.module('pages.progress')
        .component(
            'progressItemDescription',
            {
                bindings: {
                    item: '<'
                },
                controller: 'ProgressItemDescriptionController',
                templateUrl: '/wwwroot/app/pages/progress/progressItem/progressItemDescription/progressItemDescription.html'
            })
        .controller(
            'ProgressItemDescriptionController',
            ProgressItemDescriptionController);

    function ProgressItemDescriptionController() {
        var $ctrl = this;

        $ctrl.getDescription = getDescription;

        function getDescription(item) {
            var result;
            if (item.percentDone === 0) {
                result = 'Empty';
            }
            else if (item.percentDone < 100) {
                if (item.percentExpired > 0) {
                    result = 'LearnNewBeforeRepeat';
                } else {
                    result = 'LearnNew';
                }
            } else if (item.percentExpired > 0) {
                result = 'Repeat';
            } else {
                result = 'Done';
            }

            return result;
        }
    }
})();