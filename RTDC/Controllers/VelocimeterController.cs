using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RTDC.Models;
using RTDC.Services;
using System;
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
        public VelocimeterController()
        {
            VeloService.GetService().GetVelocimeters();
            //.GetVelocimeters();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(VeloService.GetService().GetVelocimeters());
        }

        [HttpGet]
        public IActionResult GetDetectorByLocation([FromQuery] int buildingNumber, [FromQuery] int device, [FromQuery] int order)
        {
            Location l = new Location();
            l.FillLocation(buildingNumber, device, order);
            Velocimeter v = VeloService.GetService().GetByLocation(l);
            if (v == null)
            {
                return NotFound();
            }
            return Ok(v);
        }

        [HttpPost]
        public IActionResult AddSensor([FromBody] Velocimeter meter)
        {
            try
            {
                return Ok( VeloService.GetService().PutVelocimeter(meter));
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateSensor([FromBody] Velocimeter vOld, [FromQuery] Velocimeter vNew)
        {
            try
            {
                return Ok(VeloService.GetService().Update(vOld, vNew));
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteSensor([FromBody] Velocimeter meter)
        {
            try
            {
                return Ok(VeloService.GetService().DeleteVelocimeter(meter));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
