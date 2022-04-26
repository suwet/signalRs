// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/demoStronglyTypeHub").build();


connection.on("recieveMessage", function (user,message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    console.log(user + "  "+message);
});


// register client method that can call from hub.
/*
connection.on("demoClientMethod", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    console.log("hub call client name is demoClientMethod and pass parameter is "+message);
});
*/
connection.start().then(function (client) {
   console.log("connect to hub.")
}).catch(function (err) {
    return console.error(err.toString());
});



/*

document.getElementById("sendButton").addEventListener("click", function (event) {

    let name = "kuker";
    connection.invoke("DemoCallFromClient", name)
    .then(function(data){
        console.log("return from hub method name is 'DemoCallFromClient' " + data);
    })
    .catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
*/