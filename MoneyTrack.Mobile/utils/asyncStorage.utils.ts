import { ITransaction } from "@/models/transaction.model";
import { IUser } from "@/models/user.model";
import AsyncStorage from "@react-native-async-storage/async-storage";
import dayjs from "dayjs";

export interface ISetting {
  syncToCloud?: boolean;
}

export class Storage {
  public keys = {
    accessToken: "accessToken",
    refreshToken: "refreshToken",
    userProfile: "userProfile",
    transactionList: "transactionList",
    TransactionNeedToSync: "TransactionSync",
    setting: "setting",
    apiAIUsage: "apiAIUsage",
  };

  protected getArrayValue = async (key: string) => {
    try {
      const stringValue = await AsyncStorage.getItem(key);
      return stringValue == "null" || !stringValue ? [] : JSON.parse(stringValue);
    } catch (error) {
      console.error("Error getting array value:", error);
      return [];
    }
  };

  protected setArrayValue = async (key: string, value: any[]) => {
    try {
      await AsyncStorage.setItem(key, JSON.stringify(value));
    } catch (error) {
      console.error("Error setting array value:", error);
    }
  };

  storeAccessToken = async (token: string) => {
    try {
      await AsyncStorage.setItem(this.keys.accessToken, token);
    } catch (error) {
      console.error("Error storing access token:", error);
    }
  };

  storeUserProfile = async (userProfile: IUser) => {
    try {
      const stringValue = JSON.stringify(userProfile);
      await AsyncStorage.setItem(this.keys.userProfile, stringValue);
    } catch (error) {
      console.error("Error storing access token:", error);
    }
  };

  storeRefreshToken = async (token: string) => {
    try {
      await AsyncStorage.setItem(this.keys.refreshToken, token);
    } catch (error) {
      console.error("Error storing refresh token:", error);
    }
  };

  getAccessToken = async () => {
    try {
      return await AsyncStorage.getItem(this.keys.accessToken);
    } catch (error) {
      console.error("Error getting access token:", error);
      return null;
    }
  };

  getRefreshToken = async () => {
    try {
      return await AsyncStorage.getItem(this.keys.refreshToken);
    } catch (error) {
      console.error("Error getting refresh token:", error);
      return null;
    }
  };

  getUserProfile = async () => {
    try {
      const stringValue = await AsyncStorage.getItem(this.keys.userProfile);
      return stringValue == "null" || !stringValue ? {} : JSON.parse(stringValue);
    } catch (error) {
      console.error("Error getting access token:", error);
      return null;
    }
  };

  storeTransactionList = async (transactionList: ITransaction[]) => {
    try {
      await AsyncStorage.setItem(this.keys.transactionList, JSON.stringify(transactionList));
    } catch (error) {
      console.error("Error storing TRANSACTION_LIST_KEY:", error);
    }
  };

  getTransactionList = async (): Promise<any> => {
    try {
      const stringValue = await AsyncStorage.getItem(this.keys.transactionList);
      return stringValue == "null" || !stringValue ? [] : JSON.parse(stringValue);
    } catch (error) {
      console.error("Error getting TransactionList", error);
      return [];
    }
  };

  getSetting = async (): Promise<ISetting> => {
    try {
      const stringValue = await AsyncStorage.getItem(this.keys.setting);
      return stringValue == "null" || !stringValue ? {} : JSON.parse(stringValue);
    } catch (error) {
      return {};
    }
  };

  setSetting = async (setting: ISetting): Promise<ISetting> => {
    try {
      await AsyncStorage.setItem(this.keys.setting, JSON.stringify(setting));
    } catch (error) {
      return {};
    }
  };

  updateTodayApiAIUsage = async () => {
    try {
      const today = dayjs().format("DD-MM-YYYY");
      const stringValue = await AsyncStorage.getItem(this.keys.apiAIUsage);
      const apiAIUsage = stringValue ? JSON.parse(stringValue) : {};

      if (!apiAIUsage[today]) {
        apiAIUsage[today] = 1;
      } else {
        apiAIUsage[today] = apiAIUsage[today] + 1;
      }
      await AsyncStorage.setItem(this.keys.apiAIUsage, JSON.stringify(apiAIUsage));
    } catch (error) {
      console.error("Error updating API AI usage:", error);
    }
  };

  getTodayApiAIUsage = async (): Promise<number | null> => {
    try {
      const today = dayjs().format("DD-MM-YYYY");
      const stringValue = await AsyncStorage.getItem(this.keys.apiAIUsage);
      const apiAIUsage = stringValue ? JSON.parse(stringValue) : {};

      return apiAIUsage[today] || null;
    } catch (error) {
      console.error("Error getting API AI usage by date:", error);
      return null;
    }
  };

  clearTokens = async () => {
    try {
      await AsyncStorage.removeItem(this.keys.accessToken);
      await AsyncStorage.removeItem(this.keys.refreshToken);
      await AsyncStorage.removeItem(this.keys.userProfile);
    } catch (error) {
      console.error("Error clearing tokens:", error);
    }
  };
}

const appStorage = new Storage();

export default appStorage;
