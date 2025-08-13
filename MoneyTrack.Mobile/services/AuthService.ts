import { ITokenResponse } from "@/models/auth.model";
import appStorage from "@/utils/asyncStorage.utils";
// import { GoogleSignin } from "@react-native-google-signin/google-signin";
import BaseService from "./BaseService";
// import * as AppleAuthentication from "expo-apple-authentication";

export type LoginParams = {
  username: string;
  password: string;
};

class AuthService extends BaseService {
  public session: any = {};

  constructor() {
    super(`${process.env.EXPO_PUBLIC_API_URL}/api/Auth`);
    // GoogleSignin.configure({
    //   // TODO: move to env
    //   webClientId: "xxx", // From Google Console
    //   offlineAccess: true, // Allows retrieving refresh tokens
    // });
  }

  verifyOtp = async ({ email, otp }: { email: string; otp: string }): Promise<ITokenResponse> => {
    try {
      const res = await this.instance.post("/authen-by-email-verify", { isPublic: true, email, otp });

      await appStorage.storeAccessToken(res.data.accessToken);
      await appStorage.storeRefreshToken(res.data.refreshToken);

      const profileRes = await this.getProfile();
      await appStorage.storeUserProfile(profileRes.data);

      return { ...res.data, ...profileRes.data };
    } catch (error) {}
  };

  requestOtpAuthenticate = async ({ email }: { email: string }): Promise<string> => {
    const response = await this.instance.post("/authen-by-email", { isPublic: true, email });
    return response.data?.email;
  };

  login = async ({ username, password }: LoginParams): Promise<ITokenResponse> => {
    const loginRes = await this.instance.post("/login", { isPublic: true, username, password });
    await appStorage.storeAccessToken(loginRes.data.accessToken);
    await appStorage.storeRefreshToken(loginRes.data.refreshToken);

    const profileRes = await this.getProfile();
    await appStorage.storeUserProfile(profileRes.data);

    return { ...loginRes.data, ...profileRes.data };
  };

  getProfile = () => {
    return this.instance.post("/profile").then((profile) => {
      this.session.profile = profile.data;
      return profile;
    });
  };

  loginByGoogleAccount = async () => {
    try {
      // const res = await GoogleSignin.signIn();
      // const loginRes = await this.instance.post("/google-login", { token: res.data.idToken });
      // await appStorage.storeAccessToken(loginRes.data.accessToken);
      // await appStorage.storeRefreshToken(loginRes.data.refreshToken);
      // const profileRes = await this.getProfile();
      // await appStorage.storeUserProfile(profileRes.data);
      // return { ...loginRes.data, ...profileRes.data };
    } catch (error) {}
  };

  loginByAppleAccount = async () => {
    try {
      // TODO: Will apply in future
      // const credential = await AppleAuthentication.signInAsync({
      //   requestedScopes: [
      //     AppleAuthentication.AppleAuthenticationScope.FULL_NAME,
      //     AppleAuthentication.AppleAuthenticationScope.EMAIL,
      //   ],
      // });
      // const loginRes = await this.instance.post("/apple-login", { token: credential.identityToken });
      // await appStorage.storeAccessToken(loginRes.data.accessToken);
      // await appStorage.storeRefreshToken(loginRes.data.refreshToken);
      // const profileRes = await this.getProfile();
      // await appStorage.storeUserProfile(profileRes.data);
      // return { ...loginRes.data, ...profileRes.data };
    } catch (error) {}
  };

  deleteMyAccount = (): Promise<string> => {
    return this.instance.post("/remove-my-account");
  };
}

// Export singleton instance
export default new AuthService();
