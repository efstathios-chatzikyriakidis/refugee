/// <reference path="../app.ts" />

'use strict';

module refugeeApp {
  export class Authrepository {
    awesomeThings:any[] = [
      'HTML5 Boilerplate',
      'AngularJS',
      'Karma'
    ];
  }
}

angular.module('refugeeApp')
  .service('authRepository', refugeeApp.Authrepository);
