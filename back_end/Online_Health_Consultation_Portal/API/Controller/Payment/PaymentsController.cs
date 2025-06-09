using MediatR;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Commands.Payment;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;
using Online_Health_Consultation_Portal.Application.Queries.Payment;

namespace Online_Health_Consultation_Portal.API.Controller.Payment
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/payments
        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreatePayment([FromBody] CreatePaymentCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // GET: api/payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllPaymentsQuery());
            return Ok(result);
        }

        // GET: api/payments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetPaymentByIdQuery { Id = id });
            if (result == null) return NotFound();
            return Ok(result);
        }

        // PUT: api/payments/{id}/status
        [HttpPut("{id}/status")]
        public async Task<ActionResult<PaymentDto>> UpdateStatus(int id, [FromBody] string status, decimal amount)
        {
            var command = new UpdatePaymentStatusCommand
            {
                PaymentId = id,
                Status = status,
            };

            var result = await _mediator.Send(command);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE: api/payments/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePaymentCommand { PaymentId = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // GET: api/payments/appointment/{appointmentId}
        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetByAppointmentId(int appointmentId)
        {
            var result = await _mediator.Send(new GetPaymentsByAppointmentQuery { AppointmentId = appointmentId });
            return Ok(result);
        }

        // GET: api/payments/patient/{patientId}
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetByPatientId(int patientId)
        {
            var result = await _mediator.Send(new GetPaymentsByPatientQuery { PatientId = patientId });
            return Ok(result);
        }
    }
}
