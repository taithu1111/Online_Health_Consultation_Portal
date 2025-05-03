-- Clear existing data (if needed)
DELETE FROM MedicationDetails;
DELETE FROM Prescriptions;
DELETE FROM Payments;
DELETE FROM ConsultationSessions;
DELETE FROM Appointments;
DELETE FROM Doctors;
DELETE FROM Patients;
DELETE FROM AspNetUserRoles;
DELETE FROM AspNetRoles;
DELETE FROM AspNetUsers;
DELETE FROM Specializations;

-- Insert roles
SET IDENTITY_INSERT AspNetRoles ON;
INSERT INTO AspNetRoles (Id, Name, NormalizedName, Permissions, ConcurrencyStamp)
VALUES 
(1, 'Admin', 'ADMIN', 'All', NEWID()),
(2, 'Doctor', 'DOCTOR', 'ManageAppointments,ManagePrescriptions', NEWID()),
(3, 'Patient', 'PATIENT', 'ViewAppointments,ViewPrescriptions', NEWID());
SET IDENTITY_INSERT AspNetRoles OFF;

-- Insert users (doctors and patients)
SET IDENTITY_INSERT AspNetUsers ON;
INSERT INTO AspNetUsers (Id, Email, NormalizedEmail, PasswordHash, FullName, Gender, CreatedAt, Role, ResetPasswordToken, UserName, NormalizedUserName, EmailConfirmed, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES 
-- Doctor 1
(1, 'doctor1@example.com', 'DOCTOR1@EXAMPLE.COM', CONVERT(varbinary, 'hashedpassword1'), 'Dr. John Smith', 'Male', GETDATE(), 'Doctor', '', 'doctor1', 'DOCTOR1', 1, NEWID(), NEWID(), '1234567890', 1, 0, NULL, 0, 0),
-- Doctor 2
(2, 'doctor2@example.com', 'DOCTOR2@EXAMPLE.COM', CONVERT(varbinary, 'hashedpassword2'), 'Dr. Sarah Johnson', 'Female', GETDATE(), 'Doctor', '', 'doctor2', 'DOCTOR2', 1, NEWID(), NEWID(), '2345678901', 1, 0, NULL, 0, 0),
-- Patient 1
(3, 'patient1@example.com', 'PATIENT1@EXAMPLE.COM', CONVERT(varbinary, 'hashedpassword3'), 'Robert Brown', 'Male', GETDATE(), 'Patient', '', 'patient1', 'PATIENT1', 1, NEWID(), NEWID(), '3456789012', 1, 0, NULL, 0, 0),
-- Patient 2
(4, 'patient2@example.com', 'PATIENT2@EXAMPLE.COM', CONVERT(varbinary, 'hashedpassword4'), 'Emily Davis', 'Female', GETDATE(), 'Patient', '', 'patient2', 'PATIENT2', 1, NEWID(), NEWID(), '4567890123', 1, 0, NULL, 0, 0);
SET IDENTITY_INSERT AspNetUsers OFF;

-- Assign roles to users
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES 
(1, 2), -- Doctor 1 has Doctor role
(2, 2), -- Doctor 2 has Doctor role
(3, 3), -- Patient 1 has Patient role
(4, 3); -- Patient 2 has Patient role

-- Insert specializations
SET IDENTITY_INSERT Specializations ON;
INSERT INTO Specializations (Id, Name, Description)
VALUES 
(1, 'Cardiology', 'Deals with disorders of the heart and cardiovascular system'),
(2, 'Dermatology', 'Focuses on diseases of the skin, hair, and nails'),
(3, 'Neurology', 'Deals with disorders of the nervous system');
SET IDENTITY_INSERT Specializations OFF;

-- Insert patient records
INSERT INTO Patients (UserId, DateOfBirth, Gender, Phone, Address)
VALUES 
(3, '1985-05-15', 'Male', '3456789012', '123 Main St, Anytown'),
(4, '1990-08-22', 'Female', '4567890123', '456 Oak Ave, Somewhere');

-- Insert doctor records
INSERT INTO Doctors (UserId, SpecializationId, ExperienceYears, Languages, Bio, ConsultationFee, AverageRating)
VALUES 
(1, 1, 10, 'English, Spanish', 'Experienced cardiologist with focus on preventive care', 150.00, 4.8),
(2, 3, 8, 'English, French', 'Neurologist specializing in headache disorders and stroke management', 175.00, 4.7);

-- Insert appointments
SET IDENTITY_INSERT Appointments ON;
INSERT INTO Appointments (Id, PatientId, DoctorId, AppointmentDateTime, Status, Type, Notes, Diagnosis)
VALUES 
(1, 3, 1, DATEADD(day, -5, GETDATE()), 'Completed', 'Online', 'Regular checkup', 'Mild hypertension'),
(2, 3, 2, DATEADD(day, -2, GETDATE()), 'Completed', 'In-person', 'Headache consultation', 'Tension headache'),
(3, 4, 1, DATEADD(day, -1, GETDATE()), 'Completed', 'Online', 'Follow-up appointment', 'Improving condition'),
(4, 4, 2, DATEADD(day, 2, GETDATE()), 'Scheduled', 'Online', 'Initial consultation', NULL),
(5, 3, 1, DATEADD(day, 5, GETDATE()), 'Scheduled', 'In-person', 'Follow-up for medication adjustment', NULL);
SET IDENTITY_INSERT Appointments OFF;

-- Insert consultation sessions for completed appointments
SET IDENTITY_INSERT ConsultationSessions ON;
INSERT INTO ConsultationSessions (Id, AppointmentId, StartTime, EndTime, SessionNotes, MeetingUrl)
VALUES 
(1, 1, DATEADD(day, -5, GETDATE()), DATEADD(minute, 30, DATEADD(day, -5, GETDATE())), 'Patient reported occasional headaches. BP was 140/90.', 'https://meeting.example.com/session1'),
(2, 2, DATEADD(day, -2, GETDATE()), DATEADD(minute, 45, DATEADD(day, -2, GETDATE())), 'Patient has been experiencing tension headaches for 2 weeks.', 'https://meeting.example.com/session2'),
(3, 3, DATEADD(day, -1, GETDATE()), DATEADD(minute, 25, DATEADD(day, -1, GETDATE())), 'Follow-up showed improvement in symptoms.', 'https://meeting.example.com/session3');
SET IDENTITY_INSERT ConsultationSessions OFF;

-- Insert payments for completed appointments
SET IDENTITY_INSERT Payments ON;
INSERT INTO Payments (Id, AppointmentId, Amount, Status, TransactionId)
VALUES 
(1, 1, 150.00, 'Paid', 'TXN123456789'),
(2, 2, 175.00, 'Paid', 'TXN234567890'),
(3, 3, 150.00, 'Paid', 'TXN345678901');
SET IDENTITY_INSERT Payments OFF;

-- Insert prescriptions for completed appointments
SET IDENTITY_INSERT Prescriptions ON;
INSERT INTO Prescriptions (Id, AppointmentId, MedicationName, Dosage, Instructions)
VALUES 
(1, 1, 'Lisinopril', '10mg', 'Take one tablet daily in the morning with water'),
(2, 2, 'Ibuprofen', '400mg', 'Take one tablet every 6 hours as needed for pain'),
(3, 3, 'Atorvastatin', '20mg', 'Take one tablet daily in the evening with food');
SET IDENTITY_INSERT Prescriptions OFF;

-- Insert medication details
SET IDENTITY_INSERT MedicationDetails ON;
INSERT INTO MedicationDetails (Id, PrescriptionId, MedicationName, Dosage, Instructions)
VALUES 
-- For Prescription 1
(1, 1, 'Lisinopril', '10mg', 'Take one tablet daily in the morning with water'),
(2, 1, 'Aspirin', '81mg', 'Take one tablet daily with food'),
-- For Prescription 2
(3, 2, 'Ibuprofen', '400mg', 'Take one tablet every 6 hours as needed for pain'),
(4, 2, 'Cyclobenzaprine', '5mg', 'Take one tablet at bedtime for muscle relaxation'),
-- For Prescription 3
(5, 3, 'Atorvastatin', '20mg', 'Take one tablet daily in the evening with food');
SET IDENTITY_INSERT MedicationDetails OFF;
