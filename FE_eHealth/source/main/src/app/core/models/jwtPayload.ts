export interface JwtPayload {
  nameid: string;
  role: string | string[];
  exp: number;
}