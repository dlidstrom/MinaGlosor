describe('WordListService', function () {
    'use strict';

    var httpMock;
    var wordListService;

    beforeEach(module('mgApp'));

    beforeEach(module(function ($urlRouterProvider) {
        $urlRouterProvider.deferIntercept();
    }));

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
        it('should get by id', function () {
            httpMock.expectGET(
                '/api/wordlist?wordListId=1')
                .respond(200, { wordListId: '1', ownerId: '1', name: 'Some name', numberOfWords: 2 });
            wordListService.getById('1');
        });

        it('should create word list', function () {
            httpMock.expectPOST('/api/wordlist', { name: 'name' }).respond(201);
            wordListService.create('name');
        });

        afterEach(function () {
            httpMock.flush();
            httpMock.verifyNoOutstandingExpectation();
            httpMock.verifyNoOutstandingRequest();
        });
    });
});