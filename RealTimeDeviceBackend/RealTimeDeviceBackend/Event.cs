using RealTimeDeviceSystem;
using System.Collections.Generic;

namespace RealTimeDeviceBackend
{
    public class Event
    {
        public List<string> messages {  get; set; }
        public Event(FireDetector fd)
        {
            messages = new List<string>();
            if (fd.SmokeStatus > 3)
            {
                messages.Add($"Повышенный уровень задымления - {fd.SmokeStatus}!" +
                    $" Местоположение: здание - {fd.Location.BuildingNumber}, " +
                    $"этаж - {fd.Location.FloorNumber} " +
                    $"номер - {fd.Location.Order}");
            }
            if (fd.Status == "Пожар")
            {
                messages.Add($"Пожар!" +
                   $" Местоположение: здание - {fd.Location.BuildingNumber}, " +
                   $"этаж - {fd.Location.FloorNumber} " +
                   $"номер - {fd.Location.Order}");
            }

            if (fd.Status == "Выключен")
            {
                messages.Add($"Датчик выключен!" +
                   $" Местоположение: здание - {fd.Location.BuildingNumber}, " +
                   $"этаж - {fd.Location.FloorNumber} " +
                   $"номер - {fd.Location.Order}");
            }
        }
    }
}
