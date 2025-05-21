namespace Online_Health_Consultation_Portal.Domain
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        //public DateTime AssignedDate { get; set; }


    }
}
