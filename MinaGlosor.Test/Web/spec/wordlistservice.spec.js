describe('WordListService', function () {
    'use strict';

    var httpMock;
    var wordListService;

    beforeEach(function () {
        module('App');
    });

    beforeEach(inject(function ($httpBackend) {
        httpMock = $httpBackend;
    }));

    beforeEach(inject(['WordListService', function (service) {
        wordListService = service;
    }]));

    it('should inject service', function () {
        expect(wordListService).toBeDefined();
    });

    describe('api', function () {
        afterEach(function () {
            httpMock.flush();
            httpMock.verifyNoOutstandingExpectation();
            httpMock.verifyNoOutstandingRequest();
        });

        it('should get word lists', function () {
            httpMock.expectGET('/api/wordlist').respond(200, [{ id: 1, name: 'Some name', wordCount: 2 }]);
            wordListService.getAll();
        });

        it('should get by id', function () {
            httpMock.expectGET('/api/wordlist?id=1').respond(200, [{ id: 1, name: 'Some name', wordCount: 1 }]);
            wordListService.getById(1);
        });

        it('should create word list', function () {
            httpMock.expectPOST('/api/wordlist', { wordListName: 'name' }).respond(201);
            wordListService.create('name');
        });
    });
});