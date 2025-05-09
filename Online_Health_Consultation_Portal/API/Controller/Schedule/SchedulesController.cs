using MediatR;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Commands.Schedule;
using Online_Health_Consultation_Portal.Application.Dtos.Schedule;
using Online_Health_Consultation_Portal.Application.Queries.Schedule;

namespace Online_Health_Consultation_Portal.API.Controller.Schedule
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SchedulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/schedules
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDoctorSchedules), new { doctorId = command.DoctorId }, result);
        }

        // GET: api/schedules/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] UpdateScheduleCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Schedule ID mismatch.");
            }
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/schedules/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var result = await _mediator.Send(new DeleteScheduleCommand { Id = id });
            return Ok(result);
        }

        // GET: api/schedules/doctor/{doctorId}
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorSchedules(int doctorId)
        {
            var result = await _mediator.Send(new GetDoctorSchedulesQuery { DoctorId = doctorId });
            return Ok(result);
        }

        // GET: api/schedules/available-slots
        [HttpGet("available-slots")]
        public async Task<ActionResult<List<AvailableSlotDto>>> GetAvailable([FromQuery] GetAvailableSlotsQuery query)
        {
            var slots = await _mediator.Send(query);
            return Ok(slots);
        }
    }
}
