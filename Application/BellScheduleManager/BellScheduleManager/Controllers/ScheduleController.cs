using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BellScheduleManager.Resources.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BellScheduleManager.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetSchedules()
        {
            return Ok(new List<ScheduleModel>()
            {
                new ScheduleModel() { ScheduleId = Guid.NewGuid(), ScheduleName = "My Schedule 1" }
            });
        }

        [HttpPost("update-from-file")]
        public async Task<IActionResult> UpdateScheduleFromFile([FromForm] string scheduleName, [FromForm] IFormFile file)
        {

            return Ok();
        }
    }
}
