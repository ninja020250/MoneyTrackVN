export interface IUser {
  id: string;
  username: string;
  email?: string;
  userRoles: UserRole[];
}

export interface UserRole {
  id: string;
  name: string;
  description: string;
}
