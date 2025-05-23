using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
﻿using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Online_Health_Consultation_Portal.Application.Command.Appointment;
using Online_Health_Consultation_Portal.Application.Commands.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Commands.HealthRecord;
using Online_Health_Consultation_Portal.Application.Commands.Schedule;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Application.Dtos.Auth.LoginDto;
using Online_Health_Consultation_Portal.Application.Dtos.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Dtos.HealthRecord;
using Online_Health_Consultation_Portal.Application.Dtos.Schedule;
using Online_Health_Consultation_Portal.Application.Queries.Appointment;
using Online_Health_Consultation_Portal.Application.Services.Interfaces.Logging;
using Online_Health_Consultation_Portal.Application.Queries.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Queries.HealthRecord;
using Online_Health_Consultation_Portal.Application.Queries.Schedule;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using Online_Health_Consultation_Portal.Infrastructure.Service;
using Online_Health_Consultation_Portal.Mappers.AutoMapping;
using System;
using System.Text;
using Serilog;
using Log = Serilog.Log;
using Online_Health_Consultation_Portal.Domain.Entities;
using Online_Health_Consultation_Portal.Application.Interfaces.Repository;
using Online_Health_Consultation_Portal.Application.Interfaces.Service;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Application.Handlers.Auth;
using FluentValidation.AspNetCore;
using FluentValidation;
using Online_Health_Consultation_Portal.Application.Validator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Thêm DbContext vào DI container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add authentication
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
Log.Logger.Information("Application is building....");


builder.Services.AddControllers();

// Add Swagger/OpenAPI configuration
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

    // Add JWT authentication to Swagger
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
            new string[] {}
        }
    });
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    // Application & infrastructure registrations
    container.AddGenericHandlers();
});

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
    IRepository<Online_Health_Consultation_Portal.Domain.Entities.Log>,
    Repository<Online_Health_Consultation_Portal.Domain.Entities.Log>>();
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

// Đăng ký các dịch vụ
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
//add logging 
builder.Services.AddScoped(typeof(IApplogger<>), typeof(SeriLoggerAdapter<>));

// Đăng ký JwtService
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAuthBusinessRule, AuthBusinessRule>();



// cấu hình JWT
var jwtSettings = builder.Configuration.GetSection("JWT");

builder.Services.AddIdentity<User, Role>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;

        // Cấu hình yêu cầu mật khẩu mạnh
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddRoles<Role>() // Thêm hỗ trợ role
    .AddEntityFrameworkStores<AppDbContext>(); // DbContext 

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
            ValidateLifetime = false,
            ValidIssuer = jwtSettings["Issuer"],
        };
    });


//------------- add handler ---------------
//appointment
builder.Services.AddScoped<IRequestHandler<CreateAppointmentCommand, int>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.CreateAppointmentHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateAppointmentCommand, bool>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.UpdateAppointmentHandler>();
builder.Services.AddScoped<IRequestHandler<CancelAppointmentCommand, bool>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.CancelAppointmentHandler>();
//health record
builder.Services.AddScoped<IRequestHandler<CreateHealthRecordCommand, int>, Online_Health_Consultation_Portal.Application.Handlers.HealthRecord.CreateHealthRecordHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateHealthRecordCommand, bool>, Online_Health_Consultation_Portal.Application.Handlers.HealthRecord.UpdateHealthRecordHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteHealthRecordCommand, bool>, Online_Health_Consultation_Portal.Application.Handlers.HealthRecord.DeleteHealthRecordHandler>();
//consultation session
builder.Services.AddScoped<IRequestHandler<CreateConsultationSessionCommand, int>, Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession.CreateConsultationSessionHandler>();
builder.Services.AddScoped<IRequestHandler<StartConsultationSessionCommand, bool>, Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession.StartConsultationSessionHandler>();
builder.Services.AddScoped<IRequestHandler<EndConsultationSessionCommand, bool>, Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession.EndConsultationSessionHandler>();
//schedule
builder.Services.AddScoped<IRequestHandler<CreateScheduleCommand, int>, Online_Health_Consultation_Portal.Application.Handlers.Schedule.CreateScheduleHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateScheduleCommand, bool>, Online_Health_Consultation_Portal.Application.Handlers.Schedule.UpdateScheduleHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteScheduleCommand, bool>, Online_Health_Consultation_Portal.Application.Handlers.Schedule.DeleteScheduleHandler>();

// Đăng ký các handlers
builder.Services.AddScoped<IRequestHandler<LoginCommand, LoginResponseDto>, LoginCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ForgotPasswordCommand, bool>, ForgotPasswordCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ResetPasswordCommand, bool>, ResetPasswordCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RegisterCommand, bool>, RegisterCommandHandler>();


//------------- add queries ------------------
//appointment
builder.Services.AddScoped<IRequestHandler<GetAppointmentDetailQuery, AppointmentDto>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.GetAppointmentDetailHandler>();
builder.Services.AddScoped<IRequestHandler<GetPatientAppointmentsQuery, List<AppointmentDto>>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.GetPatientAppointmentsHandler>();
builder.Services.AddScoped<IRequestHandler<GetDoctorAppointmentsQuery, List<AppointmentDto>>, Online_Health_Consultation_Portal.Application.Handlers.Appointment.GetDoctorAppointmentsHandler>();
//health record
builder.Services.AddScoped<IRequestHandler<GetHealthRecordByPatientQuery, List<HealthRecordResponseDto>>, Online_Health_Consultation_Portal.Application.Handlers.HealthRecord.GetHealthRecordByPatientHandler > ();
//consultation session
builder.Services.AddScoped<IRequestHandler<GetConsultationSessionByIdQuery, ConsultationSessionDto>, Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession.GetConsultationSessionByIdHandler>();
builder.Services.AddScoped<IRequestHandler<GetConsultationsByDoctorQuery, List<ConsultationSessionDto>>, Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession.GetConsultationsByDoctorHandler>();
builder.Services.AddScoped<IRequestHandler<GetConsultationsByPatientQuery, List<ConsultationSessionDto>>, Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession.GetConsultationsByPatientHandler>();
//schedule
builder.Services.AddScoped<IRequestHandler<GetDoctorSchedulesQuery, List<ScheduleDto>>, Online_Health_Consultation_Portal.Application.Handlers.Schedule.GetDoctorSchedulesQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetAvailableSlotsQuery, List<AvailableSlotDto>>, Online_Health_Consultation_Portal.Application.Handlers.Schedule.GetAvailableSlotsQueryHandler>();

//add validator 
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();

builder.Services.AddHttpContextAccessor();


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

