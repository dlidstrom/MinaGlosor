describe('routes', function () {
    'use strict';

    var route;

    beforeEach(module('mgApp'));

    beforeEach(inject(function ($route) {
        route = $route;
    }));

    it('should map default route', function () {
        expect(route.routes[null].redirectTo).toBe('/progress');
    });

    it('should map wordlist route', function () {
        expect(route.routes['/progress'].controller).toBe('ProgressController');
    });

    it('should map add word', function () {
        expect(route.routes['/wordlist/:id/add'].controller).toBe('AddWordController');
    });
});