import { IUser } from "./user.model";

export interface ITokenResponse extends IUser {
  accessToken: string;
  refreshToken: string;
}
