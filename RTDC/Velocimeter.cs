using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace RTDC
{
    public class Velocimeter
    {
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
            this.Name = name;
            this.Description = description;
            this.Model = model;
            Location loc = new Location();
            loc.FillLocation(buildingNumber, device, order);
            this.Location = loc;
            this.EmergencySetpointHigh = e;
            this.WarningSetpointHigh = w;
            this.Operability = o;
        }

        public void ChangeFireDetectorState(int o)
        {
            Random random = new Random();
            Operability = o;
            this.VelocityValue = random.Next(0, 20);

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
        }
    }

    public class Location
    {
        public int BuildingNumber { get; set; }
        public int Device { get; set; }
        public int Order { get; set; }

        public void FillLocation(int buildingNumber, int device, int order)
        {
            this.BuildingNumber = buildingNumber;
            this.Device = device;
            this.Order = order;
        }
    }
}
