/*
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
        'bower_components/jquery.validate/dist/jquery.validate.min.js'
    ]).pipe(gulp.dest('wwwroot/libs/'));

    gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/fontawesome/css/font-awesome.min.css',
        'bower_components/messenger/build/css/messenger.css',
        'bower_components/messenger/build/css/messenger-theme-future.css'
    ]).pipe(gulp.dest('wwwroot/styles/'));

    gulp.src([
        'bower_components/fontawesome/fonts/*'
    ]).pipe(gulp.dest('wwwroot/fonts/'));
});

gulp.task('image', function () {
    gulp.src([
        'Content/images/*'
    ]).pipe(gulp.dest('wwwroot/images/'));
});

gulp.task('admin', function () {
    //concat css files
    gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/fontawesome/css/font-awesome.min.css',
        'bower_components/messenger/build/css/messenger.css',
        'bower_components/messenger/build/css/messenger-theme-future.css',
        'Content/styles/app.css'
    ]).pipe(concat('admin.css')).pipe(gulp.dest('wwwroot/styles'));

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
        'Content/apps/*.js',
        'Content/apps/directives/*.js',
        'Content/apps/services/*.js',
        'Content/apps/controllers/*.js'
    ]).pipe(concat('admin.js')).pipe(gulp.dest('wwwroot/scripts'));

    //move template files
    gulp.src([
        'Content/apps/templates/*'
    ]).pipe(gulp.dest('wwwroot/templates'));

    //move css files for debug
    gulp.src([
        'Content/styles/app.css'
    ]).pipe(gulp.dest('wwwroot/styles/admin/'));

    //move javascript files for debug
    gulp.src('Content/apps/*.js').pipe(gulp.dest('wwwroot/scripts/admin'));
    gulp.src('Content/apps/directives/*.js').pipe(gulp.dest('wwwroot/scripts/admin/directives'));
    gulp.src('Content/apps/services/*.js').pipe(gulp.dest('wwwroot/scripts/admin/services'));
    gulp.src('Content/apps/controllers/*.js').pipe(gulp.dest('wwwroot/scripts/admin/controllers'));
});

gulp.task('account', function () {
    gulp.src('Content/styles/account.css').pipe(gulp.dest('wwwroot/styles/'));
});

gulp.task('all', ['libs', 'admin', 'account', 'image'], function () {

});