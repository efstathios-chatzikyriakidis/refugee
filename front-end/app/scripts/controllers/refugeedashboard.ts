/// <reference path="../app.ts" />
/// <reference path="../services/refugeerepository.ts" />
/// <reference path="../services/hotspotrepository.ts" />

'use strict';

module refugeeApp {
  export interface IRefugeedashboardScope extends ng.IScope {
    
  }

  export class RefugeedashboardCtrl {
      repository: Refugeerepository;
    
      hotspotRepository: Hotspotrepository;  
      

    public refugees: any; 
      
    public hotspots: any;
      
    public filteredRefugees: any;
    
    public selectedRefugeeIndex: number = -1;
    
    public selectedRefugee: any;
      
    public tmpRefugee: any;
      
    public editing: boolean = false;
      
    public growl: any;  
      
    public $filter: ng.IFilterService;  
    
    public filterText: string;  
      
    public newRefugee: boolean = false;
    
    public config: any ={
            itemsPerPage: 8,
            maxPages: 5,
            fillLastPage: true
          };
      
constructor ($scope: IRefugeedashboardScope, repository: Refugeerepository,hotspotRepository: Hotspotrepository, growl: any, $filter:ng.IFilterService) {
            this.repository= repository;
            this.growl = growl;
            this.$filter = $filter;
            this.hotspotRepository = hotspotRepository;
            this.initData();
        }

        getRefugess(){
            this.repository.getAll().then((result)=>{
                this.refugees = result.data;
                this.refugeeManip();
                this.filteredRefugees = angular.copy(this.refugees);
            },(error)=>{
                this.growl.error("Error while fetching refugees", {ttl: 5000});
            });
        }
      
        getHotspots(){
            this.hotspotRepository.getAll().then((result)=>{
                this.hotspots = result.data;
            },(error)=>{
                this.growl.error("Error while fetching hotspots", {ttl: 5000});
            });
        }
      
        refugeeManip(){
            angular.forEach(this.refugees,(refugee,index)=>{
                refugee.index = index;    
            });
        }

        openRefugee(refugeeIndex: number){
            this.selectedRefugeeIndex = refugeeIndex;
            this.selectedRefugee = this.refugees[refugeeIndex];
            this.tmpRefugee = angular.copy(this.selectedRefugee);
        }
      
        createRefugee(){
            this.newRefugee = true;
            this.editing = true;
            this.selectedRefugee = {};
            this.growl.info("Type in refugee info", {ttl: 5000});
        }
      
        makeCreate(){
            this.selectedRefugee.hotSpotId = this.selectedRefugee.hotSpot.id;
            delete this.selectedRefugee.hotSpot;
            this.repository.createSingle(this.selectedRefugee).then((result)=>{
                this.getRefugess();
                this.growl.success("Refugee created!", {ttl: 2000});
            },(error)=>{
                this.growl.error("Error while creating refugee", {ttl: 5000});
            });
            this.editing = false;
            this.newRefugee = false;
        }
      
        saveRefugee(){
            this.selectedRefugee.hotSpotId = this.selectedRefugee.hotSpot.id;
            delete this.selectedRefugee.hotSpot;
            this.repository.updateSingle(this.selectedRefugee).then((result)=>{
                this.getRefugess();
                this.growl.success("Refugee updated!", {ttl: 2000});
            },(error)=>{
                this.growl.error("Error while updating refugees", {ttl: 5000});
            });
            this.editing = false;
        }
      
        updateFilter(){
            this.filteredRefugees = this.$filter("filter")(this.refugees, this.filterText);
        }
      
        closeRefugee(){
            this.selectedRefugeeIndex = -1;
            this.newRefugee = false;
        }
      
        initData(){
            this.getRefugess();
            this.getHotspots();
        }
      }
    }

angular.module('refugeeApp')
    .controller('RefugeedashboardCtrl', ['$scope','refugeeRepository','hotspotRepository','growl','$filter',refugeeApp.RefugeedashboardCtrl])
  .filter('propsFilter', function() {
  return function(items, props) {
    var out = [];

    if (angular.isArray(items)) {
      var keys = Object.keys(props);
        
      items.forEach(function(item) {
        var itemMatches = false;

        for (var i = 0; i < keys.length; i++) {
          var prop = keys[i];
          var text = props[prop].toLowerCase();
          if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
            itemMatches = true;
            break;
          }
        }

        if (itemMatches) {
          out.push(item);
        }
      });
    } else {
      // Let the output be the input untouched
      out = items;
    }

    return out;
  };
});;
