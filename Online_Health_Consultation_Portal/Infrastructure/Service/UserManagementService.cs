using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;

        public UserManagementService(UserManager<User> userManager, AppDbContext dbContext) {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task PermanentlyDeleteUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            
            if (user == null) throw new KeyNotFoundException("User not found");
            if (user.Role == "Admin") 
                throw new InvalidOperationException("Cannot delete a different Admin.");
            
            await _userManager.UpdateAsync(user);
            
            // Hủy các appointment liên quan
            await _dbContext.Appointments
                .Where(a => a.PatientId == userId || a.DoctorId == userId)
                .ExecuteUpdateAsync(setters => 
                    setters.SetProperty(a => a.Status, "Cancelled"));
        }
    }
}