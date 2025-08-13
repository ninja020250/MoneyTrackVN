import { ITokenResponse } from "@/models/auth.model";
import { IUser } from "@/models/user.model";
import { create } from "zustand";
import { immer } from "zustand/middleware/immer";

type State = {
  accessToken?: string;
  refreshToken?: string;
  userProfile?: IUser;
};

type Actions = {
  setToken: (tokenResponse: ITokenResponse) => void;
  setUserProfile: (userProfile: IUser) => void;
  clearAll: () => void;
};

export const useAuthStore = create<State & Actions>()(
  immer((set) => ({
    userProfile: undefined,
    accessToken: undefined,
    refreshToken: undefined,
    setToken: (tokenResponse: ITokenResponse) =>
      set((state) => {
        state.accessToken = tokenResponse.accessToken;
        state.refreshToken = tokenResponse.refreshToken;
        state.userProfile = {
          id: tokenResponse.id,
          username: tokenResponse.username,
          email: tokenResponse.email,
          userRoles: tokenResponse.userRoles,
        };
      }),
    setUserProfile: (userProfile: IUser) =>
      set((state) => {
        state.userProfile = userProfile;
      }),

    clearAll: () =>
      set((state) => {
        state.accessToken = undefined;
        state.refreshToken = undefined;
        state.userProfile = undefined;
      }),
  }))
);
