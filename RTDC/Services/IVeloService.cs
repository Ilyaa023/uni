using RTDC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTDC.Services
{
    public interface IVeloService
    {
        protected static IVeloService service;
        public static IVeloService GetService() { return service; }
        public void CreateVelocimeters();
        public async Task<Velocimeter> PutVelocimeter(string name, string description, string model, int buildingNumber, int device, int order, int w, int e, int o) { throw new System.Exception("Very Bad Requset"); }
        public async Task<Velocimeter> PutVelocimeter(Velocimeter v) { throw new System.Exception("Very Bad Requset"); }
        public List<Velocimeter> GetVelocimeters();
        public async Task DeleteVelocimeter(Velocimeter v) { }
        public Velocimeter GetByLocation(Location loc);
        public async Task<Velocimeter> Update(Velocimeter vOld, Velocimeter vNew){ return null; }
        public async Task SendUpdatedValues() { }
        public async Task SendUpdatedEvents() { }
    }   
}
