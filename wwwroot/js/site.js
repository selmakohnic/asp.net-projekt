// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";

//Visar och döljer hamburgarmeny
function toggleMenu() {
    $("#globalNav").toggle();
    $(".hamburgerMenu").toggleClass("hMenuClose");
}