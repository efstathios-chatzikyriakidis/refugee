module Refugee {
    
    export class RefugeeModel{
        id: string;
        name: string;
        birth: Date;
        
        constructor( id: string, name: string, birth: Date){
                this.id = id;
                this.name = name;
                this.birth = birth

        }
        
        
    }
    
    export enum RelationDegree{
        Parent,

        Children,
        
        Cousin,

        Grandparent,

        Spouse,

        Siblings
    }
    
}