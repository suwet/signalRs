// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


connection.on("apple", function () {   
    console.log("calling apple method");
});

connection.onclose(function(){
    onDisconnected();
});

connection.start().then(onConnected).catch(function (err) {
    return console.error(err.toString());
});

function onConnected() {
    console.log('connected');
}

function onDisconnected() {
    console.log('disconnected');
}

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