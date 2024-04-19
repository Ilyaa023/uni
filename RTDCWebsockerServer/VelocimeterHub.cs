using Microsoft.AspNetCore.SignalR;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace RTDCWebsockerServer
{
    public class VelocimeterHub: Hub
    {
        private async Task UpdatedValues(string mess)
        {
            await Clients.All.SendAsync("UpdatedValues", mess);
        }

        private async Task UpdatedEvents(string mess)
        {
            await Clients.All.SendAsync("UpdatedEvents", mess);
        }
    }
}
