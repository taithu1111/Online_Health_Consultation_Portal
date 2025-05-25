namespace Online_Health_Consultation_Portal.Application.Dtos.Users
{
    public sealed record UserResponse
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public string Role { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}