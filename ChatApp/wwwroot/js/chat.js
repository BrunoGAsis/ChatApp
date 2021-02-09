class Message {
    constructor(userEmail, messageText) {
        this.userEmail = userEmail;
        this.messageText = messageText;
    }
};

//Fetch from index page constant created with viewbag
const userEmail = currentUserEmail;

var errorCount = 0;

var connection = new signalR.HubConnectionBuilder()
    .withUrl('/Home/Index')
    .build();

connection.on("addMessage", addMessage);
connection.on("addErrorMessage", addErrorMessage);
connection.on("removeMessage", removeMessage);


async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
        loadChat();
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(start);

// Start the connection.
start();


function sendMessage(msg) {
    connection.invoke("sendMessage", msg);
}

//Intercept enter key for chat input

$("#txtMessage").on('keyup', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        pickUpMessage();
    }
});

function pickUpMessage() {
    try {

        var msg = new Message(userEmail, $("#txtMessage").val());
        if (msg != "" && msg !=undefined && msg !=null) {
            sendMessage(msg);
            $("#txtMessage").val("");
        }
    }
    catch (e) {
        console.log(e);
    }
}

function addErrorMessage(msg) {

    var div = $("#msg-div");

    errorCount--;

    div.append("<div id=\"" + errorCount + "\" class=\"media msg\"><a class=\"pull-right\"><img class=\"media-object\" style=\"width: 32px; height: 32px;\" src=\"/img/robot.png\" /></a ><div class=\"media-body\"><h5 class=\"media-heading text-danger\"><b>" + msg.userEmail + "</b></h5><small class=\"col-lg-10\"><b>" + msg.messageText + "</b></small><small><a href=\"javascript: dismiss(" + errorCount + ");\">Dismiss</a></small></div></div >");

    $('#msg-div').scrollTop($('#msg-div')[0].scrollHeight);
}

function addMessage(msg) {
    var div = $("#msg-div");

    var myDate = "";
    var fmDate = "";
    if (msg.createdDate != null) {
        myDate = new Date(msg.createdDate);
        fmDate = (myDate.getMonth() + 1) + "/" + myDate.getDate() + "/" + myDate.getFullYear() + " " + myDate.getHours().padLeft(2) + ":" + myDate.getMinutes().padLeft(2) + ":" + myDate.getSeconds().padLeft(2);
    }

    if (msg.userEmail == "ChatBot") {
        div.append("<div id=\"" + msg.id + "\" class=\"media msg\"><a class=\"pull-right\"><img class=\"media-object\" style=\"width: 32px; height: 32px;\" src=\"/img/robot.png\" /></a ><div class=\"media-body\"><small class=\"pull-right time\"><i class=\"fa fa-clock-o\"></i> " + fmDate + "</small><h5 class=\"media-heading\"><b>" + msg.userEmail + "</b></h5><small class=\"col-lg-10\"><b>" + msg.messageText + "</b></small></div></div >");
    }
    else {
        div.append("<div id=\"" + msg.id + "\" class=\"media msg\"><a class=\"pull-right\"><img class=\"media-object\" style=\"width: 32px; height: 32px;\" src=\"/img/person.png\" /></a ><div class=\"media-body\"><small class=\"pull-right time\"><i class=\"fa fa-clock-o\"></i> " + fmDate + "</small><h5 class=\"media-heading\"><b>" + msg.userEmail + "</b></h5><small class=\"col-lg-10\"><b>" + msg.messageText + "</b></small></div></div >");
    }

    $('#msg-div').scrollTop($('#msg-div')[0].scrollHeight);
}



//For removing bot error messages
function dismiss(e) {
    $("#" + e).remove();
}



function removeMessage(id) {
    $("#" + id).remove();
}


function loadChat() {
    $.getJSON("/home/getavailablemessages", function (data) {
        try {

            for (i = 0; i < data.length; i++) {
                addMessage(data[i]);
            }
        }
        catch (e) {
            console.log(e);
            console.log(data);
        }
    });
}



function padLeft(nr, n, str) {
    return Array(n - String(nr).length + 1).join(str || '0') + nr;
}
//or as a Number prototype method:
Number.prototype.padLeft = function (n, str) {
    return Array(n - String(this).length + 1).join(str || '0') + this;
}