using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OnlineAuctionApp.AuctionAPI.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task AddGroupAsync(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName).ConfigureAwait(false);
        }

        public async Task SendBidAsync(string groupName, string user, string bid)
        {
            await Clients.Group(groupName).SendAsync("Bids", user, bid).ConfigureAwait(false);
        }
    }
}
