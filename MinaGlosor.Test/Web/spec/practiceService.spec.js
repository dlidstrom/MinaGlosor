describe('PracticeService', function () {
    'use strict';

    var httpMock;
    var rootScope;
    var practiceService;

    beforeEach(module('mgApp'));

    beforeEach(inject(function ($httpBackend, $rootScope) {
        httpMock = $httpBackend;
        rootScope = $rootScope;
    }));

    beforeEach(inject(['PracticeService', function (service) {
        practiceService = service;
    }]));

    it('should inject service', function () {
        expect(practiceService).toBeDefined();
    });

    describe('api', function () {
        describe('start new session', function () {
            var practiceSessionId;
            beforeEach(function () {
                httpMock.expectPOST(
                    '/api/practicesession',
                    { wordListId: '1' })
                    .respond(201, { practiceSessionId: '1', wordText: 'ari', wordDefinition: 'ja' });
                practiceService.create('1').then(function (data) {
                    practiceSessionId = data;
                });
                rootScope.$apply();
                httpMock.flush();
            });

            it('should return practiceSessionId', function () {
                expect(practiceSessionId).toBe('1');
            });
        });

        describe('get unfinished', function () {
            var unfinished;
            beforeEach(function () {
                httpMock.expectGET('/api/unfinishedpracticesession?wordListId=2')
                    .respond(200, [{ practiceSessionId: '1', createdDate: '2012-01-01T00:00:00' }]);
                practiceService.getUnfinished('2').then(function (data) { unfinished = data; });
                rootScope.$apply();
                httpMock.flush();
            });

            it('should return unfinished sessions', function () {
                expect(unfinished.length).toBe(1);
                expect(unfinished[0].practiceSessionId).toBe('1');
                expect(unfinished[0].createdDate).toBe('2012-01-01T00:00:00');
            });
        });

        afterEach(function () {
            httpMock.verifyNoOutstandingExpectation();
            httpMock.verifyNoOutstandingRequest();
        });
    });
});