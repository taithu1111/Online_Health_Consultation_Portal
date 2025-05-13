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

            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (user.Role == "Admin")
                throw new InvalidOperationException("Cannot delete a different Admin.");

            // Cancel related appointments
            await _dbContext.Appointments
                .Where(a => a.PatientId == userId || a.DoctorId == userId)
                .ExecuteUpdateAsync(setters => 
                    setters.SetProperty(a => a.Status, "Cancelled"));

            // Remove related health records, messages, etc. if needed
            // Example (optional cleanup depending on cascade delete settings):
            // _dbContext.Messages.RemoveRange(_dbContext.Messages.Where(m => m.SenderId == userId || m.ReceiverId == userId));
            // _dbContext.Notifications.RemoveRange(_dbContext.Notifications.Where(n => n.UserId == userId));
            // await _dbContext.SaveChangesAsync();

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
