/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var copy = require('gulp-copy');

gulp.task('styles', function () {
    gulp.src(['bower_components/bootstrap/dist/css/bootstrap.min.css']).pipe(copy('wwwroot/styles', { prefix: 4 }));
});