/// <reference path="../app.ts" />
/// <reference path="../services/refugeerepository.ts"/>

'use strict';

module refugeeApp {
  export interface ISearchgraphScope extends ng.IScope {
    awesomeThings: any[];
  }

  export class SearchgraphCtrl {

    repository: Refugeerepository;
      
    public refugees: any;
      
    public refugeesFromPredefinedQuery: any;  
      
    public filteredRefugeesFromPredefinedQuery: any; 
      
    public filterText: string;  
      
    public rootScope: ng.IRootScopeService;  
    
    public growl: any;  
    
    public $filter: ng.IFilterService;
      
    public curGuid: any;
      
    public isGraph: number;
      
    public config: any ={
            itemsPerPage: 8,
            maxPages: 5,
            fillLastPage: true
          };
constructor (private $scope: ISearchgraphScope, repository:    Refugeerepository,$rootScope: ng.IRootScopeService, growl: any,filter: ng.IFilterService) {
        this.repository = repository;
        this.rootScope = $rootScope;
        this.$filter = filter;
        this.growl = growl;
        this.getRefugess();
        this.isGraph = -1;
    }
      
    updateGraph(){
        this.isGraph=1;
        this.rootScope.$broadcast('updateGraph',"/"+this.curGuid.id+"/"+this.repository.refugeeRelationsGraphPath());
    }
      
    getRefugess(){
        this.repository.getAll().then((result)=>{
            this.refugees = result.data;
        },(error)=>{
            this.growl.error("Error while fetching refugees", {ttl: 5000});
        });
    }
    
    getFamilyWithChild(){
        this.isGraph=1;
        this.rootScope.$broadcast('updateGraph',this.repository.refugeeWithChildGraphPath());
    }
      
    getNoFamily(){
        this.isGraph = 0;
        var dis = this;
        this.repository.refugeesNoFamily().then((result)=>{
            dis.refugeesFromPredefinedQuery = result.data;
            dis.filteredRefugeesFromPredefinedQuery =  angular.copy(this.refugeesFromPredefinedQuery);
        });
    }
      
    getUnderage(){
        this.isGraph = 0;
        var dis = this;
        this.repository.refugeesUnderage().then((result)=>{
            dis.refugeesFromPredefinedQuery = result.data;
            dis.filteredRefugeesFromPredefinedQuery =  angular.copy(this.refugeesFromPredefinedQuery);
        });
    }
      
    
    updateFilter(){
            this.filteredRefugeesFromPredefinedQuery = this.$filter("filter")(this.refugeesFromPredefinedQuery, this.filterText);
    }
      
  }
}

angular.module('refugeeApp')
.controller('SearchgraphCtrl',['$scope','refugeeRepository','$rootScope','growl','$filter', refugeeApp.SearchgraphCtrl]);
