﻿let time = new Date();
let year = time.getFullYear();
document.getElementById("rights").innerHTML = " CINEL | UFCD 5417 - Programação para a WEB - servidor (server-side) | 25853";


var btn = document.querySelector("#refresh");
btn.addEventListener("click", function () {

    location.reload();
});