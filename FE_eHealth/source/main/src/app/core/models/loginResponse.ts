export interface LoginResponse {
  token: string;
  expires: string;
  roles: string[];
}