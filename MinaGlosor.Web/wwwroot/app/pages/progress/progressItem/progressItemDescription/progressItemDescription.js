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
            var description;
            if (item.percentExpired > 0) {
                description = item.percentExpired + '% av dina ord behöver repeteras.';
            } else if (item.percentDone === 100) {
                description = 'Du har gjort klart ' + item.percentDone + '%. För tillfället har du inget att öva på.';
            } else {
                description = 'Du har gjort klart ' + item.percentDone + '%. Fortsätt öva tills du har når 100%.';
            }

            return description;
        }
    }
})();