/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var copy = require('gulp-copy');
var concat = require('gulp-concat');

gulp.task('admin', function () {
    var styleFiles = [
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/fontawesome/css/font-awesome.min.css',
        'bower_components/messenger/build/css/messenger.css',
        'bower_components/messenger/build/css/messenger-theme-future.css',
        'styles/app.css'
    ];

    gulp.src(styleFiles).pipe(gulp.dest('wwwroot/styles'));
    gulp.src(styleFiles).pipe(concat('app_all.css')).pipe(gulp.dest('wwwroot/styles'));

    var scriptFiles = [
        'bower_components/jquery/dist/jquery.min.js',
        'bower_components/bootstrap/dist/js/bootstrap.min.js',
        'bower_components/angularjs/angular.min.js',
        'bower_components/angular-route/angular-route.min.js',
        'bower_components/angular-messages/angular-messages.min.js',
        'bower_components/angular-bootstrap/ui-bootstrap.min.js',
        'bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js',
        'bower_components/lodash/lodash.min.js',
        'bower_components/messenger/build/js/messenger.min.js',
        'apps/*.js',
        'apps/directives/*.js',
        'apps/services/*.js',
        'apps/controllers/*.js'
    ]

    gulp.src(scriptFiles).pipe(gulp.dest('wwwroot/scripts'));
    gulp.src(scriptFiles).pipe(concat('app_all.js')).pipe(gulp.dest('wwwroot/scripts'));

    gulp.src([
        'apps/templates/*'
    ])
    .pipe(gulp.dest('wwwroot/templates'));

    gulp.src([
        'bower_components/fontawesome/fonts/*'
    ])
    .pipe(gulp.dest('wwwroot/fonts'));
});