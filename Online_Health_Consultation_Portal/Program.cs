using Autofac;
using Autofac.Extensions.DependencyInjection;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Online_Health_Consultation_Portal.Infrastructure;
using System;
using System.Text;
=======
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Online_Health_Consultation_Portal.Infrastructure;
using System;
>>>>>>> 738aa228cdb979423fe5ed2525c5e724919a7378
using Online_Health_Consultation_Portal.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
<<<<<<< HEAD

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
=======
>>>>>>> 738aa228cdb979423fe5ed2525c5e724919a7378

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
<<<<<<< HEAD

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
=======
>>>>>>> 738aa228cdb979423fe5ed2525c5e724919a7378
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Health Consultation Portal API v1");
        c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root
    });

    // Enable detailed error information in development
    app.UseDeveloperExceptionPage();
}
else
{
    // Use more production-appropriate error handling
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Apply CORS policy - must be before routing
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseRouting();

<<<<<<< HEAD
// Authentication and authorization middleware must be after UseRouting and before UseEndpoints/MapControllers
app.UseAuthentication();
app.UseAuthorization();
=======
app.MapHub<ChatHub>("/chathub");

// Removed authentication and authorization middleware
>>>>>>> 738aa228cdb979423fe5ed2525c5e724919a7378

app.MapHub<ChatHub>("/chathub");
app.MapControllers();

// Optionally apply migrations automatically in development
if (app.Environment.IsDevelopment())
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        // Log the error - replace with your actual logging mechanism
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();
