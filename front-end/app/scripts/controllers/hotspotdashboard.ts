/// <reference path="../app.ts" />
/// <reference path="../services/hotspotrepository.ts"/>
'use strict';

module refugeeApp {
  export interface IHotspotdashboardScope extends ng.IScope {
    awesomeThings: any[];
  }

    export class HotspotdashboardCtrl {

        repository: Hotspotrepository;

        public hotspots: any;
        
        public filteredHotspots: any;
        
        public filterByName: string;
        
        public $filter: ng.IFilterService;
        
        public selectedHotspotIndex: number = -1;
    
        public selectedHotspot: any;

        public editing: boolean = false;

        public newHotspot: boolean = false;
        
        public tmpHotspot: any;
        
        public growl: any;
        
        public config: any ={
            itemsPerPage: 8,
            maxPages: 5,
            fillLastPage: true
          };
        
constructor ($scope: IHotspotdashboardScope, repository: Hotspotrepository, growl:any, $filter: ng.IFilterService) {
                this.repository = repository;
                this.growl = growl;
                this.$filter = $filter;
                this.getHotspots();
        }

        getHotspots(){
            this.repository.getAll().then((result)=>{
                this.hotspots = result.data;
                this.hotspotManipulate();
                this.filteredHotspots = angular.copy(this.hotspots);
            },(error)=>{ 
                this.growl.error("Error while fetching hotspots", {ttl: 5000});
            });
        }

        hotspotManipulate(){
            angular.forEach(this.hotspots,(hotspot,key)=>{
                //Create hotspot location url 
                hotspot.locationUrl = "http://maps.google.com/maps?q="+hotspot.latitude+","+hotspot.longitude;
                hotspot.index = key;
                
            });
        }

    
        openHotspot(hotspotIndex: number){
            this.selectedHotspotIndex = hotspotIndex;
            this.selectedHotspot = this.hotspots[hotspotIndex];
            this.tmpHotspot = angular.copy(this.selectedHotspot);
        }
      
        createHotspot(){
            this.newHotspot = true;
            this.editing = true;
            this.selectedHotspot = {};
            this.growl.info("Type in hotspot info!", {ttl: 2000});
        }
      
        makeCreate(){
            this.selectedHotspot.hotSpotId = this.selectedHotspot.id;
            this.repository.createSingle(this.selectedHotspot).then((result)=>{
                this.getHotspots();
                this.growl.success("Hotspot created!", {ttl: 2000});
            },(error)=>{this.growl.error("Hotspots create failed. Check input!!", {ttl: 2000});});
            this.editing = false;
            this.newHotspot = false;
        }
      
        updateFilter(){
            this.filteredHotspots = this.$filter("filter")(this.hotspots, this.filterByName);
        }
        
        saveHotspot(){
            this.selectedHotspot.hotSpotId = this.selectedHotspot.id;
            this.repository.updateSingle(this.selectedHotspot).then((result)=>{
                this.getHotspots();
                this.growl.success("Hotspot updated!", {ttl: 2000});
            },(error)=>{this.growl.error("Hotspots update failed. Check input!", {ttl: 2000});});
            this.editing = false;
        }
      
        closeHotspot(){
            this.selectedHotspotIndex = -1;
            this.newHotspot = false;
        }
 
    }
}

angular.module('refugeeApp')
    .controller('HotspotdashboardCtrl', ['$scope','hotspotRepository','growl','$filter',refugeeApp.HotspotdashboardCtrl]);
