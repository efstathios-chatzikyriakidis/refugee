/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="../../typings/angularjs/angular-route.d.ts" />
/// <reference path="./models/hotspot.ts"/>
/// <reference path="./models/refugee.ts"/>
'use strict';

angular.module('refugeeApp', [
    'ngAnimate',
    'ngCookies',
    'ngMessages',
    'ngResource',
    'ngRoute',
    'ngSanitize',
    'ngTouch',
    'angular-table',
    'ui.select',
    'ngSanitize',
    'angular-growl'
  ])
   
.config(($routeProvider:ng.route.IRouteProvider,$httpProvider:ng.IHttpProvider) => {
    $routeProvider
      .when('/', {
        templateUrl: 'views/main.html',
        controller: 'MainCtrl'
      })
      .when('/about', {
        templateUrl: 'views/about.html',
        controller: 'AboutCtrl',
        controllerAs: 'about'
      })
      .when('/refugeeDashboard', {
        templateUrl: 'views/refugeedashboard.html',
        controller: 'RefugeedashboardCtrl',
        controllerAs: 'rf'
      })
      .when('/searchGraph', {
        templateUrl: 'views/searchgraph.html',
        controller: 'SearchgraphCtrl',
        controllerAs: 'sv'
      })
      .when('/hotspotDashboard', {
        templateUrl: 'views/hotspotdashboard.html',
        controller: 'HotspotdashboardCtrl',
        controllerAs: 'ht'
      })
      .when('/relations', {
        templateUrl: 'views/relations.html',
        controller: 'RelationsCtrl',
        controllerAs: 'rl'
      })
      .otherwise({
        redirectTo: '/'
      });
      
    
  });
//.constant('_', window._);
