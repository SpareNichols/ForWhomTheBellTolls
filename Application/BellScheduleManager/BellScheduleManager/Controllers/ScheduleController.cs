using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BellScheduleManager.Resources.Models;
using BellScheduleManager.Resources.Services;
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
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchedules(CancellationToken cancellationToken)
        {
            var schedules = await _scheduleService.GetSchedulesAsync(cancellationToken).ConfigureAwait(false);

            return Ok(schedules);
        }

        [HttpDelete("{scheduleId}")]
        public async Task<IActionResult> DeleteSchedule(Guid scheduleId, CancellationToken cancellationToken)
        {
            await _scheduleService.DeleteScheduleAsync(scheduleId, cancellationToken).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost("{scheduleId}/google-calendar")]
        public async Task<IActionResult> CreateGoogleCalendarForSchedule(Guid scheduleId, CancellationToken cancellationToken)
        {
            await _scheduleService.CreateCalendarForScheduleAsync(scheduleId, cancellationToken).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost("from-file")]
        public async Task<IActionResult> UpdateScheduleFromFile([FromForm] string scheduleName, [FromForm] IFormFile file)
        {
            using var stream = file.OpenReadStream();
            await _scheduleService.CreateScheduleFromFileAsync(scheduleName, stream).ConfigureAwait(false);
            return Ok();
        }
    }
}
