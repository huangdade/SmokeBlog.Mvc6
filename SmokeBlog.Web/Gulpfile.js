﻿/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var copy = require('gulp-copy');
var concat = require('gulp-concat');

gulp.task('libs', function () {
    gulp.src([
        'bower_components/jquery/dist/jquery.min.js',
        'bower_components/jquery/dist/jquery.min.map',
        'bower_components/bootstrap/dist/js/bootstrap.min.js',
        'bower_components/angularjs/angular.min.js',
        'bower_components/angularjs/angular.min.js.map',
        'bower_components/angular-route/angular-route.min.js',
        'bower_components/angular-route/angular-route.min.js.map',
        'bower_components/angular-messages/angular-messages.min.js',
        'bower_components/angular-messages/angular-messages.min.js.map',
        'bower_components/angular-bootstrap/ui-bootstrap.min.js',
        'bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js',
        'bower_components/lodash/lodash.min.js',
        'bower_components/messenger/build/js/messenger.min.js',
        'bower_components/jquery.validate/dist/jquery.validate.min.js',
        'bower_components/bootstrap-select/bootstrap-select.min.js',
        'bower_components/bootstrap-select/bootstrap-select.js.map',
        'bower_components/async/lib/async.js',
        'bower_components/bootstrap-tagsinput/dist/bootstrap-tagsinput.js'
    ]).pipe(gulp.dest('wwwroot/libs/'));

    gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/fontawesome/css/font-awesome.min.css',
        'bower_components/messenger/build/css/messenger.css',
        'bower_components/messenger/build/css/messenger-theme-future.css',
        'bower_components/bootstrap-select/bootstrap-select.min.css',
    ]).pipe(gulp.dest('wwwroot/css/'));

    gulp.src([
        'bower_components/fontawesome/fonts/*'
    ]).pipe(gulp.dest('wwwroot/fonts/'));
});

gulp.task('admin', function () {
    //concat css files
    gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/fontawesome/css/font-awesome.min.css',
        'bower_components/messenger/build/css/messenger.css',
        'bower_components/messenger/build/css/messenger-theme-future.css',        
        'bower_components/bootstrap-select/bootstrap-select.min.css',
        'bower_components/ng-tags-input/ng-tags-input.css',
        'wwwroot/styles/app.css'
    ]).pipe(concat('admin.css')).pipe(gulp.dest('wwwroot/css'));

    //concat javascript files
    gulp.src([
        'bower_components/jquery/dist/jquery.min.js',
        'bower_components/bootstrap/dist/js/bootstrap.min.js',
        'bower_components/angularjs/angular.min.js',
        'bower_components/angular-route/angular-route.min.js',
        'bower_components/angular-messages/angular-messages.min.js',
        'bower_components/angular-bootstrap/ui-bootstrap.min.js',
        'bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js',
        'bower_components/lodash/lodash.min.js',
        'bower_components/messenger/build/js/messenger.min.js',        
        'bower_components/bootstrap-select/bootstrap-select.min.js',
        'bower_components/async/lib/async.js',
        'bower_components/bootstrap-tagsinput/dist/bootstrap-tagsinput.js',
        'wwwroot/apps/*.js',
        'wwwroot/apps/directives/*.js',
        'wwwroot/apps/services/*.js',
        'wwwroot/apps/controllers/*.js'
    ]).pipe(concat('admin.js')).pipe(gulp.dest('wwwroot/js'));
});

gulp.task('account', function () {
    gulp.src([
        'bower_components/jquery/dist/jquery.min.js',
        'bower_components/jquery.validate/dist/jquery.validate.min.js'
    ]).pipe(concat('account.js')).pipe(gulp.dest('wwwroot/js'));

    gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'wwwroot/styles/account.css'
    ]).pipe(concat('account.css')).pipe(gulp.dest('wwwroot/css'));
});

gulp.task('all', ['libs', 'admin', 'account'], function () {

});