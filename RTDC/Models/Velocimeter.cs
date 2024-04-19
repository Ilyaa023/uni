using Microsoft.AspNetCore.Authentication;
using RTDC.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTDC.Models
{
    public class Velocimeter
    {
        private static List<Velocimeter> velocimeters = new List<Velocimeter>();

        public static string[] StatusList = { "On", "Off", "Warning", "Emergency" };
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public Location Location { get; set; }
        public int VelocityValue { get; set; }
        public string Status { get; set; }
        public int WarningSetpointHigh { get; set; }
        public int EmergencySetpointHigh { get; set; }
        public int Operability { get; set; }

        public void FillVelocimeter(string name, string description, string model, int buildingNumber, int device, int order, int w, int e, int o)
        {
            Name = name;
            Description = description;
            Model = model;
            Location loc = new Location();
            loc.FillLocation(buildingNumber, device, order);
            Location = loc;
            EmergencySetpointHigh = e;
            WarningSetpointHigh = w;
            Operability = o;
        }

        public async void ChangeVeloDetectorState()
        {
            Random random = new Random();
            VelocityValue = random.Next(0, 20);

            if (Operability == 0)
                Status = StatusList[1];
            else
                Status = StatusList[0];

            if (Status != StatusList[1])
            {
                if (VelocityValue > EmergencySetpointHigh)
                    Status = StatusList[3];
                else if (VelocityValue > WarningSetpointHigh)
                    Status = StatusList[2];
                else
                    Status = StatusList[0];
            }
            await VeloService.GetService().SendUpdatedValues();
            await VeloService.GetService().SendUpdatedEvents();
        }

    }
}
