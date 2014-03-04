// @reference config.js
// @reference app.js
(function (app) {
    // allow non-strict mode here, to be able to capture more detail
    //'use strict';

    if (!app) throw new Error('Expected app to be defined');

    function formatMsg(ex, stack) {
        if (ex == null) return '';
        if (app.logErrorUrl == null) {
            alert('logErrorUrl must be defined.');
            return '';
        }
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
    };

    function logError(out) {
        // send error message
        $.ajax({
            type: 'POST',
            url: app.logErrorUrl,
            data: { message: out }
        });

        return out;
    };

    var cache = {};
    window.onerror = function (msg) {
        var out = formatMsg(msg);
        if (cache.hasOwnProperty(out) == false) {
            try {
                logError(out);
            } catch (e) {
            }

            cache[out] = true;
        }
    };
})(window.App);