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
            var result = null;
            if (item.percentDone < 100) {
                if (item.percentExpired > 0) {
                    //description = 'Du har fler nya ord att öva på innan du repeterar tidigare ord.';
                    result = 'LearnNewBeforeRepeat';
                } else {
                    //description = 'Du har fler nya ord att öva på.';
                    result = 'LearnNew';
                }
            } else {
                if (item.percentExpired > 0) {
                    result = 'Repeat';
                    //description = item.percentExpired + '% av dina ord behöver repeteras.';
                } else if (item.percentDone === 100) {
                    result = 'Done';
                    //description = 'Du har gjort klart ' + item.percentDone + '%. För tillfället har du inget att öva på.';
                }
            }

            return result;
        }
    }
})();