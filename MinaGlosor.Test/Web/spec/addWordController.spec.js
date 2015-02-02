describe('AddWordController', function () {
    'use strict';

    var ctrl;
    var wordService;
    var rootScope;
    var returnResolvedDeferred;
    var returnRejectedDeferred;

    beforeEach(module('mgApp'));

    beforeEach(inject(function ($controller, $q, $rootScope) {
        rootScope = $rootScope;

        returnResolvedDeferred = function () {
            var deferred = $q.defer();
            deferred.resolve();
            return deferred.promise;
        };
        returnRejectedDeferred = function () {
            var deferred = $q.defer();
            deferred.reject();
            return deferred.promise;
        };
        wordService = {
            create: returnResolvedDeferred
        };
        //var errorHandler = function () {
        //};
        var wordList = {
            wordListId: 2,
            name: 'word list name'
        };
        ctrl = $controller(
            'AddWordController',
            {
                //ErrorHandler: errorHandler,
                WordList: wordList,
                WordService: wordService
            });
    }));

    it('should inject controller', function () {
        expect(ctrl).toBeDefined();
    });

    describe('initialize', function () {
        it('should set wordList', function () {
            expect(ctrl.wordList.name).toEqual('word list name');
        });

        it('should set wordList.wordListId', function () {
            expect(ctrl.wordList.wordListId).toBe(2);
        });

        it('should set wordList.name', function () {
            expect(ctrl.wordList.name).toEqual('word list name');
        });
    });

    describe('add', function () {
        var form;
        beforeEach(function () {
            form = {
                $pristine: false,
                $setPristine: function () {
                    this.$pristine = true;
                }
            };
        });

        describe('success', function () {
            var entry;
            beforeEach(function () {
                entry = { text: 'Some word', definition: 'Some def' };
                ctrl.add(form, entry);

                // resolve all deferreds
                rootScope.$apply();
            });

            it('should reset saving flag', function () {
                expect(ctrl.saving).toBe(false);
            });

            it('should reset entry text', function () {
                expect(entry.text).toBeNull();
            });

            it('should reset entry definition', function () {
                expect(entry.definition).toBeNull();
            });

            it('should set form to pristine', function () {
                expect(form.$pristine).toBe(true);
            });
        });

        describe('failure', function () {
            beforeEach(function () {
                wordService.create = returnRejectedDeferred;
                var entry = { word: 'Some word', definition: 'Some def' };
                ctrl.add(form, entry);

                // resolve all deferreds
                rootScope.$apply();
            });

            it('should reset saving flag', function () {
                expect(ctrl.saving).toBe(false);
            });

            it('should not reset entry', function () {
                expect(ctrl.entry).not.toBeNull();
            });
        });
    });
});