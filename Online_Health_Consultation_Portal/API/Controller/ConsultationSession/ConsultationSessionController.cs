using MediatR;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Commands.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Queries.ConsultationSession;

namespace Online_Health_Consultation_Portal.API.Controller.ConsultationSession
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationSessionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ConsultationSessionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateConsultationSessionCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var session = await _mediator.Send(new GetConsultationSessionByIdQuery { Id = id });
            return session != null ? Ok(session) : NotFound();
        }
        
        [HttpPut("{id}/start")]
        public async Task<IActionResult> StartSession(int id)
        {
            var result = await _mediator.Send(new StartConsultationSessionCommand { Id = id });
            return result ? Ok("Session started") : NotFound();
        }

        [HttpPut("{id}/end")]
        public async Task<IActionResult> EndSession(int id)
        {
            var result = await _mediator.Send(new EndConsultationSessionCommand { Id = id });
            return result ? Ok("Session ended") : NotFound();
        }

        [HttpGet("by-doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctorId(int doctorId)
        {
            var sessions = await _mediator.Send(new GetConsultationsByDoctorQuery { DoctorId = doctorId });
            return Ok(sessions);
        }

        [HttpGet("by-patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var sessions = await _mediator.Send(new GetConsultationsByPatientQuery { PatientId = patientId });
            return Ok(sessions);
        }
    }
}
