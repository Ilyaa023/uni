using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace RTDC.Models
{
    public class Event
    {
        public List<string> messages { get; set; }
        public Event(Velocimeter v)
        {
            messages= new List<string>();
            string m;
            if (Velocimeter.StatusList[0] == v.Status || Velocimeter.StatusList[1] == v.Status)
                m = $"Sensor turned {v.Status}, ";
            else if (Velocimeter.StatusList[2] == v.Status)
                m = $"High level of vibration - {v.Status}! ";
            else if (Velocimeter.StatusList[3] == v.Status)
                m = $"Very high level of vibration - {v.Status}! ";
            else m = $"Unknown state - {v.Status}";

            messages.Add($"{m}" +
                $"Location is: building number - {v.Location.BuildingNumber}, " +
                $"machine number - {v.Location.Device}, " +
                $"sensor number - {v.Location.Order}");
        }
    }
}
