export interface JwtPayload {
  id: number;
  role: string | string[];
  exp: number;
  email?: string;
}