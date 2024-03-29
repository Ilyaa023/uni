using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RTDC.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VelocimeterController : ControllerBase
    {
        List<Velocimeter> velocimeters = new List<Velocimeter>();
        public VelocimeterController()
        {
            for (int i = 0; i < 10; i++)
            {
                Velocimeter v = new Velocimeter();
                v.FillVelocimeter($"Sensor#{i}", "", "DVA", 1, 1, i, 10, 15, 1);
                velocimeters.Add(v);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(velocimeters);
        }

        [HttpGet]
        public IActionResult GetDetectorByLocation([FromQuery] int buildingNumber, [FromQuery] int device, [FromQuery] int order)
        {
            Velocimeter v = velocimeters.FirstOrDefault(f => f.Location.BuildingNumber == buildingNumber &&
                                                        f.Location.Order == order && f.Location.Device == device);
            if (v == null)
            {
                return NotFound();
            }
            return Ok(v);
        }

        [HttpPost]
        public IActionResult AddSensor([FromBody] Velocimeter meter)
        {
            Velocimeter v = velocimeters.FirstOrDefault(f => f.Location.BuildingNumber == meter.Location.BuildingNumber &&
                                                            f.Location.Order == meter.Location.Order &&
                                                            f.Location.Device == meter.Location.Device);
            velocimeters.Add(v);
            return Created("", meter);
        }

        [HttpPut]
        public IActionResult UpdateSensor([FromBody] Velocimeter meter)
        {
            Velocimeter v = velocimeters.FirstOrDefault(f => f.Location.BuildingNumber == meter.Location.BuildingNumber &&
                                                            f.Location.Order == meter.Location.Order &&
                                                            f.Location.Device == meter.Location.Device);
            if (v == null)
            {
                return BadRequest();
            }
            v.Name= meter.Name;
            v.Description= meter.Description;
            v.Model= meter.Model;
            v.VelocityValue = meter.VelocityValue;
            v.Status= meter.Status;
            v.WarningSetpointHigh= meter.WarningSetpointHigh;
            v.EmergencySetpointHigh= meter.EmergencySetpointHigh;
            v.Operability= meter.Operability;
            return Ok(v);
        }

        [HttpDelete]
        public IActionResult DeleteSensor([FromBody] Velocimeter meter)
        {
            Velocimeter v = velocimeters.FirstOrDefault(f => f.Location.BuildingNumber == meter.Location.BuildingNumber &&
                                                            f.Location.Order == meter.Location.Order &&
                                                            f.Location.Device == meter.Location.Device);
            velocimeters = velocimeters.Where(f => !(f.Location.BuildingNumber == meter.Location.BuildingNumber &&
                                                            f.Location.Order == meter.Location.Order &&
                                                            f.Location.Device == meter.Location.Device)).ToList();
            return Ok(velocimeters);
        }
    }
}
