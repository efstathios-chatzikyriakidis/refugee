///<reference path="../models/hotspot.ts"/>
///<reference path="../models/refugee.ts"/>

module HotspotDto{
    export class HotspotDtoBasic extends Hotspot.HotspotModel{
        refugees: Refugee.RefugeeModel[];
    }
    
    export class HotspotDashboardDto{
        name: string;
        
    }
}