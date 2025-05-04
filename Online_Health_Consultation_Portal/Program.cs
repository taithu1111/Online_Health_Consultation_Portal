using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Online_Health_Consultation_Portal.Application.Command.Appointment;
using Online_Health_Consultation_Portal.Application.Commands.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Commands.HealthRecord;
using Online_Health_Consultation_Portal.Application.Commands.Schedule;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Application.Dtos.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Dtos.HealthRecord;
using Online_Health_Consultation_Portal.Application.Dtos.Schedule;
using Online_Health_Consultation_Portal.Application.Handlers.Appointment;
using Online_Health_Consultation_Portal.Application.Queries.Appointment;
using Online_Health_Consultation_Portal.Application.Queries.ConsultationSession;
using Online_Health_Consultation_Portal.Application.Queries.HealthRecord;
using Online_Health_Consultation_Portal.Application.Queries.Schedule;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using Online_Health_Consultation_Portal.Mappers.AutoMapping;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Thêm DbContext vào DI container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// cấu hình automapper
builder.Services.AddSingleton<IAutoMapper, AutoMapperProfile>();

// add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));


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
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
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

// add repository

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


// Đăng ký các dịch vụ

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

var app = builder.Build();
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

app.Run();
