/// <reference path="../app.ts" />

'use strict';

module refugeeApp {
    
    export enum CallType{
        GET,PUT,POST,DELETE   
    }
    export interface IBasicCaller{
        makeCall(type: CallType, path: string, data: any):any;
    }
    
    export class Httpcaller implements IBasicCaller{
        static $inject = ['$http'];
        
        // change to localhost if wanted. Be aware of CORS when other
        private IP_ADDRESS:string = '192.168.0.118';
        
        private rootPath = 'http://'+this.IP_ADDRESS+'/Refugee.Server/api/'
        
        http: ng.IHttpService;
        
        constructor(private $http: ng.IHttpService){
            this.http = $http;
        }
        
        public getAddress(): string{
            return this.IP_ADDRESS;
        }
        
        public makeCall(type: CallType,path: string, data: any): ng.IHttpPromise<{}>{
    
            var callType: string = CallType[type];
    
            var dis = this;
            
            return this.http({
                method: callType,
                url: dis.rootPath+path,
                data: data
            })
        }
    }
}

angular.module('refugeeApp')
  .service('httpCaller', refugeeApp.Httpcaller);
