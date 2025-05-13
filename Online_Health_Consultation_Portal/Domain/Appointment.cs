using System.Numerics;

namespace Online_Health_Consultation_Portal.Domain
{
    public class Appointment
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Cancelled...
        public string Type { get; set; } // Online, Offline
        public string? Notes { get; set; }

        //Thêm mới trường này để lưu chuẩn đoán
        public string? Diagnosis { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public Prescription Prescription { get; set; }
        public Payment Payment { get; set; }
        //========= thêm
        public ConsultationSession ConsultationSession { get; set; }
        public Appointment(int patientId, int doctorId, DateTime appointmentDateTime, string? notes, string type)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentDateTime = appointmentDateTime;
            Notes = notes;
            Type = type;
        }

    }
}
