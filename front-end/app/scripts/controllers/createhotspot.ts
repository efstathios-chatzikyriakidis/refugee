/// <reference path="../app.ts" />

'use strict';

module refugeeApp {
  export interface ICreatehotspotScope extends ng.IScope {
    awesomeThings: any[];
  }

  export class CreatehotspotCtrl {

    constructor (private $scope: ICreatehotspotScope) {
      $scope.awesomeThings = [
        'HTML5 Boilerplate',
        'AngularJS',
        'Karma'
      ];
    }
  }
}

angular.module('refugeeApp')
  .controller('CreatehotspotCtrl', refugeeApp.CreatehotspotCtrl);
