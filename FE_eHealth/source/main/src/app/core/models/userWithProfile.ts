import { User } from "@core/service/user.service";
import { Doctor } from "@shared/components/operations-tbl-widget/operations-tbl-widget.component";
import { PatientProfile } from "app/admin/patients/allpatients/patient.model";

export interface UserWithProfile {
  user: User;
  doctor?: Doctor | null;
  patient?: PatientProfile | null;
}
