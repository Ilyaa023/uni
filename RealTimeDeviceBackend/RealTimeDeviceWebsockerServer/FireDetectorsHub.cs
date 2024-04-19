using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace RealTimeDeviceWebsockerServer
{
    public class FireDetectorsHub: Hub
    {
        public async Task UpdateFireDetectorState(string message)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("UpdateFireDetectorState", message);
        }

        public async Task UpdateFireDetectorEvents(string message)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("UpdateFireDetectorEvents", message);
        }
    }
}
