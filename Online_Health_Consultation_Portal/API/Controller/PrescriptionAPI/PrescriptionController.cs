using MediatR;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.API.DTo;
using Online_Health_Consultation_Portal.Application.CQRS.Command;
using Online_Health_Consultation_Portal.Application.CQRS.Querries;
using Online_Health_Consultation_Portal.Application.Dtos.Precription;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.API.Controller.PrescriptionAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrescriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Tạo đơn thuốc (Create Prescription)
        /// </summary>
        /// <param name="prescriptionDto">Prescription details</param>
        /// <returns>The created prescription</returns>
        [HttpPost]
        [ProducesResponseType(typeof(PrescriptionResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionDto prescriptionDto)
        {
            try
            {
                // Log the received DTO for debugging
                Console.WriteLine($"Received DTO: AppointmentId={prescriptionDto.AppointmentId}, Instructions={prescriptionDto.Instructions}, Medications count={prescriptionDto.Medications?.Count ?? 0}");

                // Convert DTO medication items to domain medication details
                var medicationDetails = prescriptionDto.Medications?.Select(m => new MedicationDetail
                {
                    MedicationName = m.Name,
                    Dosage = m.Dosage,
                    Instructions = m.Instructions
                }).ToList() ?? new List<MedicationDetail>();

                // Extract values from the first medication in the list, or use default values if none exist
                var firstMedication = prescriptionDto.Medications?.FirstOrDefault();
                var medicationName = firstMedication?.Name ?? "General Medication";
                var dosage = firstMedication?.Dosage ?? "As directed";

                // Map DTO to command
                var command = new CreatePrescriptionCommand(
                    AppointmentId: prescriptionDto.AppointmentId,
                    MedicationName: medicationName,
                    Dosage: dosage,
                    Instructions: prescriptionDto.Instructions,
                    MedicationDetails: medicationDetails
                );

                var result = await _mediator.Send(command);

                // Map result to response DTO
                var responseDto = new PrescriptionResponseDto
                {
                    Id = result.Id,
                    AppointmentId = result.AppointmentId,
                    PatientId = result.Appointment?.PatientId ?? 0,
                    DoctorId = result.Appointment?.DoctorId ?? 0,
                    PatientName = result.Appointment?.Patient?.User?.FullName ?? "Unknown Patient",
                    DoctorName = result.Appointment?.Doctor?.User?.FullName ?? "Unknown Doctor",
                    Notes = result.Instructions,
                    CreatedDate = DateTime.UtcNow,
                    Medications = result.MedicationDetails?.Select(m => new MedicationItemDto
                    {
                        Id = m.Id,
                        MedicationId = null,
                        Name = m.MedicationName,
                        Dosage = m.Dosage,
                        Frequency = string.Empty,
                        Duration = string.Empty,
                        Instructions = m.Instructions
                    }).ToList() ?? new List<MedicationItemDto>()
                };

                return CreatedAtAction(nameof(GetPrescriptionById), new { prescriptionId = responseDto.Id }, responseDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error creating prescription: {ex.Message}");
                return BadRequest($"Error creating prescription: {ex.Message}");
            }
        }

        /// <summary>
        /// Danh sách đơn thuốc bệnh nhân (List of patient prescriptions)
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>List of prescriptions for the patient</returns>
        [HttpGet("patient/{patientId}")]
        [ProducesResponseType(typeof(IEnumerable<PrescriptionListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPrescriptionsByPatientId(int patientId)
        {
            var query = new GetPrescriptionsByPatientIdQuery { PatientId = patientId };
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
                return NotFound();

            // Map result to response DTOs
            var responseDtos = result.Select(p => new PrescriptionListItemDto
            {
                Id = p.Id,
                AppointmentId = p.AppointmentId,
                DoctorName = p.Appointment?.Doctor?.User?.FullName ?? "Unknown Doctor",
                CreatedDate = DateTime.UtcNow,
                MedicationCount = p.MedicationDetails?.Count ?? 0
            }).ToList();

            return Ok(responseDtos);
        }

        /// <summary>
        /// Chi tiết đơn thuốc (Prescription details)
        /// </summary>
        /// <param name="prescriptionId">Prescription ID</param>
        /// <returns>Details of the specified prescription</returns>
        [HttpGet("{prescriptionId}")]
        [ProducesResponseType(typeof(PrescriptionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPrescriptionById(int prescriptionId)
        {
            var query = new GetPrescriptionByIdQuery { PrescriptionId = prescriptionId };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            // Map result to response DTO
            var responseDto = new PrescriptionResponseDto
            {
                Id = result.Id,
                AppointmentId = result.AppointmentId,
                PatientId = result.Appointment?.PatientId ?? 0,
                DoctorId = result.Appointment?.DoctorId ?? 0,
                PatientName = result.Appointment?.Patient?.User?.FullName ?? "Unknown Patient",
                DoctorName = result.Appointment?.Doctor?.User?.FullName ?? "Unknown Doctor",
                Notes = result.Instructions,
                CreatedDate = DateTime.UtcNow,
                Medications = result.MedicationDetails?.Select(m => new MedicationItemDto
                {
                    Id = m.Id,
                    MedicationId = null,
                    Name = m.MedicationName,
                    Dosage = m.Dosage,
                    Frequency = string.Empty,
                    Duration = string.Empty,
                    Instructions = m.Instructions
                }).ToList() ?? new List<MedicationItemDto>()
            };

            return Ok(responseDto);
        }

        /// <summary>
        /// Create a test prescription and add it to the database
        /// </summary>
        /// <param name="appointmentId">Optional appointment ID (defaults to 1 if not provided)</param>
        /// <returns>The created prescription</returns>
        [HttpGet("test")]
        [ProducesResponseType(typeof(PrescriptionResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTestPrescription([FromQuery] int appointmentId = 1)
        {
            try
            {
                // Create test medication details
                var medicationDetails = new List<MedicationDetail>
                {
                    new MedicationDetail
                    {
                        MedicationName = "Paracetamol",
                        Dosage = "500mg",
                        Instructions = "Take with food every 6 hours for 5 days"
                    },
                    new MedicationDetail
                    {
                        MedicationName = "Amoxicillin",
                        Dosage = "250mg",
                        Instructions = "Take three times daily for 7 days. Complete the full course."
                    }
                };

                // Create the command to add a prescription
                var command = new CreatePrescriptionCommand(
                    AppointmentId: appointmentId,
                    MedicationName: "Test Prescription",
                    Dosage: "As directed",
                    Instructions: "Take medications as prescribed. Follow up in two weeks if symptoms persist.",
                    MedicationDetails: medicationDetails
                );

                // Send the command to create a real prescription in the database
                var result = await _mediator.Send(command);

                // Map the result to response DTO (same as in CreatePrescription)
                var responseDto = new PrescriptionResponseDto
                {
                    Id = result.Id,
                    AppointmentId = result.AppointmentId,
                    PatientId = result.Appointment?.PatientId ?? 0,
                    DoctorId = result.Appointment?.DoctorId ?? 0,
                    PatientName = result.Appointment?.Patient?.User?.FullName ?? "Unknown Patient",
                    DoctorName = result.Appointment?.Doctor?.User?.FullName ?? "Unknown Doctor",
                    Notes = result.Instructions,
                    CreatedDate = DateTime.UtcNow,
                    Medications = result.MedicationDetails?.Select(m => new MedicationItemDto
                    {
                        Id = m.Id,
                        MedicationId = null,
                        Name = m.MedicationName,
                        Dosage = m.Dosage,
                        Frequency = string.Empty,
                        Duration = string.Empty,
                        Instructions = m.Instructions
                    }).ToList() ?? new List<MedicationItemDto>()
                };

                return CreatedAtAction(nameof(GetPrescriptionById), new { prescriptionId = responseDto.Id }, responseDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error creating test prescription: {ex.Message}");
                return BadRequest($"Error creating test prescription: {ex.Message}");
            }
        }
    }
}