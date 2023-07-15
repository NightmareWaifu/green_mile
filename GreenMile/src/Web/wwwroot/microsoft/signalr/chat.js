"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Chathub").configureLogging(signalR.LogLevel.Information).build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true; 
connection.on("ReceiveMessage", function (userName, id, message, time) {
    var isUser = document.getElementById("username").value == userName
    var container = document.createElement("div");
    document.getElementById("messagesList").appendChild(container);
    container.className = !isUser ? "text-white px-2 rounded" : "px-2 border rounded ms-auto";
    container.style = !isUser ? "background-color: #33764e;" : "";
    container.id = `message_${id}`;
    
    var nameAndTrashRow = document.createElement("div");
    container.appendChild(nameAndTrashRow);
    nameAndTrashRow.className = "d-flex justify-content-between align-items-center"

    var userNameText = document.createElement("strong");
    nameAndTrashRow.appendChild(userNameText);
    userNameText.textContent = userName;

    var messageBody = document.createElement("div");
    container.appendChild(messageBody);
    messageBody.className = "d-flex gap-3"

    var messageText = document.createElement("p");
    messageBody.appendChild(messageText)
    messageText.textContent = message;
    messageText.className = !isUser ? "text-white" : "";

    var messageTime = document.createElement("small");
    messageBody.appendChild(messageTime);
    messageTime.textContent = time;
    messageTime.className = "small text-muted";

    var messageList = document.getElementById("messagesList");
    messageList.scrollTop = messageList.scrollHeight;
});

connection.on("ReceiveDeleteMessage", function (messageId) {
    console.log(`Received instruction to delete message_${messageId}`);
    document.getElementById(`message_${messageId}`).remove();
})

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("JoinGroup", parseInt(document.getElementById("requestId").value))
    console.log(document.getElementById("requestId").value)
}).catch(function (err) {
    return console.error(err.toString());
});



function sendMessage() {
    var messageInput = document.getElementById("messageInput");
    var userId = document.getElementById("userId").value;
    var text = messageInput.value;
    var requestId = parseInt(document.getElementById("requestId").value);

    if (text == "") {
        return;
    }
    messageInput.value = "";
    connection.invoke("SendMessage", userId, text, requestId).catch(function (err) {
        return console.error(err.toString());
    });
}

document.getElementById("messageForm").addEventListener("submit", (e) => {
    e.preventDefault();
    sendMessage();
})

