describe('WordService', function () {
    'use strict';

    var httpMock;
    var wordService;

    beforeEach(function () {
        module('App');
    });

    beforeEach(inject(function ($httpBackend) {
        httpMock = $httpBackend;
    }));

    beforeEach(inject(['WordService', function (service) {
        wordService = service;
    }]));

    it('should inject service', function () {
        expect(wordService).toBeDefined();
    });

    describe('api', function () {
        afterEach(function () {
            httpMock.flush();
            httpMock.verifyNoOutstandingExpectation();
            httpMock.verifyNoOutstandingRequest();
        });

        it('should create word', function () {
            httpMock.expectPOST('/api/word', { wordListId: 1, word: 'some word', definition: 'some def' }).respond(201);
            wordService.create(1, 'some word', 'some def');
        });

        it('should get all words', function () {
            httpMock.expectGET('/api/word?wordListId=1').respond(200);
            wordService.getAll(1);
        });
    });
});