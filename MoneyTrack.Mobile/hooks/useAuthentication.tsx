import AuthService from "@/services/AuthService";
import { useAuthStore } from "@/stores/authStore";
import appStorage from "@/utils/asyncStorage.utils";
import { useMutation } from "@tanstack/react-query";

export const useAuthentication = () => {
  const authStore = useAuthStore();

  /**
   * @deprecated
   */
  const loginMutation = useMutation({
    mutationKey: ["login"],
    mutationFn: AuthService.login,
    onSuccess: (res) => {
      authStore.setToken(res);
    },
    onError: (error) => {},
  });

  const googleSigninMutation = useMutation({
    mutationKey: ["google-signin"],
    mutationFn: AuthService.loginByGoogleAccount,
    onSuccess: (res) => {
      // authStore.setToken(res);
    },
    onError: (error) => {},
  });

  const appleSigninMutation = useMutation({
    mutationKey: ["apple-signin"],
    mutationFn: AuthService.loginByAppleAccount,
    onSuccess: (res) => {
      // authStore.setToken(res);
    },
    onError: (error) => {},
  });

  const otpMutation = useMutation({
    mutationKey: ["verify-otp"],
    mutationFn: AuthService.verifyOtp,
    onSuccess: (res) => {
      authStore.setToken(res);
    },
  });

  const requestOtpAuthenticationByEmail = useMutation({
    mutationKey: ["authenticate-by-email"],
    mutationFn: AuthService.requestOtpAuthenticate,
  });

  const checkLoginAsync = async () => {
    try {
      const accessToken = await appStorage.getAccessToken();
      const refreshToken = await appStorage.getRefreshToken();
      const userProfile = await appStorage.getUserProfile();
      if (!userProfile?.id) {
        await logout();
        return;
      }
      authStore.setToken({
        accessToken,
        refreshToken,
        ...userProfile,
      });
    } catch (error) {
      await logout();
    }
  };

  const logout = async () => {
    await authStore.clearAll();
    await appStorage.clearTokens();
  };

  const deleteMyAccountMutation = useMutation({
    mutationKey: ["delete-my-account"],
    mutationFn: AuthService.deleteMyAccount,
  });

  return {
    isLoading: otpMutation.isPending || requestOtpAuthenticationByEmail.isPending,
    login: loginMutation.mutate,
    requestOtpAuthenticationByEmailAsync: requestOtpAuthenticationByEmail.mutateAsync,
    verifyOtpAsync: otpMutation.mutateAsync,
    checkLoginAsync,
    isOtpError: otpMutation.isError,
    logout,
    googleSignin: googleSigninMutation.mutate,
    appleSignin: appleSigninMutation.mutate,
    deleteMyAccount: deleteMyAccountMutation.mutateAsync,
    isLoadingDeleteMyAccount: deleteMyAccountMutation.isPending,
  };
};

export default useAuthentication;
