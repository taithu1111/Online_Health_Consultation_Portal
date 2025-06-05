using System.Security.Claims;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Online_Health_Consultation_Portal.Application.Command.Appointment;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Application.Commands.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Commands.HealthRecord;
using Online_Health_Consultation_Portal.Application.Commands.Notifications;
using Online_Health_Consultation_Portal.Application.Commands.Schedule;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Application.Dtos.Auth.LoginDto;
using Online_Health_Consultation_Portal.Application.Dtos.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Dtos.HealthRecord;
using Online_Health_Consultation_Portal.Application.Dtos.Schedule;
using Online_Health_Consultation_Portal.Application.Handlers.Appointment;
using Online_Health_Consultation_Portal.Application.Handlers.Auth;
using Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Handlers.HealthRecord;
using Online_Health_Consultation_Portal.Application.Handlers.Schedule;
using Online_Health_Consultation_Portal.Application.Mappings;
using Online_Health_Consultation_Portal.Application.Queries.Appointment;
using Online_Health_Consultation_Portal.Application.Queries.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Queries.HealthRecord;
using Online_Health_Consultation_Portal.Application.Queries.Schedule;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repositories;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using Online_Health_Consultation_Portal.Infrastructure.Service;
using Online_Health_Consultation_Portal.Infrastructure.Services;
using Online_Health_Consultation_Portal.Mappers.AutoMapping;
using Serilog;
using Log = Serilog.Log;
using Online_Health_Consultation_Portal.Application.Commands.Payment;
using Online_Health_Consultation_Portal.Application.Dtos.Payment;
using Online_Health_Consultation_Portal.Application.Queries.Payment;
using Online_Health_Consultation_Portal.Services;
using Microsoft.Extensions.FileProviders;
using Online_Health_Consultation_Portal.Application.Handlers.Payment;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();
Log.Logger.Information("Application is building....");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));
    
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey) && !AppDomain.CurrentDomain.FriendlyName.Contains("ef"))
{
    throw new InvalidOperationException("JWT key is missing from configuration.");
}

// Authentication Configuration
// 1. First, update your package references as shown previously
// 2. Then modify your authentication setup:

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured"),
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        // Add these additional parameters for better stability
        ClockSkew = TimeSpan.FromMinutes(5), // 5 mins
        NameClaimType = ClaimTypes.NameIdentifier,    // Standard claim type mapping
        RoleClaimType = ClaimTypes.Role     // Standard claim type mapping
    };

    // Add debugging events
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token successfully validated");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Online Health Consultation Portal API",
        Version = "v1",
        Description = "API for Online Health Consultation Portal",
        Contact = new OpenApiContact
        {
            Name = "Support Team",
            Email = "support@healthportal.com"
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Autofac Configuration
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//builder.Host.ConfigureContainer<ContainerBuilder>(container =>
//{
//    var isEf = AppDomain.CurrentDomain.FriendlyName.Contains("ef");

//    if (!isEf)
//    {
//        container.AddGenericHandlers();
//        Console.WriteLine("AddGenericHandlers success");
//    }
//});

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.AddAutoMapper(typeof(NotificationProfile).Assembly);
builder.Services.AddAutoMapper(typeof(DoctorProfile).Assembly);
builder.Services.AddSingleton<IAutoMapper, AutoMapperProfile>();

// MediatR Configuration
builder.Services.AddMediatR(typeof(CreateNotificationCommand).Assembly);
builder.Services.AddMediatR(typeof(Program).Assembly);

// Repository Registrations
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRepository<Appointment>, Repository<Appointment>>();
builder.Services.AddScoped<IRepository<AuditLog>, Repository<AuditLog>>();
builder.Services.AddScoped<IRepository<ConsultationSession>, Repository<ConsultationSession>>();
builder.Services.AddScoped<IRepository<Doctor>, Repository<Doctor>>();
builder.Services.AddScoped<IRepository<HealthRecord>, Repository<HealthRecord>>();
builder.Services.AddScoped<IRepository<Online_Health_Consultation_Portal.Domain.Log>, Repository<Online_Health_Consultation_Portal.Domain.Log>>();
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

// Service Registrations
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();
//builder.Services.AddScoped(typeof(IApplogger<>), typeof(SeriLoggerAdapter<>));

// Command Handlers
builder.Services.AddScoped<IRequestHandler<CreateAppointmentCommand, int>, CreateAppointmentHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateAppointmentCommand, bool>, UpdateAppointmentHandler>();
builder.Services.AddScoped<IRequestHandler<CancelAppointmentCommand, bool>, CancelAppointmentHandler>();
builder.Services.AddScoped<IRequestHandler<CreateHealthRecordCommand, int>, CreateHealthRecordHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateHealthRecordCommand, bool>, UpdateHealthRecordHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteHealthRecordCommand, bool>, DeleteHealthRecordHandler>();
builder.Services.AddScoped<IRequestHandler<CreateConsultationSessionCommand, int>, CreateConsultationSessionHandler>();
builder.Services.AddScoped<IRequestHandler<StartConsultationSessionCommand, bool>, StartConsultationSessionHandler>();
builder.Services.AddScoped<IRequestHandler<EndConsultationSessionCommand, bool>, EndConsultationSessionHandler>();
builder.Services.AddScoped<IRequestHandler<CreateScheduleCommand, int>, CreateScheduleHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateScheduleCommand, bool>, UpdateScheduleHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteScheduleCommand, bool>, DeleteScheduleHandler>();
builder.Services.AddScoped<IRequestHandler<LoginCommand, LoginResponseDto>, LoginCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ForgotPasswordCommand, bool>, ForgotPasswordCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ResetPasswordCommand, bool>, ResetPasswordCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RegisterCommand, bool>, RegisterCommandHandler>();
//Payment
builder.Services.AddScoped<IRequestHandler<CreatePaymentCommand, PaymentDto>, CreatePaymentHandler>();
builder.Services.AddScoped<IRequestHandler<UpdatePaymentStatusCommand, PaymentDto>, UpdatePaymentStatusHandler>();
builder.Services.AddScoped<IRequestHandler<DeletePaymentCommand, bool>, DeletePaymentHandler>();



// Query Handlers
builder.Services.AddScoped<IRequestHandler<GetAppointmentDetailQuery, AppointmentDto>, GetAppointmentDetailHandler>();
builder.Services.AddScoped<IRequestHandler<GetPatientAppointmentsQuery, List<AppointmentDto>>, GetPatientAppointmentsHandler>();
builder.Services.AddScoped<IRequestHandler<GetDoctorAppointmentsQuery, List<AppointmentDto>>, GetDoctorAppointmentsHandler>();
builder.Services.AddScoped<IRequestHandler<GetHealthRecordByPatientQuery, List<HealthRecordResponseDto>>, GetHealthRecordByPatientHandler>();
builder.Services.AddScoped<IRequestHandler<GetConsultationSessionByIdQuery, ConsultationSessionDto>, GetConsultationSessionByIdHandler>();
builder.Services.AddScoped<IRequestHandler<GetConsultationsByDoctorQuery, List<ConsultationSessionDto>>, GetConsultationsByDoctorHandler>();
builder.Services.AddScoped<IRequestHandler<GetConsultationsByPatientQuery, List<ConsultationSessionDto>>, GetConsultationsByPatientHandler>();
builder.Services.AddScoped<IRequestHandler<GetDoctorSchedulesQuery, List<ScheduleDto>>, GetDoctorSchedulesQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetAvailableSlotsQuery, List<AvailableSlotDto>>, GetAvailableSlotsQueryHandler>();
//payment
builder.Services.AddScoped<IRequestHandler<GetPaymentByIdQuery, PaymentDto>, GetPaymentByIdHandler>();
builder.Services.AddScoped<IRequestHandler<GetPaymentsByPatientQuery, IEnumerable<PaymentDto>>, GetPaymentsByPatientHandler>();


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

try
{
    app.UseSerilogRequestLogging();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
        RequestPath = "/uploads"
    });

    Log.Logger.Information("Application is running...");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Build app error: ");
    Console.WriteLine(ex.ToString());
    Log.Logger.Error(ex, "Application failed to start");
}