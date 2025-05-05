using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Online_Health_Consultation_Portal.Application.Command.Appointment;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Application.Dtos.Auth.LoginDto;
using Online_Health_Consultation_Portal.Application.Handlers.Auth;
using Online_Health_Consultation_Portal.Application.Queries.Appointment;
using Online_Health_Consultation_Portal.Application.Services.Interfaces.Logging;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using Online_Health_Consultation_Portal.Infrastructure.Service;
using Online_Health_Consultation_Portal.Mappers.AutoMapping;
using System.Text;
using Serilog;
using Log = Serilog.Log;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Thêm DbContext vào DI container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));



Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
Log.Logger.Information("Application is building....");


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// cấu hình automapper
builder.Services.AddSingleton<IAutoMapper, AutoMapperProfile>();

// add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Đăng ký các repositories
builder.Services.AddScoped<IRepository<Appointment>, Repository<Appointment>>();
builder.Services.AddScoped<IRepository<AuditLog>, Repository<AuditLog>>();
builder.Services.AddScoped<IRepository<ConsultationSession>, Repository<ConsultationSession>>();
builder.Services.AddScoped<IRepository<Doctor>, Repository<Doctor>>();
builder.Services.AddScoped<IRepository<HealthRecord>, Repository<HealthRecord>>();
builder.Services.AddScoped<
    IRepository<Online_Health_Consultation_Portal.Domain.Log>,
    Repository<Online_Health_Consultation_Portal.Domain.Log>>();
builder.Services.AddScoped<IRepository<MedicationDetail>, Repository<MedicationDetail>>();
builder.Services.AddScoped<IRepository<Message>, Repository<Message>>();
builder.Services.AddScoped<IRepository<Notification>, Repository<Notification>>();
builder.Services.AddScoped<IRepository<Patient>, Repository<Patient>>();
builder.Services.AddScoped<IRepository<Payment>, Repository<Payment>>();
builder.Services.AddScoped<IRepository<Prescription>, Repository<Prescription>>();
builder.Services.AddScoped<IRepository<Rating>, Repository<Rating>>();
builder.Services.AddScoped<IRepository<Role>, Repository<Role>>();
builder.Services.AddScoped<IRepository<Schedule>, Repository<Schedule>>();
builder.Services.AddScoped<IRepository<Specialization>, Repository<Specialization>>();
builder.Services.AddScoped<IRepository<Statistic>, Repository<Statistic>>();
builder.Services.AddScoped<IRepository<SystemLog>, Repository<SystemLog>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();


//builder.Services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();



// Đăng ký các dịch vụ
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
//add logging 
builder.Services.AddScoped(typeof(IApplogger<>), typeof(SeriLoggerAdapter<>));

// Đăng ký JwtService
builder.Services.AddScoped<IJwtService, JwtService>();


// cấu hình JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //ValidateIssuer = false,
            //ValidateAudience = false,
            //ValidateLifetime = false,
            //ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))

            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)),
            ValidateLifetime = false,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

// add handler
builder.Services.AddScoped<IRequestHandler<CreateAppointmentCommand, int>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.CreateAppointmentHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateAppointmentCommand, bool>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.UpdateAppointmentHandler>();
builder.Services.AddScoped<IRequestHandler<CancelAppointmentCommand, bool>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.CancelAppointmentHandler>();

// Đăng ký các handlers
builder.Services.AddScoped<IRequestHandler<LoginCommand, LoginResponseDto>, LoginCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ForgotPasswordCommand, bool>, ForgotPasswordCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ResetPasswordCommand, bool>, ResetPasswordCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RegisterCommand, bool>, RegisterCommandHandler>();


// add queries
builder.Services.AddScoped<IRequestHandler<GetAppointmentDetailQuery, AppointmentDto>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.GetAppointmentDetailHandler>();
builder.Services.AddScoped<IRequestHandler<GetPatientAppointmentsQuery, List<AppointmentDto>>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.GetPatientAppointmentsHandler>();
builder.Services.AddScoped<IRequestHandler<GetDoctorAppointmentsQuery, List<AppointmentDto>>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.GetDoctorAppointmentsHandler>();




// Đăng ký các dịch vụ

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

try
{


    var app = builder.Build();
    app.UseSerilogRequestLogging();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // cấu hình swagger
    //builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen(c =>
    //{
    //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Online Health Consultation API", Version = "v1" });
    //});

    // Kích hoạt Swagger UI
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI(c =>
    //    {
    //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Health Consultation API v1");
    //    });
    //}
    app.UseAuthentication();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    Log.Logger.Information("Application is running .... ");

    app.Run();
}
catch(Exception ex)
{
    Log.Logger.Error(ex, "Application failed is running....");
}finally
{
    Log.CloseAndFlush();
};

