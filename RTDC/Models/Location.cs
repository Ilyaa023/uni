namespace RTDC.Models
{
    public class Location
    {
        public int BuildingNumber { get; set; }
        public int Device { get; set; }
        public int Order { get; set; }

        public void FillLocation(int buildingNumber, int device, int order)
        {
            BuildingNumber = buildingNumber;
            Device = device;
            Order = order;
        }
    }
}
