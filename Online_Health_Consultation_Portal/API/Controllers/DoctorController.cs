using MediatR;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Queries.Doctors;

namespace Online_Health_Consultation_Portal.API.Controllers
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
            [FromQuery] int? specializationId,
            [FromQuery] int? minExperienceYears,
            [FromQuery] string language)
        {
            var query = new GetDoctorListQuery
            {
                SpecializationId = specializationId,
                MinExperienceYears = minExperienceYears,
                Language = language
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}