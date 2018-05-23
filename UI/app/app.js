'use strict';

var app = angular.module('myApp', [
    'ngRoute',
    'ngResource'
]);
app.config(function ($routeProvider) {
    $routeProvider.when('/lotsView', { templateUrl: 'app/lotsView.html', controller: 'lotsView' });
    $routeProvider.when('/signIn', { templateUrl: 'app/signIn.html', controller: 'signIn' });
    $routeProvider.when('/register', { templateUrl: 'app/register.html', controller: 'register' });
    $routeProvider.when('/manager', { templateUrl: 'app/managerMenu.html', controller: 'managerMenu' });
    $routeProvider.when('/admin', { templateUrl: 'app/adminMenu.html', controller: 'adminMenu' });
    $routeProvider.when('/lotCreate', { templateUrl: 'app/lotCreate.html', controller: 'lotCreate' });
    $routeProvider.otherwise({ redirectTo: '/signIn' });
});