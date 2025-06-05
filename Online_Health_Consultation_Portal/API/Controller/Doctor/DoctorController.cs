// DoctorController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;
using Online_Health_Consultation_Portal.Application.Queries.Doctors;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.API.Controllers.Doctor
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors(
            [FromQuery] List<int>? specializations, // changed to int list
            [FromQuery] int? minExperienceYears,
            [FromQuery] string? language,
            [FromQuery] bool strictSpecializationFilter = false) // new param
        {
            var request = new DoctorListRequestDto
            {
                Specializations = specializations,
                MinExperienceYears = minExperienceYears,
                Language = language,
                StrictSpecializationFilter = strictSpecializationFilter
            };

            var query = new GetDoctorListQuery { Request = request };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
    }


    [ApiController]
    [Route("api/getById")]
    public class GetByIdController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GetByIdController(AppDbContext context) { _context = context; }

        [HttpGet("getDoctorIdByUserId/{userId}")]
        public async Task<IActionResult> GetDoctorIdByUserId(int userId)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null) return NotFound("Doctor not found");
            return Ok(doctor.Id); // đây là doctorId
        }

        [HttpGet("getPatientIdByUserId/{userId}")]
        public async Task<IActionResult> GetPatientIdByUserId(int userId)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null) return NotFound();
            return Ok(patient.Id); // PatientId
        }
    }
}