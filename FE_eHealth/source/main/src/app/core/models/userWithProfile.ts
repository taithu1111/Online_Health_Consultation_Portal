import { User } from "@core/service/user.service";
import { Doctors } from "app/admin/doctors/alldoctors/doctors.model";
import { PatientProfile } from "app/admin/patients/allpatients/patient.model";

export interface UserWithProfile {
  user: User;
  doctor?: Doctors | null;
  patient?: PatientProfile | null;
}
