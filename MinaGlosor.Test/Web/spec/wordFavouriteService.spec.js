describe('WordFavouriteService', function () {
    'use strict';

    var httpMock;
    var wordFavouriteService;

    beforeEach(module('mgApp'));

    beforeEach(inject(function ($httpBackend) {
        httpMock = $httpBackend;
    }));

    beforeEach(inject(['WordFavouriteService', function (service) {
        wordFavouriteService = service;
    }]));

    it('should inject service', function () {
        expect(wordFavouriteService).toBeDefined();
    });

    describe('api', function () {
        afterEach(function () {
            httpMock.flush();
            httpMock.verifyNoOutstandingExpectation();
            httpMock.verifyNoOutstandingRequest();
        });

        it('should get all words', function () {
            httpMock.expectGET('/api/wordfavourite?page=1').respond(200);
            wordFavouriteService.getAll();
        });
    });
});