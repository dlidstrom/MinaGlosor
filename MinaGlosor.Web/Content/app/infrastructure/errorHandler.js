// @reference app.js
(function (app) {
    'use strict';

    app.factory(
        'ErrorHandler',
        [
            'toaster',
            'ErrorParser',
            function (toaster, errorParser) {
                var errorHandler = function (response, opts) {
                    opts = opts || {};
                    opts.modelState = opts.modelState || modelStateErrors;
                    opts.exceptionMessage = opts.exceptionMessage || alert;
                    opts.message = opts.message || alert;
                    opts.status = opts.status || alert;
                    opts.unknown = opts.unknown || alert;

                    var error = errorParser.parse(response);
                    if (error.modelState && error.modelState.length) {
                        opts.modelState(error.modelState);
                    } else if (error.exceptionMessage) {
                        opts.exceptionMessage('Exception: ' + error.exceptionMessage);
                    } else if (error.message) {
                        opts.message('Error: ' + error.message);
                    } else if (response.status) {
                        opts.status('Internal Server Error: ' + response.status);
                    } else {
                        opts.unknown('Unknown error occurred');
                    }

                    function modelStateErrors(modelState) {
                        var dl = '<dl>';
                        for (var k in modelState) {
                            dl += '<dt>' + modelState[k].field + '</dt>';
                            dl += '<dd>' + modelState[k].reasons + '</dd>';
                        }

                        toaster.pop('warning', 'Fel', dl, 10000, 'trustedHtml');
                    }

                    function alert(text) {
                        toaster.pop('error', 'Internt fel uppstod', text);
                    }
                };
                return errorHandler;
            }]);
})(window.App);