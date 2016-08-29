describe('WordListProgressService', function () {
    'use strict';

    var httpMock;
    var wordListProgressService;

    beforeEach(module('mgApp'));

    beforeEach(inject(function ($httpBackend) {
        httpMock = $httpBackend;
    }));

    beforeEach(inject(['WordListProgressService', function (service) {
        wordListProgressService = service;
    }]));

    it('should inject service', function () {
        expect(wordListProgressService).toBeDefined();
    });

    describe('api', function () {
        it('should get word list progresses', function () {
            var result = {
                wordLists: [
                    {
                        wordListId: "1",
                        ownerId: "1",
                        name: "Some name",
                        numberOfWords: 2
                    }
                ],
                numberOfFavourites: 1
            };
            httpMock.expectGET('/api/wordlistprogress').respond(200, result);
            wordListProgressService.getAll();
        });

        afterEach(function () {
            httpMock.flush();
            httpMock.verifyNoOutstandingExpectation();
            httpMock.verifyNoOutstandingRequest();
        });
    });
});