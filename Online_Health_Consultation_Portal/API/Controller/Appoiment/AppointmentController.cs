using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Command.Appointment;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Application.Queries.Appointment;

namespace Online_Health_Consultation_Portal.API.Controllers.Appointment
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Admin, Patient")]
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody]CreateAppointmentDto dto)
        {
            var id = await _mediator.Send(new CreateAppointmentCommand { Appointment = dto });
            return Ok(id);
        }

        [Authorize(Roles = "Admin, Patient, Doctor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody]UpdateAppointmentDto dto)
        {
            try
            {
                bool result = await _mediator.Send(new UpdateAppointmentCommand { AppointmentId = id, Appointment = dto });
                if (result)
                {
                    return Ok("Appointment updated successfully");
                }
                else
                {
                    return NotFound("Appointment not found."); // Trả về 404 nếu không tìm thấy cuộc hẹn
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You do not have permission to update this appointment.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Trả về 400 nếu có lỗi
            }
        }

        [Authorize(Roles = "Admin, Patient, Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                bool result = await _mediator.Send(new CancelAppointmentCommand { AppointmentId = id });
                if (result)
                {
                    return Ok("Appointment deleted successfully");
                }
                else
                {
                    return NotFound("Appointment not found."); // Trả về 404 nếu không tìm thấy cuộc hẹn
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You do not have permission to cancel this appointment.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Trả về 400 nếu có lỗi
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
        {
            var appointments = await _mediator.Send(new GetPatientAppointmentsQuery { PatientId = patientId });
            return Ok(appointments);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = await _mediator.Send(new GetDoctorAppointmentsQuery { DoctorId = doctorId });
            return Ok(appointments);
        }

        [Authorize(Roles = "Admin, Patient, Doctor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            try
            {
                var appointment = await _mediator.Send(new GetAppointmentDetailQuery { AppointmentId = id });
                return Ok(appointment); // Trả về chi tiết cuộc hẹn
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You do not have permission to view this appointment.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Trả về 400 nếu có lỗi
            }
        }
    }
}