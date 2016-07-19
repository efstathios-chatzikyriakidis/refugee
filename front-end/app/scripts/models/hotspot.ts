module Hotspot{
    export class HotspotModel{
        id: string;
        name: string;
        long: string;
        lat: string;
        
        constructor( id: string, name: string, long: string, lat: string){
            this.id = id;
            this.name = name;
            this.long = long;
            this.lat = lat;
        }
        
        setPlace(long: string, lat: string): void{
            this.long = long;
            this.lat = lat;
        }
        
    }
}