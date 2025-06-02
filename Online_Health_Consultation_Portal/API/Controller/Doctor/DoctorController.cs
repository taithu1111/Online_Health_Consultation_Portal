// DoctorController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;
using Online_Health_Consultation_Portal.Application.Queries.Doctors;

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
}