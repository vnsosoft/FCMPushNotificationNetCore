var clientService = {
    functionKeys: {
        joinToGroup: "JoinToGroup",
        removeFromGroup: "RemoveFromGroup",
        sendMessageToGroup: "SendMessageToGroup",
        sendMessageToUser: "SendMessageToUser",
    },
    settings: {
        user: null,
        messageInput: '.message-input',
        sendButton: $(".message-submit"),
        logoutButton: $("#logoutButton"),
        connection: null,
    },
    groupName: "DEMO",
    isLogin: false,
    connectionUrl: "https://localhost:44392/chatHub",
    init: function () {
        clientService.isLogin = true;
        clientService.connect();
    },
    connect: function () {
        if (!clientService.isLogin) {
            return;
        }

        var user = $("#userInput").val();
        clientService.settings.user = user;
        console.log(clientService.settings.user);

        var connection = new signalR.HubConnectionBuilder().withUrl(this.connectionUrl).build();
        connection.start().then(function () {
            connection.invoke(clientService.functionKeys.joinToGroup, clientService.groupName, user).catch(function (err) {
                return console.error(err.toString());
            });

        }).catch(function (err) {
            return console.error(err.toString());
        });

        clientService.initBackGroundServices(connection);
        clientService.settings.connection = connection;
    },
    initBackGroundServices: function (connection) {
        if (!clientService.isLogin) {
            return;
        }

        connection.on("message", function (from, message) {
            console.log(`${from} : ${message}`)

            clientService.handleMessage(from, message);
        });

        connection.on("notify", function (message) {
            var li = document.createElement("li");
            document.getElementById("messagesList").appendChild(li);
            li.textContent = `${message}`;
        });
    },
    disconnect: function () {
        if (!clientService.isLogin) {
            return;
        }
        clientService.isLogin = false;
        clientService.settings.connection.invoke(clientService.functionKeys.removeFromGroup, clientService.groupName).catch(function (err) {
            return console.error(err.toString());
        });
    },
    handleMessage: function (from, messages) {
    },
    sendMessageToGroup: function (fromUser, messages) {
        if (!clientService.isLogin) {
            return;
        }

        clientService.settings.connection.invoke(clientService.functionKeys.sendMessageToGroup, clientService.groupName, fromUser, messages).catch(function (err) {
            return console.error(err.toString());
        });

        event.preventDefault();
    }
};

