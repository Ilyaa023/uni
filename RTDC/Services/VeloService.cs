using RTDC.Models;
using System;
using System.Text.Json;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace RTDC.Services
{
    public class VeloService: IVeloService
    {
        private static IVeloService service;
        private List<Velocimeter> velocimeters;
        private System.Timers.Timer timer;
        private HubConnection connection;

        public static IVeloService GetService() {
            if (service == null)
            {
                service = new VeloService();
            }
            return service;
        }
        private VeloService()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:6060/vmhub").Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(1000);
                await connection.StartAsync();
            };
            velocimeters = new List<Velocimeter>();
            CreateVelocimeters();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += RealTimeSimulator;
            timer.Enabled = true;
        }   
        public void CreateVelocimeters()
        {
            for (int i = 0; i < 10; i++)
            {
                PutVelocimeter($"Sensor#{i}", "", "DVA", 1, i%3, i, 10, 15, 1);
            }
        }

        public async Task<Velocimeter> PutVelocimeter(string name, string description, string model, int buildingNumber, int device, int order, int w, int e, int o)
        {
            Velocimeter v = new Velocimeter();
            v.FillVelocimeter(name, description, model, buildingNumber, device, order, w, e, o);

            if (GetByLocation(v.Location) != null)
                throw new Exception("Такой датчик уже существует");
            velocimeters.Add(v);
            await SendUpdatedValues();
            return v;
        }
        public async Task<Velocimeter> PutVelocimeter(Velocimeter v)
        {
            if (GetByLocation(v.Location) != null)
                throw new Exception("Такой датчик уже существует");
            velocimeters.Add(v);
            await SendUpdatedValues();
            return v;
        }
        public List<Velocimeter> GetVelocimeters()
        {
            return velocimeters;
        }
        public async Task DeleteVelocimeter(Velocimeter v)
        {
            Velocimeter vm = GetByLocation(v.Location);
            if (vm == null)
                throw new Exception("Такого датчика нет");
            velocimeters.RemoveAt(GetVelocimeters().IndexOf(vm));
            await SendUpdatedValues();
        }
        public Velocimeter GetByLocation(Location loc)
        {
            return velocimeters.FirstOrDefault(f => f.Location.BuildingNumber == loc.BuildingNumber &&
                                                        f.Location.Order == loc.Order && f.Location.Device == loc.Device);
        }
        public async Task<Velocimeter> Update(Velocimeter vOld, Velocimeter vNew)
        {
            Velocimeter vm = GetByLocation(vOld.Location);
            if (vm == null)
               throw new Exception("Такого датчика не сущесвует");
            vm.Name = vNew.Name;
            vm.Description = vNew.Description;
            vm.Model = vNew.Model;
            vm.VelocityValue = vNew.VelocityValue;
            vm.Status = vNew.Status;
            vm.Location = vNew.Location;
            vm.WarningSetpointHigh = vNew.WarningSetpointHigh;
            vm.EmergencySetpointHigh = vNew.EmergencySetpointHigh;
            vm.Operability = vNew.Operability;
            await SendUpdatedValues();
            return vm;
        }    
        public async void RealTimeSimulator(Object source, ElapsedEventArgs e)
        {
            foreach(Velocimeter v in velocimeters)
            {
                v.ChangeVeloDetectorState();
            }
        }

        public async Task SendUpdatedValues()
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
                await connection.InvokeAsync("UpdatedValues", JsonSerializer.Serialize(velocimeters, options));
            }
            catch { }
        }

        public async Task SendUpdatedEvents()
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
            foreach(Velocimeter v in velocimeters)
            {
                Event e = new Event(v);
                if (e.messages.Count > 0)
                {
                    await connection.InvokeAsync("UpdatedEvents", JsonSerializer.Serialize(e, options));
                }
            }
        }
    }
}
