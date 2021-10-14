
$(window).load(function () {
    $messages.mCustomScrollbar();
    //setTimeout(function () {
    //    fakeMessage();
    //}, 100);
});

$(window).on('keydown', function (e) {
    if (e.which == 13) {
        var fromUser = $("#userInput").val();
        var msg = $('.message-input').val();
        clientService.sendMessageToGroup(fromUser, msg);
        //insertMessage(msg);
        return false;
    }
});

$(".message-submit").click(function () {
    var fromUser = $("#userInput").val();
    var msg = $('.message-input').val();
    clientService.sendMessageToGroup(fromUser, msg);
    //insertMessage(msg);
});

var $messages = $('.messages-content'),
    d, h, m,
    i = 0;

function updateScrollbar() {
    $messages.mCustomScrollbar("update").mCustomScrollbar('scrollTo', 'bottom', {
        scrollInertia: 10,
        timeout: 0
    });
}

function setDate() {
    d = new Date();
    if (m != d.getMinutes()) {
        m = d.getMinutes();
        $('<div class="timestamp">' + d.getHours() + ':' + m + '</div>').appendTo($('.message:last'));
        $('<div class="checkmark-sent-delivered">&check;</div>').appendTo($('.message:last'));
        $('<div class="checkmark-read">&check;</div>').appendTo($('.message:last'));
    }
}

function insertMessage(messages) {
    if ($.trim(messages) == '') {
        return false;
    }
    $('<div class="message message-personal">' + messages + '</div>').appendTo($('.mCSB_container')).addClass('new');
    setDate();
    $('.message-input').val(null);
    updateScrollbar();
    setTimeout(function () {
        fakeMessage();
    }, 1000 + Math.random() * 20 * 100);
}

function IsParner(fromUser) {
    var me = $("#userInput").val();
    if (fromUser === me) {
        return true;
    }

    return false;
}

function renderMyMessage(messages) {
    if ($.trim(messages) == '') {
        return false;
    }
    $('<div class="message message-personal">' + messages + '</div>').appendTo($('.mCSB_container')).addClass('new');
    setDate();
    $('.message-input').val(null);
    updateScrollbar();
}

function renderPartnerMessage(messages) {
    if ($.trim(messages) == '') {
        return false;
    }

    $('<div class="message loading new"><figure class="avatar"><img src="http://askavenue.com/img/17.jpg" /></figure><span></span></div>').appendTo($('.mCSB_container'));
    updateScrollbar();

    setTimeout(function () {
        $('.message.loading').remove();
        $('<div class="message new"><figure class="avatar"><img src="http://askavenue.com/img/17.jpg" /></figure>' + messages + '</div>').appendTo($('.mCSB_container')).addClass('new');
        setDate();
        updateScrollbar();
        i++;
    }, 1000 + Math.random() * 20 * 100);
}

var Fake = [
    'Hi there, I\'m Jesse and you?',
    'Nice to meet you',
    'How are you?',
    'Not too bad, thanks',
    'What do you do?',
    'That\'s awesome',
    'Codepen is a nice place to stay',
    'I think you\'re a nice person',
    'Why do you think that?',
    'Can you explain?',
    'Anyway I\'ve gotta go now',
    'It was a pleasure chat with you',
    'Time to make a new codepen',
    'Bye',
    ':)'];

function fakeMessage() {
    if ($('.message-input').val() != '') {
        return false;
    }
    $('<div class="message loading new"><figure class="avatar"><img src="http://askavenue.com/img/17.jpg" /></figure><span></span></div>').appendTo($('.mCSB_container'));
    updateScrollbar();

    setTimeout(function () {
        $('.message.loading').remove();
        $('<div class="message new"><figure class="avatar"><img src="http://askavenue.com/img/17.jpg" /></figure>' + Fake[i] + '</div>').appendTo($('.mCSB_container')).addClass('new');
        setDate();
        updateScrollbar();
        i++;
    }, 1000 + Math.random() * 20 * 100);
}


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
    handleMessage: function (from, message) {
        var itsMe = IsParner(from);
        if (itsMe == true) {
            console.log(message);
            renderMyMessage(message);
        } else {
            console.log(`${from} : ${message}`)
            renderPartnerMessage(message);
        }

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