///<reference path="../models/hotspot.ts"/>
///<reference path="../models/refugee.ts"/>

module RefugeeDto{
    export class RefugeeDtoBasic extends Refugee.RefugeeModel{
        hotspot: Hotspot.HotspotModel;
    }
    
    export class RefugeeDashboardDto{
        name: string;
        
    }
    
    export enum RelationDegree{
        Parent,

        Children,

        Cousin,

        Grandparent,

        Spouse,

        Siblings
    }
    
    export class RefugeeRelate{
        sourceId: string;   
        
        targetId: string;
        
        relationshipDegree: number;
        
constructor(){ this.sourceId=''; this.targetId=''; this.relationshipDegree=-1;}
    }
}