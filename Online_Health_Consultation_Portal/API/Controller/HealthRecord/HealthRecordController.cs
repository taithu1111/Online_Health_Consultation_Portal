using MediatR;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Commands.HealthRecord;
using Online_Health_Consultation_Portal.Application.Dtos.HealthRecord;
using Online_Health_Consultation_Portal.Application.Queries.HealthRecord;

namespace Online_Health_Consultation_Portal.API.Controller.HealthRecord
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HealthRecordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/HealthRecords/{patientId}
        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetHealthRecords(int patientId)
        {
            var query = new GetHealthRecordByPatientQuery(patientId);
            var healthRecords = await _mediator.Send(query);
            return Ok(healthRecords);
        }

        // POST: api/HealthRecords
        [HttpPost]
        public async Task<IActionResult> CreateHealthRecord([FromBody] HealthRecordDto healthRecordDto)
        {
            var command = new CreateHealthRecordCommand(
                healthRecordDto.PatientId, 
                healthRecordDto.RecordType, 
                healthRecordDto.FileUrl, 
                healthRecordDto.CreatedAt);
            var healthRecordId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetHealthRecords), new { patientId = healthRecordDto.PatientId }, healthRecordId);
        }

        // PUT: api/HealthRecords/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHealthRecord(int id, [FromBody] HealthRecordDto healthRecordDto)
        {
            var command = new UpdateHealthRecordCommand(
                id, 
                healthRecordDto.PatientId, 
                healthRecordDto.RecordType, 
                healthRecordDto.FileUrl, 
                healthRecordDto.CreatedAt
            );
            var success = await _mediator.Send(command);
            if (success)
            {
                return NoContent();
            }

            return NotFound("Health record not found.");
        }

        // DELETE: api/HealthRecords/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthRecord(int id)
        {
            var command = new DeleteHealthRecordCommand(id);
            var success = await _mediator.Send(command);
            if (success)
            {
                return NoContent();
            }

            return NotFound("Health record not found.");
        }
    }
}
