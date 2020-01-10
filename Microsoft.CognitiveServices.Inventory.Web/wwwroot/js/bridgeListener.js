"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/bridgeListenerHub").build();

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    var msgList = document.getElementById("messagesList");
    msgList.insertBefore(li, msgList.childNodes[0]);
    //document.getElementById("messagesList").appendChild(li);
});

connection.on("UpdateInventory", function (action, product, quantity) {
    //var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var msg = action + " " + quantity + " " + product + "."
    var encodedMsg = msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    var msgList = document.getElementById("messagesList");
    msgList.insertBefore(li, msgList.childNodes[0]);
    //document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    // do nothing
}).catch(function (err) {
    return console.error(err.toString());
});