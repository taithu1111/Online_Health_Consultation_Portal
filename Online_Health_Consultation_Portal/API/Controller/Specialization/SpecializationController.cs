using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Dtos.Specializations;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Services;

namespace Online_Health_Consultation_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationsController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecializationDto>>> GetAll()
        {
            var specializations = await _specializationService.GetAllSpecializationsAsync();
            return Ok(specializations);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SpecializationDto>>> Search([FromQuery] string term)
        {
            var specializations = await _specializationService.GetAllSpecializationsAsync(term);
            return Ok(specializations);
        }
    }
}