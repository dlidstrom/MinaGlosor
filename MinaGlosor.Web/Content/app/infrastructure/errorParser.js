(function (app) {
    'use strict';

    app.factory(
        'ErrorParser',
        function () {
            var parseError = function (response) {
                var data = {
                    modelState: [],
                    message: response.Message
                };

                if (response.exceptionMessage) {
                    // does this ever happen?
                    angular.extend(data, {
                        exceptionMessage: response.exceptionMessage,
                        exceptionType: response.exceptionType,
                        stackTrace: response.stackTrace,
                        message: response.message
                    });
                } else if (response.data) {
                    angular.extend(data, {
                        exceptionMessage: response.data.exceptionMessage,
                        exceptionType: response.data.exceptionType,
                        stackTrace: response.data.stackTrace,
                        message: response.data.message
                    });
                }

                // convert model state into something usable
                if (response.modelState || (response.data && (response.modelState = response.data.modelState))) {
                    var modelState = response.modelState;
                    for (var s in modelState) {
                        var state = {
                            field: s.replace('criteria.', ''),
                            reasons: modelState[s].join(', ')
                        };
                        data.modelState.push(state);
                    }
                }

                return data;
            };

            return {
                parse: parseError
            };
        });
})(window.App)