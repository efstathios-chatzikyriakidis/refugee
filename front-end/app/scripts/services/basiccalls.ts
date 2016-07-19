/// <reference path="../app.ts"/>
    
'use strict';

interface IBasicCalls<T>{
    getAll():ng.IHttpPromise<T>;
    getSingle(identifier: string):ng.IHttpPromise<T>;
    deleteSingle(identifier: string):void;
    updateSingle(entity:T):ng.IHttpPromise<T>;
    createSingle(entity:T):ng.IHttpPromise<T>;
}