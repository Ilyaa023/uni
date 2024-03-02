using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace RealTimeDeviceSystem
{
    public class FireDetector
    {
        public static string[] StatusList = { "Работает", "Выключен", "Пожар" };
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public Location Location { get; set; }
        public int SmokeStatus { get; set; }
        public string Status { get; set; }

        public void FillFireDetector(string name, string description, string model, int buildingNumber, int floorNumber, int order)
        {
            this.Name = name;
            this.Description = description;
            this.Model = model;
            Location loc = new Location();
            loc.FillLocation(buildingNumber, floorNumber, order);
            this.Location = loc;
        }

        public void ChangeFireDetectorState()
        {
            Random random = new Random();
            this.SmokeStatus = random.Next(0, 11);
            this.Status = StatusList[random.Next(0, 3)];
        }
    }

    public class Location
    {
        public int BuildingNumber { get; set; }
        public int FloorNumber { get; set; }
        public int Order { get; set; }

        public void FillLocation(int buildingNumber, int floorNumber, int order)
        {
            this.BuildingNumber = buildingNumber;
            this.FloorNumber = floorNumber;
            this.Order = order;
        }
    }
}
