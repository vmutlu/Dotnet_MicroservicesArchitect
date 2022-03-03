var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:8001/auctionHub").build();
var auctionId = document.getElementById("AuctionId").value;

document.getElementById("sendButton").disabled = true; //if signalR connection disabled, button visible disabled

var groupName = "auction-" + auctionId;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("AddToGroup", groupName).catch(function (err) {
        return console.log(err.toString());
    });
}).catch(function (err) {
    return console.log(err.toString());
})