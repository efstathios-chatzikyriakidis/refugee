/// <reference path="../app.ts" />
/// <reference path="./basiccalls.ts"/>
/// <reference path="./httpcaller.ts"/>
/// <reference path="../dtos/refugeeDto.ts"/>

'use strict';

module refugeeApp {
  export class Refugeerepository implements IBasicCalls<RefugeeDto.RefugeeDtoBasic> {
    
    caller: Httpcaller = null;
    
    static $inject = ['httpCaller'];
      
    constructor(httpCaller: Httpcaller){
            this.caller = httpCaller;    
    }
    
    getAll():ng.IHttpPromise<RefugeeDto.RefugeeDtoBasic>{
        var path: string = "refugees";
        
        return this.caller.makeCall(CallType.GET,path,{});
    }
      
    getSingle(identifier: string):ng.IHttpPromise<RefugeeDto.RefugeeDtoBasic>{
            
        var path: string = "refugees/"+identifier;
            
        return this.caller.makeCall(CallType.GET,path,{});
    }
        
deleteSingle( identifier: string ): void{ 

            var path: string= "refugees/"+identifier;
        
            this.caller.makeCall(CallType.DELETE,path,{});
        }

        updateSingle(entity:RefugeeDto.RefugeeDtoBasic):ng.IHttpPromise<RefugeeDto.RefugeeDtoBasic>{
            
            var path: string= "refugees/"+entity.id;
        
            return  this.caller.makeCall(CallType.PUT,path,entity);
            
        };
        
        createSingle(entity:RefugeeDto.RefugeeDtoBasic):ng.IHttpPromise<RefugeeDto.RefugeeDtoBasic>{
            var path: string = "refugees/";
            
            return this.caller.makeCall(CallType.POST,path,entity);
    
        }
      
        relateRefugees(entity: RefugeeDto.RefugeeRelate):ng.IHttpPromise<RefugeeDto.RefugeeRelate>{
            var path: string = "refugees/relationships/family";
            
            return this.caller.makeCall(CallType.POST,path,entity) ; 

        }
        refugeesNoFamily(){
            var path: string = "/refugees/adultsWithNoFamily/all";
            
            return this.caller.makeCall(CallType.GET,path,{});
        }
      
        refugeesUnderage(){
            var path: string = "/refugees/underage/all";
            
            return this.caller.makeCall(CallType.GET,path,{});
        }
      
        refugeeWithChildGraphPath(){
            return "familiesWithChildren/graph";
        }
      
        refugeeRelationsGraphPath(){
            return "/relationships/graph";
        }
      
      getRelationships(entity: RefugeeDto.RefugeeRelate):ng.IHttpPromise<RefugeeDto.RefugeeRelate>{
            var path: string = "refugees/"+entity.sourceId+"/family";
            
            return this.caller.makeCall(CallType.GET,path,entity) ; 

        }
  }
}

angular.module('refugeeApp')
  .service('refugeeRepository', refugeeApp.Refugeerepository);
