// @reference app.js
(function (app) {
    'use strict';

    app.factory(
        '$exceptionHandler',
        [
            '$window',
            '$log',
            function ($window, $log) {
                return function (exception) {
                    $window.onerror(exception);
                    if (exception.name && exception.message) {
                        $log.error(exception.name + ': ' + exception.message);
                    } else if (exception) {
                        $log.error(exception);
                    } else {
                        $log.error('Unhandled exception');
                    }
                };
            }]);
})(window.App);