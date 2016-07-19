/// <reference path="../app.ts" />
/// <reference path="./basiccalls.ts"/>
/// <reference path="./httpcaller.ts"/>
/// <reference path="../dtos/hotspotDto.ts"/>

'use strict';

module refugeeApp {
    export class Hotspotrepository implements IBasicCalls<HotspotDto.HotspotDtoBasic> {
        
        caller: Httpcaller = null;
      
        static $inject = ['httpCaller'];
      
        constructor(httpCaller: Httpcaller){
            this.caller = httpCaller;    
        }
            
        getAll():ng.IHttpPromise<HotspotDto.HotspotDtoBasic>{
            var path: string = "hotspots";
            
            return this.caller.makeCall(CallType.GET,path,{});
        }
        
        getSingle(identifier: string):ng.IHttpPromise<HotspotDto.HotspotDtoBasic>{
            
            var path: string = "hotspots/"+identifier;
            
            return this.caller.makeCall(CallType.GET,path,{});
        }
        
deleteSingle( identifier: string ): void{ 

            var path: string= "hotspots/"+identifier;
        
            this.caller.makeCall(CallType.DELETE,path,{});
        }

        updateSingle(entity:HotspotDto.HotspotDtoBasic):ng.IHttpPromise<HotspotDto.HotspotDtoBasic>{
            
            var path: string= "hotspots/"+entity.id;
        
            return this.caller.makeCall(CallType.PUT,path,entity);
            
        };
        
        createSingle(entity:HotspotDto.HotspotDtoBasic):ng.IHttpPromise<HotspotDto.HotspotDtoBasic>{
            var path: string = "hotspots/";
            
            return this.caller.makeCall(CallType.POST,path,entity);
    
        }
    }
}

angular.module('refugeeApp')
  .service('hotspotRepository', refugeeApp.Hotspotrepository);
