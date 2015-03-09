describe('routes', function () {
    'use strict';

    var route;

    beforeEach(module('mgApp'));

    beforeEach(inject(function ($route) {
        route = $route;
    }));

    it('should map default route', function () {
        expect(route.routes[null].redirectTo).toBe('/wordlist');
    });

    it('should map wordlist route', function () {
        expect(route.routes['/wordlist'].controller).toBe('WordListController');
    });

    it('should map add word', function () {
        expect(route.routes['/wordlist/:id/add'].controller).toBe('AddWordController');
    });
});