var chat = {
    login: function () {
        console.log("login")
        $(".login-dialog").addClass("d-none");
        $(".main-chat").removeClass("d-none");

        clientService.init();
    },
    logout: function () {
        console.log("logout")
        $(".login-dialog").removeClass("d-none");
        $(".main-chat").addClass("d-none");
        clientService.disconnect();
    },
}

$(document).ready(function () {
    $("#loginButton").click(function () {
        let user = $("#userInput").val();
        if (user == null || user == "") {
            return;
        }

        chat.login();
    });

    $("#logoutButton").click(function () {
        chat.logout();
    });

});
