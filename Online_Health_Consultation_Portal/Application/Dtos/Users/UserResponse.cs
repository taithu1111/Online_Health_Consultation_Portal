namespace Online_Health_Consultation_Portal.Application.Dtos.Users
{
    public sealed record UserResponse
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public string Role { get; init; }
        public DateTime CreatedAt { get; init; }
        // public DateTime? LastLogin { get; init; }
        
        // Cho admin thấy thông tin nhạy cảm
        public string? DeletionReason { get; init; }
    }
}