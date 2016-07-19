/// <reference path="../app.ts" />

'use strict';

module refugeeApp {
  export interface IUpdaterefugeeScope extends ng.IScope {
    awesomeThings: any[];
  }

  export class UpdaterefugeeCtrl {

    constructor (private $scope: IUpdaterefugeeScope) {
      $scope.awesomeThings = [
        'HTML5 Boilerplate',
        'AngularJS',
        'Karma'
      ];
    }
  }
}

angular.module('refugeeApp')
  .controller('UpdaterefugeeCtrl', refugeeApp.UpdaterefugeeCtrl);
