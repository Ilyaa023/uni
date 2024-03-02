using System;
using System.Collections.Generic;

namespace RealTimeDeviceSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<FireDetector> fdList = new List<FireDetector>();
            for (int i=0; i<10; i++)
            {
                FireDetector fd = new FireDetector();
                fd.FillFireDetector($"Датчик#{i}", "", "ДП1", 1, 1, i);
                fdList.Add(fd);
            }

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(2);

            var timer = new System.Threading.Timer((e) =>
            {
                foreach(var fd in fdList)
                {
                    fd.ChangeFireDetectorState();
                    Console.WriteLine($"Состояние датчика {fd.Name} - Статус: {fd.Status}, Дым - {fd.SmokeStatus}");
                }
            }, null, startTimeSpan, periodTimeSpan);

            Console.ReadKey();
        }
    }
}
