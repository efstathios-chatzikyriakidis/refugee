/// <reference path="../app.ts" />
/// <reference path="../services/refugeerepository.ts" />
/// <reference path="../dtos/refugeeDto.ts" />
/// <reference path="../models/refugee.ts" />
                 
'use strict';

module refugeeApp {
  export interface IRelationsScope extends ng.IScope {
    awesomeThings: any[];
  }

  
    
  export class RelationsCtrl {
    
    repository: Refugeerepository;
    
    static $inject = ['$scope','refugeeRepository'];
    
    public degreeTypes: any;
    
    public tmpDegree: number;  
      
    public refugeeRelation:   any;
    
    public refugees: any;
    
      
constructor (private $scope: IRelationsScope, private  refugeeRepository: Refugeerepository) {
        this.repository = refugeeRepository;
        this.initData();
        
    }
    
    public degreeManip(){
        this.degreeTypes = [{name: 'Parent', value:0},
            {name: 'Children', value:1},
            {name: 'Cousin', value:2},
            {name: 'Grandparent', value:3},
            {name: 'Spouse', value:4},
            {name: 'Siblings', value:5}];
    }
      
    relationManip(){
        return <RefugeeDto.RefugeeRelate> {sourceId: this.refugeeRelation.sourceId.id, targetId: this.refugeeRelation.targetId.id, relationshipDegree: this.refugeeRelation.relationshipDegree
        }
    }
      
    getRefugess(){
        this.repository.getAll().then((result)=>{
            this.refugees = result.data;
        });
    }  
      
    public setDegree(){
this.refugeeRelation.relationshipDegree = this.tmpDegree;
    }
      
    public relate(){
        var dto = this.relationManip();
        this.repository.relateRefugees(dto);
    }
      
    public initData(){
        this.refugeeRelation=<any>{};
        this.degreeManip();
        this.getRefugess();
    }
      
  }
}

angular.module('refugeeApp')
  .controller('RelationsCtrl', refugeeApp.RelationsCtrl);
