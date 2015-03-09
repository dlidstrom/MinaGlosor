describe('PracticeWordService', function () {
    'use strict';

    var httpMock;
    var rootScope;
    var practiceWordService;

    beforeEach(module('mgApp'));

    beforeEach(inject(function ($httpBackend, $rootScope) {
        httpMock = $httpBackend;
        rootScope = $rootScope;
    }));

    beforeEach(inject(['PracticeWordService', function (service) {
        practiceWordService = service;
    }]));

    it('should inject service', function () {
        expect(practiceWordService).toBeDefined();
    });

    describe('api', function () {
        describe('get word', function () {
            var practiceWord;
            beforeEach(function () {
                httpMock.expectGET('/api/practiceword?practiceSessionId=1')
                    .respond(200, { text: 'ari', definition: 'ja' });
                practiceWordService.getNext('1').then(function (data) {
                    practiceWord = data;
                });

                rootScope.$apply();
                httpMock.flush();
            });

            it('should get word text', function () {
                expect(practiceWord.text).toBe('ari');
            });

            it('should get word definition', function () {
                expect(practiceWord.definition).toBe('ja');
            });
        });

        describe('submit', function () {
            it('should send response', function () {
                httpMock.expectPOST(
                    '/api/wordconfidence',
                    {
                        practiceSessionId: '1',
                        practiceWordId: 'abc123',
                        confidenceLevel: 'PerfectResponse'
                    }).respond(201, { isFinished: true });
                var practiceWord = {
                    practiceSessionId: '1',
                    practiceWordId: 'abc123'
                };
                practiceWordService.submit(practiceWord, 'PerfectResponse')
                    .then(function (data) {
                        expect(data.isFinished).toBe(true);
                    });
            });

            afterEach(function () {
                httpMock.flush();
            });
        });

        afterEach(function () {
            httpMock.verifyNoOutstandingExpectation();
            httpMock.verifyNoOutstandingRequest();
        });
    });
});