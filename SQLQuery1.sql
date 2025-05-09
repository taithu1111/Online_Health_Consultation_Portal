-- Insert test data into AspNetRoles
INSERT INTO AspNetRoles (Name, Permissions, NormalizedName, ConcurrencyStamp)
VALUES 
('Admin', 'AdminPermissions', 'ADMIN', '00000000-0000-0000-0000-000000000001'),
('Doctor', 'DoctorPermissions', 'DOCTOR', '00000000-0000-0000-0000-000000000002'),
('Patient', 'PatientPermissions', 'PATIENT', '00000000-0000-0000-0000-000000000003');

-- Insert test data into AspNetUsers
INSERT INTO AspNetUsers (Email, PasswordHash, FullName, Gender, CreatedAt, Role, ResetPasswordToken, ResetPasswordTokenExpiry, UserName, NormalizedUserName, NormalizedEmail, EmailConfirmed, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES 
('admin@example.com', HASHBYTES('SHA2_256', 'AdminPassword123!'), 'Admin User', 'Male', GETDATE(), 'Admin', 'Token123', NULL, 'admin', 'ADMIN', 'ADMIN@EXAMPLE.COM', 1, '00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000001', NULL, 0, 0, NULL, 0, 0),
('doctor@example.com', HASHBYTES('SHA2_256', 'DoctorPassword123!'), 'Dr. John Doe', 'Male', GETDATE(), 'Doctor', 'Token456', NULL, 'doctor', 'DOCTOR', 'DOCTOR@EXAMPLE.COM', 1, '00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000002', NULL, 0, 0, NULL, 0, 0),
('patient@example.com', HASHBYTES('SHA2_256', 'PatientPassword123!'), 'Jane Doe', 'Female', GETDATE(), 'Patient', 'Token789', NULL, 'patient', 'PATIENT', 'PATIENT@EXAMPLE.COM', 1, '00000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000003', NULL, 0, 0, NULL, 0, 0);

-- Insert test data into AspNetUserRoles
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'admin@example.com' AND r.Name = 'Admin'
UNION ALL
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'doctor@example.com' AND r.Name = 'Doctor'
UNION ALL
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'patient@example.com' AND r.Name = 'Patient';

-- Insert test data into Specializations
INSERT INTO Specializations (Name, Description)
VALUES 
('Cardiology', 'Specialization in heart and cardiovascular system'),
('Dermatology', 'Specialization in skin disorders'),
('Pediatrics', 'Specialization in children''s health');

-- Insert test data into Doctors
INSERT INTO Doctors (UserId, SpecializationId, ExperienceYears, Languages, Bio, ConsultationFee, AverageRating)
SELECT u.Id, s.Id, 10, 'English, Spanish', 'Experienced Cardiologist', 100.00, 4.5
FROM AspNetUsers u, Specializations s
WHERE u.Email = 'doctor@example.com' AND s.Name = 'Cardiology';

-- Insert test data into Patients
INSERT INTO Patients (UserId, DateOfBirth, Gender, Phone, Address)
SELECT u.Id, '1990-01-01', 'Female', '123-456-7890', '123 Main St'
FROM AspNetUsers u
WHERE u.Email = 'patient@example.com';

-- Insert test data into Appointments
INSERT INTO Appointments (PatientId, DoctorId, AppointmentDateTime, Status, Type, Notes, Diagnosis)
SELECT p.UserId, d.UserId, '2025-05-01 10:00:00', 'Scheduled', 'Online', 'Initial consultation', NULL
FROM Patients p, Doctors d
WHERE p.UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'patient@example.com')
AND d.UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'doctor@example.com');

-- Insert test data into ConsultationSessions
INSERT INTO ConsultationSessions (AppointmentId, StartTime, EndTime, SessionNotes, MeetingUrl)
SELECT a.Id, '2025-05-01 10:00:00', '2025-05-01 11:00:00', 'Session notes here', 'https://example.com/meeting'
FROM Appointments a
WHERE a.PatientId = (SELECT UserId FROM Patients WHERE UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'patient@example.com'));

-- Insert test data into Payments
INSERT INTO Payments (AppointmentId, Amount, Status, TransactionId)
SELECT a.Id, 100.00, 'Completed', 'TXN123456789'
FROM Appointments a
WHERE a.PatientId = (SELECT UserId FROM Patients WHERE UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'patient@example.com'));

-- Insert test data into Prescriptions
INSERT INTO Prescriptions (AppointmentId, MedicationName, Dosage, Instructions)
SELECT a.Id, 'Medicine A', '10mg', 'Take twice daily'
FROM Appointments a
WHERE a.PatientId = (SELECT UserId FROM Patients WHERE UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'patient@example.com'));

-- Insert test data into Ratings
INSERT INTO Ratings (DoctorId, PatientId, Value, Comment, CreatedAt)
SELECT d.UserId, p.UserId, 5, 'Excellent service!', GETDATE()
FROM Doctors d, Patients p
WHERE d.UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'doctor@example.com')
AND p.UserId = (SELECT Id FROM AspNetUsers WHERE Email = 'patient@example.com');