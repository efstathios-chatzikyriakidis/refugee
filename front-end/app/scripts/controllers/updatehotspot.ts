/// <reference path="../app.ts" />

'use strict';

module refugeeApp {
  export interface IUpdatehotspotScope extends ng.IScope {
    awesomeThings: any[];
  }

  export class UpdatehotspotCtrl {

    constructor (private $scope: IUpdatehotspotScope) {
      $scope.awesomeThings = [
        'HTML5 Boilerplate',
        'AngularJS',
        'Karma'
      ];
    }
  }
}

angular.module('refugeeApp')
  .controller('UpdatehotspotCtrl', refugeeApp.UpdatehotspotCtrl);
