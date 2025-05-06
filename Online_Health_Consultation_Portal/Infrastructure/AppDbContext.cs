using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // thêm ============

        public DbSet<ConsultationSession> ConsultationSessions { get; set; }
        public DbSet<MedicationDetail> MedicationDetails { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Patient 1 - 1 User
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User);
            //.WithOne(u => u.Patient)
            //.HasForeignKey<Patient>(p => p.UserId)
            //.OnDelete(DeleteBehavior.Restrict);

            // Doctor 1 - 1 User
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User);
            //.WithOne(u => u.Doctor)
            //.HasForeignKey<Doctor>(d => d.UserId)
            //.OnDelete(DeleteBehavior.Restrict);

            // Doctor - Specialization
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment - Doctor
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserRole many-to-many
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // HealthRecord - Patient
            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Patient)
                .WithMany(p => p.HealthRecords)
                .HasForeignKey(hr => hr.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Notification - User
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User);
            //.WithMany(u => u.Notifications)
            //.HasForeignKey(n => n.UserId)
            //.OnDelete(DeleteBehavior.Cascade);

            // Rating - Doctor
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Doctor)
                .WithMany(d => d.Ratings)
                .HasForeignKey(r => r.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Rating - Patient
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Patient)
                .WithMany(p => p.Ratings)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Schedule - Doctor
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Doctor)
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);


            //=======================================
            // ConsultationSession - Appointment (1-1)
            modelBuilder.Entity<ConsultationSession>()
                .HasOne(cs => cs.Appointment)
                .WithOne(a => a.ConsultationSession)
                .HasForeignKey<ConsultationSession>(cs => cs.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // MedicationDetail - Prescription (1 - many)
            modelBuilder.Entity<MedicationDetail>()
                .HasOne(md => md.Prescription)
                .WithMany(p => p.MedicationDetails)
                .HasForeignKey(md => md.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Config AuditLog
            modelBuilder.Entity<AuditLog>()
                .HasOne(al => al.User)
                .WithMany()
                .HasForeignKey(al => al.UserId) 
                .OnDelete(DeleteBehavior.Restrict);

            // Config Statistic
            modelBuilder.Entity<Statistic>()
                .HasKey(s => s.StatisticId);

            // Config SystemLog
            modelBuilder.Entity<SystemLog>()
                .HasIndex(sl => sl.Timestamp);

        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //}

    }
}
