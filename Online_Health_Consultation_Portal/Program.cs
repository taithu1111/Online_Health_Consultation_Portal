using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Commands.Notifications;
using Online_Health_Consultation_Portal.Application.Mappings;
using Online_Health_Consultation_Portal.Application.Mappings.Admin;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repositories;
using Online_Health_Consultation_Portal.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddIdentity<User, Role>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

builder.Services.AddControllers();

// Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// Service
builder.Services.AddScoped<IUserManagementService, UserManagementService>();

// Mapping
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.AddAutoMapper(typeof(NotificationProfile).Assembly);
builder.Services.AddAutoMapper(typeof(DoctorProfile).Assembly);
builder.Services.AddAutoMapper(typeof(AdminUserProfile).Assembly);
builder.Services.AddMediatR(typeof(CreateNotificationCommand).Assembly);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin")); // Req
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
