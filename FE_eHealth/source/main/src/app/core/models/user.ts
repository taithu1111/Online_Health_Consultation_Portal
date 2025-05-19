export interface User {
  token: string;
  username: string,
  email?: string,
  expires: Date,
  roles: string[],
  img?: string,
  firstName?: string,
  lastName?: string,
}

// import { Role } from './role';

// export class User {
//   id!: number;
//   img!: string;
//   username!: string;
//   password!: string;
//   firstName!: string;
//   lastName!: string;
//   role!: Role;
//   token!: string;
// }
