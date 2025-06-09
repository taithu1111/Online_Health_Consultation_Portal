namespace Online_Health_Consultation_Portal.Application.Dtos.Users
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}