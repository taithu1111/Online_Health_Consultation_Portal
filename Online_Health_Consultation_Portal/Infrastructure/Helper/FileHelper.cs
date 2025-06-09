namespace Online_Health_Consultation_Portal.Infrastructure.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> SaveImageAsync(IFormFile file, string relativePathFromRoot)
        {
            if (file == null || file.Length == 0)
                return null;

            // Validate file type and size
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid file type");

            if (file.Length > 5 * 1024 * 1024) // 5MB max
                throw new InvalidOperationException("File size exceeds limit");

            // Create a dedicated uploads directory in your backend
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile_images");
            
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return a path that can be used in URLs (relative to your web root)
            return $"/uploads/profile_images/{fileName}";
        }
    }
}