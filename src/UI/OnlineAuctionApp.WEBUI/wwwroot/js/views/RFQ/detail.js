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

connection.on("Bids", function (user, bid) {
    addBidToTable();
})

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("SellerUserName").value;
    var productId = document.getElementById("ProductId").value;
    var sellerUser = user;
    var bid = document.getElementById("inputPrice").value;

    var sendBid = {
        AuctionId: auctionId,
        ProductId: productId,
        SellerUserName: sellerUser,
        Price: parseFloat(bid).toString()
    };

    SendBid(sendBid);
    event.preventDefault();
});

document.getElementById("finishButton").addEventListener("click", function (event) {
    var sendCompleteBid = {
        AuctionId: auctionId
    };

    sendCompleteBid(sendCompleteBid);
    event.preventDefault();
});

function addBidToTable(user, bid) {
    var str = "<tr>";
    str += "<td>" + user + "</td>";
    str += "<td>" + bid + "</td>";
    str += "</tr>";

    if ($('table > tbody > tr:first').length > 0) {
        $('table > tbody > tr:first').before(str);
    }
    else {
        $('.bidLine').append(str);
    }
}

function SendBid(model) {

    $.ajax({
        url: "/Auction/SendBid",
        type: "POST",
        data: model,
        success: function (response) {
            if (response.isSuccess) {
                document.getElementById("inputPrice").value = "";

                connection.invoke("SendBidAsync", groupName, model.SellerUserName, model.Price).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });

}

function sendCompleteBid(model) {

    $.ajax({
        url: "/Auction/CompleteBid",
        type: "POST",
        data: { id: auctionId}, //send id
        success: function (response) {
            if (response.isSuccess) {
                console.log("Transaction successful");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });

}