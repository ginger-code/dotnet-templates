'use strict';
const gulp = require('gulp');
const sass = require('gulp-sass')(require('sass'));
const concat = require('gulp-concat');
sass.compiler = require('node-sass');

gulp.task('sass', function () {
    return gulp.src('./wwwroot-src/scss/*.scss')
        .pipe(concat('GingerCodeSite.scss'))
        .pipe(sass().on('error', console.log))
        .pipe(gulp.dest('./wwwroot/css/'));
});