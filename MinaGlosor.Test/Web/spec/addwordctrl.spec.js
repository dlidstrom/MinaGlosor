describe('AddWordCtrl', function () {
    'use strict';

    var ctrl;
    var scope;
    var wordService;
    var rootScope;
    var returnResolvedDeferred;
    var returnRejectedDeferred;

    beforeEach(function () {
        module('App');
    });

    beforeEach(inject(function ($controller, $q, $rootScope) {
        rootScope = $rootScope;
        scope = {};

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
        var errorHandler = function () {
        };
        var wordList = {
            id: 2,
            name: 'word list name'
        };
        ctrl = $controller(
            'AddWordCtrl',
            {
                $scope: scope,
                ErrorHandler: errorHandler,
                WordList: wordList,
                WordService: wordService
            });
    }));

    describe('initialize', function () {
        it('should set wordList', function () {
            expect(scope.wordList.name).toEqual('word list name');
        });

        it('should set wordList.id', function () {
            expect(scope.wordList.id).toBe(2);
        });

        it('should set wordList.name', function () {
            expect(scope.wordList.name).toEqual('word list name');
        });
    });

    describe('add', function () {
        describe('success', function () {
            beforeEach(function () {
                scope.form = {
                    $pristine: false,
                    $setPristine: function () {
                        this.$pristine = true;
                    }
                };
                scope.entry = { word: 'Some word', definition: 'Some def' };
                scope.add(scope.entry);

                // resolve all deferreds
                rootScope.$apply();
            });

            it('should reset saving flag', function () {
                expect(scope.saving).toBe(false);
            });

            it('should reset entry', function () {
                expect(scope.entry).toBeNull();
            });

            it('should set form to pristine', function () {
                expect(scope.form.$pristine).toBe(true);
            });
        });

        describe('failure', function () {
            beforeEach(function () {
                wordService.create = returnRejectedDeferred;
                scope.entry = { word: 'Some word', definition: 'Some def' };
                scope.add(scope.entry);

                // resolve all deferreds
                rootScope.$apply();
            });

            it('should reset saving flag', function () {
                expect(scope.saving).toBe(false);
            });

            it('should not reset entry', function () {
                expect(scope.entry).not.toBeNull();
            });
        });
    });
});