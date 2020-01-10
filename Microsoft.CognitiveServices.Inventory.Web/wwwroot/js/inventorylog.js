"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/inventoryLogHub").build();
var inventoryTableBody = document.getElementById('inventoryTableBody');

//Disable send and reset buttons until connection is established.
document.getElementById("sendButton").disabled = true;
document.getElementById("resetButton").disabled = true;

connection.start().then(function () {
    disableWebClientSendMessageControls();
    displayInventoryItemsInTable();
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveMessage", function (user, message) {
    // Clean up and format the message so it displays nicely.
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var logMsg = user + ": " + msg;
    displayLogMessage(logMsg);
    clearInventoryTableBody();
    displayInventoryItemsInTable();
});

connection.on("Reset", function () {
    clearLogMessages();
    displayInventoryItemsInTable();
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("resetButton").addEventListener("click", function (event) {
    connection.invoke("Reset").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function enableWebClientSendMessageControls() {
    document.getElementById("sendButton").disabled = false;
    document.getElementById("resetButton").disabled = false;
}

function disableWebClientSendMessageControls() {
    document.getElementById("resetButton").disabled = false;
    document.getElementById("sendButton").style = "display: none;";
    document.getElementById("userInput").disabled = true;
    document.getElementById("userInput").style = "display:none;";
    document.getElementById("messageInput").disabled = true;
    document.getElementById("messageInput").style = "display:none;";
    document.getElementById("userLabel").style = "display: none;";
    document.getElementById("userLabel").disabled = true;
    document.getElementById("messageLabel").style = "display: none;";
    document.getElementById("messageLabel").disabled = true;
}

function displayInventoryItemsInTable() {
    connection.invoke("GetAllItems").then(function (items) {
        for (var i = 0; i < items.length; i++) {
            buildInventoryTableRow(items[i]);
        }
    }).catch(function (err) {
        return console.error(err.toString());
    });
}

function buildInventoryTableRow(item) {
    var row = inventoryTableBody.insertRow(inventoryTableBody.length);
    row.id = item.name + "row";
    row.innerHTML =
        "<tr>" +
        "<td>" + item.name +
        "</td><td>" + item.remainingQuantity +
        "</td><td>" + item.quantityMade +
        "</td><td>" + item.quantitySlacked +
        "</td><td>" + item.quantityShrinked +
        "</td><td>" + item.quantityReceived +
        "</td><td>" + item.totalQuantity +
        "</tr>";
    return row;
}

function displayLogMessage(logMsg) {
    var li = document.createElement("li");
    li.className = "list-group-item";
    li.textContent = logMsg;
    var list = document.getElementById("messagesList");
    list.insertBefore(li, list.childNodes[0]);
}

function clearLogMessages() {
    document.getElementById("messagesList").innerHTML = "";
}

function clearInventoryTableBody() {
    inventoryTableBody.innerHTML = "";
}