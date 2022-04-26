// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:5000/notiHub").build();

//Disable send button until connection is established

connection.on("ReceiveMessage", function (itemname, price) {
  
    $('#toast_div').toast('show');
});

connection.start().then(function () {
   console.log("connection connected");
}).catch(function (err) {
    return console.error(err.toString());
});

