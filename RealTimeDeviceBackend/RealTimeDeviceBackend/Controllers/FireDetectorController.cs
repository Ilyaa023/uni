using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeDeviceSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RealTimeDeviceBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FireDetectorController : ControllerBase
    {
        IFireDetectorService fireDetectorService;
        public FireDetectorController(IFireDetectorService fireDetectorService) {
            this.fireDetectorService = fireDetectorService;
        }

        [HttpGet]
        public IActionResult GetAllFireDetectors()
        {
            return Ok(fireDetectorService.GetFireDetectors());
        }

        [HttpGet]
        public IActionResult GetFireDetectorByLocation([FromQuery]int buildingNumber, [FromQuery]int floorNumber, [FromQuery]int order)
        {
            try
            {
                return Ok(fireDetectorService.GetFireDetector(new Location()
                {
                    BuildingNumber = buildingNumber,
                    FloorNumber = floorNumber,
                    Order = order
                }));
            } catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public IActionResult AddFireDetector([FromBody] FireDetector detector)
        {
            try
            {
                return Ok(fireDetectorService.AddFireDetector(detector));
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete]
        public IActionResult RemoveFireDetector([FromQuery] int buildingNumber, [FromQuery] int floorNumber, [FromQuery] int order) {

            try
            {
                fireDetectorService.RemoveFireDetector(new Location()
                {
                    BuildingNumber = buildingNumber,
                    FloorNumber = floorNumber,
                    Order = order
                });
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();

        }

        [HttpPut]
        public IActionResult UpdateFireDetector([FromBody]FireDetector fd)
        {
            try
            {
                fireDetectorService.UpdateFireDetector(fd);
                return Ok(fireDetectorService.UpdateFireDetector(fd));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
