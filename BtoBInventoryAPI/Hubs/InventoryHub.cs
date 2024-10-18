using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace BtoBInventoryAPI.Hubs
{
    public class InventoryHub : Hub
    {
        public async Task SendMessage( string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

    }
}
