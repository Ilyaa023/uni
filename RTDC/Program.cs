using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RTDC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTDC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*List<Velocimeter> vList = new List<Velocimeter>();
            for (int i = 0; i < 10; i++)
            {
                Velocimeter v = new Velocimeter();
                v.FillVelocimeter($"Sensor#{i}", "", "DVA", 1, 1, i, 10, 15, 1);
                vList.Add(v);
            }

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(2);

            var timer = new System.Threading.Timer((e) =>
            {
                foreach (var v in vList)
                {
                    v.ChangeFireDetectorState(1);
                    Console.WriteLine($"Status of {v.Name} is: {v.Status}, Velocity = {v.VelocityValue}");
                }
                Console.WriteLine();
            }, null, startTimeSpan, periodTimeSpan);*/
            VeloService.GetService();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
