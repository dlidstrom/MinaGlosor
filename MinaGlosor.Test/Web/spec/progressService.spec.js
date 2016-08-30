describe('ProgressService', function () {
    'use strict';

    var httpMock;
    var progressService;

    beforeEach(module('mgApp'));

    beforeEach(inject(function ($httpBackend) {
        httpMock = $httpBackend;
    }));

    beforeEach(inject(['ProgressService', function (service) {
        progressService = service;
    }]));

    it('should inject service', function () {
        expect(progressService).toBeDefined();
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
            httpMock.expectGET('/api/progress?page=1').respond(200, result);
            progressService.getAll();
        });

        afterEach(function () {
            httpMock.flush();
            httpMock.verifyNoOutstandingExpectation();
            httpMock.verifyNoOutstandingRequest();
        });
    });
});