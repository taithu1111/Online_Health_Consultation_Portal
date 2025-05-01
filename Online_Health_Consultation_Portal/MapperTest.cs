using AutoMapper;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;
using Online_Health_Consultation_Portal.Domain;
using Xunit;

public class MapperTests
{
    private readonly IMapper _mapper;

    public MapperTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DoctorProfile>();  // Đảm bảo rằng bạn đã cấu hình đúng profile của AutoMapper
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Should_Map_Doctor_To_DoctorDto()
    {
        // Arrange: Tạo một đối tượng Doctor giả lập.
        var doctor = new Doctor
        {
            UserId = 1,
            ExperienceYears = 10,
            Languages = "English, Spanish",
            Bio = "Experienced doctor",
            ConsultationFee = 200,
            AverageRating = 4.5,
            User = new User { FullName = "Dr. John Doe", Email = "john.doe@example.com" },
            Specialization = new Specialization { Name = "Cardiology" }
        };

        // Act: Ánh xạ từ Doctor sang DoctorDto.
        var doctorDto = _mapper.Map<DoctorDto>(doctor);

        // Assert: Kiểm tra các thuộc tính ánh xạ có đúng không.
        Assert.Equal(doctor.UserId, doctorDto.UserId);
        Assert.Equal(doctor.User.FullName, doctorDto.FullName);
        Assert.Equal(doctor.User.Email, doctorDto.Email);
        Assert.Equal(doctor.Specialization.Name, doctorDto.Specialization);
        Assert.Equal(doctor.ExperienceYears, doctorDto.ExperienceYears);
        Assert.Equal(doctor.Languages, doctorDto.Languages);
        Assert.Equal(doctor.Bio, doctorDto.Bio);
        Assert.Equal(doctor.ConsultationFee, doctorDto.ConsultationFee);
        Assert.Equal(doctor.AverageRating, doctorDto.AverageRating);
    }
}
