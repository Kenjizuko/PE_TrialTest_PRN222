using Microsoft.AspNetCore.SignalR;

namespace Test_Prn222_Trial
{
    public class SignalrServer : Hub
    {
        public async Task NotifyItemDeleted(string itemId)
        {
            await Clients.All.SendAsync("ItemDeleted", itemId);
        }
    }
}
