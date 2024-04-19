using RealTimeDeviceSystem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealTimeDeviceBackend
{
    public interface IFireDetectorService
    {
        public FireDetector GetFireDetector(Location location);

        public List<FireDetector> GetFireDetectors();

        public Task<FireDetector> AddFireDetector(FireDetector detector);

        public Task<FireDetector> UpdateFireDetector(FireDetector detector);

        public Task RemoveFireDetector(Location location);

    }
}
