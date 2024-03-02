using System;
using System.Collections.Generic;

namespace RealTimeDeviceSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Velocimeter> vList = new List<Velocimeter>();
            for (int i=0; i<10; i++)
            {
                Velocimeter v = new Velocimeter();
                v.FillVelocimeter($"Sensor#{i}", "", "DVA", 1, 1, i, 10, 15, 1);
                vList.Add(v);
            }

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(2);

            var timer = new System.Threading.Timer((e) =>
            {
                foreach(var v in vList)
                {
                    v.ChangeFireDetectorState(1);
                    Console.WriteLine($"Status of {v.Name} is: {v.Status}, Velocity = {v.VelocityValue}");
                }
                Console.WriteLine();
            }, null, startTimeSpan, periodTimeSpan);

            Console.ReadKey();
        }
    }
}
