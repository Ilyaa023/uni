using Microsoft.AspNetCore.SignalR.Client;
using RealTimeDeviceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Timers;

namespace RealTimeDeviceBackend
{
    public class FireDetectorService: IFireDetectorService
    {
        private List<FireDetector> fireDetectors = new List<FireDetector>();

        private System.Timers.Timer timer;

        private HubConnection connection;

        public FireDetectorService() {

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:6060/fdhub")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            for (int i = 0; i < 10; i++)
            {
                FireDetector fd = new FireDetector();
                fd.FillFireDetector($"Датчик#{i}", "", "ДП1", 1, 1, i);
                fireDetectors.Add(fd);
            }

            timer = new System.Timers.Timer(30000);
            timer.Elapsed += RealTimeSimulation;
            timer.Enabled = true;
        }

        public async Task<FireDetector> AddFireDetector(FireDetector detector)
        {
            if (FireDetectorExists(detector.Location.BuildingNumber, detector.Location.FloorNumber, detector.Location.Order))
            {
                throw new Exception("Данный датчик уже существует");
            }
            fireDetectors.Add(detector);
            await sendUpdateDetectorsState();
            return detector;
        }

        public FireDetector GetFireDetector(Location location) {
            if (!FireDetectorExists(location.BuildingNumber, location.FloorNumber, location.Order))
            {
                throw new Exception("Данный датчик не найден");
            }
            return FireDetectorByLocation(location.BuildingNumber, location.FloorNumber, location.Order);
        }

        public List<FireDetector> GetFireDetectors()
        {
            return fireDetectors;
        }

        public async Task RemoveFireDetector(Location location)
        {
            if (!FireDetectorExists(location.BuildingNumber, location.FloorNumber, location.Order))
            {
                throw new Exception("Данного датчика не существует");
            }
            fireDetectors = fireDetectors.Where(f => !(f.Location.BuildingNumber == location.BuildingNumber
                                                             && f.Location.FloorNumber == location.FloorNumber
                                                             && f.Location.Order == location.Order))
                                         .ToList();
            await sendUpdateDetectorsState();
        }

        public async Task<FireDetector> UpdateFireDetector(FireDetector fd)
        {
            if (!FireDetectorExists(fd.Location.BuildingNumber, fd.Location.FloorNumber, fd.Location.Order))
            {
                throw new Exception("Данного датчика не существует");
            }
            FireDetector detector = FireDetectorByLocation(fd.Location.BuildingNumber, fd.Location.FloorNumber, fd.Location.Order);
            detector.Name = fd.Name;
            detector.Description = fd.Description;
            detector.Model = fd.Model;
            await sendUpdateDetectorsState();
            return detector;
        }

        private bool FireDetectorExists(int buildingNumber, int floorNumber, int order)
        {
            FireDetector fd = fireDetectors.FirstOrDefault(f => f.Location.BuildingNumber == buildingNumber
                                                             && f.Location.FloorNumber == floorNumber
                                                             && f.Location.Order == order);
            return fd != null;
        }

        private FireDetector FireDetectorByLocation(int buildingNumber, int floorNumber, int order)
        {
            FireDetector fd = fireDetectors.FirstOrDefault(f => f.Location.BuildingNumber == buildingNumber
                                                             && f.Location.FloorNumber == floorNumber
                                                             && f.Location.Order == order);
            return fd;
        }

        private async void RealTimeSimulation(Object source, ElapsedEventArgs e)
        {
            foreach (FireDetector detector in fireDetectors)
            {
                detector.ChangeFireDetectorState();
            }
            await sendUpdateDetectorsState();
            await sendUpdateDetectorsEvents();
        }

        private async Task sendUpdateDetectorsState()
        {
            try
            {
                if (connection.State != HubConnectionState.Connected)
                {
                    await connection.StartAsync();
                }

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                await connection.InvokeAsync("UpdateFireDetectorState", JsonSerializer.Serialize(fireDetectors, options));
            }
            catch { }
        }

        private async Task sendUpdateDetectorsEvents()
        {
            if (connection.State != HubConnectionState.Connected)
            {
                await connection.StartAsync();
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            foreach (FireDetector detector in fireDetectors)
            {
                Event e = new Event(detector);
                if (e.messages.Count > 0)
                {
                    await connection.InvokeAsync("UpdateFireDetectorEvents", JsonSerializer.Serialize(e, options));
                }
            }
            
        }
    }
}
