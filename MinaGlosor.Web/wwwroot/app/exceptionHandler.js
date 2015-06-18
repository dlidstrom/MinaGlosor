(function () {
    //'use strict'; allow default mode here, to be able to capture more detail

    angular.module('mgApp')
        .config(ExceptionHandlerDecorator);

    ExceptionHandlerDecorator.$inject = ['$provide'];

    function ExceptionHandlerDecorator($provide) {
        $provide.decorator('$exceptionHandler', ExceptionHandler);
    }

    ExceptionHandler.$inject = ['$delegate', '$injector'];
    function ExceptionHandler($delegate, $injector) {
        var apiUrl = '/api/logerror';
        // avoid sending a bunch of the same error
        var cache = {};
        return function (exception, cause) {
            // original implementation
            $delegate(exception, cause);

            var out = FormatMsg(exception);
            if (cache.hasOwnProperty(out) === false) {
                try {
                    // service locator pattern...
                    var $http = $injector.get('$http');
                    $http.post(
                        apiUrl,
                        {
                            message: out
                        });
                } catch (e) {
                }

                cache[out] = true;
            }
        };
    }

    function FormatMsg(ex, stack) {
        if (ex == null) return '';
        var url = ex.fileName != null ? ex.fileName : document.location;
        if (stack == null && ex.stack != null) stack = ex.stack;

        // format output
        var out = ex.message != null ? ex.name + ": " + ex.message : ex;
        out += ": at document path '" + url + "'.";
        if (stack != null) {
            try {
                // not sure about the stack property, best be careful...
                out += "\n  at " + stack;
            } catch (e) {
            }
        }

        return out;
    }
})();