﻿describe('AddWordController', function () {
    'use strict';

    var ctrl;
    var rootScope;

    beforeEach(module('mgApp'));

    beforeEach(module(function ($urlRouterProvider) {
        $urlRouterProvider.deferIntercept();
    }));

    beforeEach(inject(function ($controller, $q, $rootScope) {
        rootScope = $rootScope;

        ctrl = $controller('ProgressItemDescriptionController');
    }));

    it('should inject controller', function () {
        expect(ctrl).toBeDefined();
    });

    describe('getDescription', function () {
        var result = null;

        describe('when empty list', function () {
            beforeEach(function () {
                result = ctrl.getDescription({percentDone: 0, percentExpired: 0});
            });

            it('should return "Empty"', function () {
                expect(result).toBe('Empty');
            });
        });

        describe('when incomplete list', function () {
            describe('without any expired', function () {
                beforeEach(function () {
                    result = ctrl.getDescription({percentDone: 50, percentExpired: 0});
                });

                it('should return "LearnNewBeforeRepeat"', function () {
                    expect(result).toBe('LearnNew');
                });
            });

            describe('with expired', function () {
                beforeEach(function () {
                    result = ctrl.getDescription({percentDone: 50, percentExpired: 10});
                });

                it('should return "LearnNewBeforeRepeat"', function () {
                    expect(result).toBe('LearnNewBeforeRepeat');
                });
            });
        });

        describe('when complete list', function () {
            describe('with some expired', function () {
                beforeEach(function () {
                    result = ctrl.getDescription({percentDone: 100, percentExpired: 10});
                });

                it('should return "Repeat"', function () {
                    expect(result).toBe('Repeat');
                });
            });

            describe('with all expired', function () {
                beforeEach(function () {
                    result = ctrl.getDescription({percentDone: 100, percentExpired: 100});
                });

                it('should return "RepeatAll"', function () {
                    expect(result).toBe('RepeatAll');
                });
            });

            describe('without expired', function () {
                beforeEach(function () {
                    result = ctrl.getDescription({percentDone: 100, percentExpired: 0});
                });

                it('should return "Done"', function () {
                    expect(result).toBe('Done');
                });
            });
        });
    });
});