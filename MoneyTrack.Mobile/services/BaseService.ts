import { useAuthStore } from "@/stores/authStore";
import appStorage from "@/utils/asyncStorage.utils";
import axios, { AxiosError, AxiosInstance, AxiosResponse, InternalAxiosRequestConfig } from "axios";
import { router } from "expo-router";

export type CustomAxiosRequestConfig = {
  isPublic?: boolean;
} & InternalAxiosRequestConfig;

class BaseService {
  protected instance: AxiosInstance;

  protected readonly baseURL: string;

  private TIME_OUT = 60000;

  private MAX_RETRY_REFRESH_TOKEN = 2;

  private subscribersOriginalRequest: any[] = [];

  private retryRefreshToken = 0;

  private isRefreshToken = false;

  private readonly URL_REFRESH_TOKEN = "//TODO";

  public constructor(baseURL: string) {
    this.baseURL = baseURL;
    this.instance = axios.create({ baseURL, timeout: this.TIME_OUT });
    this.initializeRequestInterceptor();
    this.initializeResponseInterceptor();
  }

  private initializeRequestInterceptor = () => {
    this.instance.interceptors.request.use(this.handleRequest);
  };

  private initializeResponseInterceptor = () => {
    this.instance.interceptors.response.use((response: AxiosResponse<any>) => response, this.handlerError);
  };

  private handleRequest = async (config: CustomAxiosRequestConfig) => {
    const accessToken = await appStorage.getAccessToken();
    if (accessToken) {
      config.headers!.Authorization = `Bearer ${accessToken}`;
    }

    config.withCredentials = true;

    return config;
  };

  private handlerError = async (error: AxiosError) => {
    const originalRequest = error.config;

    const refreshToken = await appStorage.getRefreshToken();

    if (error.response?.status === 403) {
      // TODO: handle unauthorized
    }

    if (error.response?.status === 401) {
      useAuthStore.getState().clearAll();
      await appStorage.clearTokens();
      router.dismissTo("/");
    }

    // if (error.response?.status === 401 && refreshToken) {
    //   if (!this.isRefreshToken) {
    //     this.isRefreshToken = true;
    //     this.refreshToken(refreshToken);
    //   }

    //   return new Promise((resolve) =>
    //     this.subscribeRequest(async () => {
    //       const accessToken = await appStorage.getAccessToken();

    //       if (originalRequest) {
    //         originalRequest.headers!.Authorization = `Bearer ${accessToken}`;
    //         resolve(this.instance(originalRequest));
    //       }
    //       resolve({});
    //     })
    //   );
    // }

    // TODO: mapping errorCode to error message here
    throw error;
  };

  private async refreshToken(refreshToken: string): Promise<void> {
    try {
      this.retryRefreshToken += 1;
      const response = await axios.post<any>(this.URL_REFRESH_TOKEN, { refreshToken });
      await appStorage.storeAccessToken(response.data.accessToken);
      await appStorage.storeRefreshToken(response.data.refreshToken);

      this.executeRequests();
    } catch (error) {
      if (this.retryRefreshToken <= this.MAX_RETRY_REFRESH_TOKEN) {
        return this.refreshToken(refreshToken);
      }
      this.retryRefreshToken = 0;
      // TODO: handle logout
      throw error;
    } finally {
      this.isRefreshToken = false;
      this.subscribersOriginalRequest = [];
    }
  }

  private executeRequests = () => {
    this.subscribersOriginalRequest.map((executeRequest) => executeRequest());
  };

  private subscribeRequest: any = async (callback: () => null) => this.subscribersOriginalRequest.push(callback);
}

export default BaseService;
