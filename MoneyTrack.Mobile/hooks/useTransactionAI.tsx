import { Toast, ToastDescription, ToastTitle, useToast } from "@/components/ui/toast";
import TransactionService from "@/services/TransactionService";
import { useAuthStore } from "@/stores/authStore";
import appStorage from "@/utils/asyncStorage.utils";
import { useMutation } from "@tanstack/react-query";
import { useRouter } from "expo-router";
import uuid from "react-native-uuid";

export const useTransactionAI = ({ onSuccess }: any) => {
  const toast = useToast();
  const router = useRouter();
  const userProfile = useAuthStore((state) => state.userProfile);

  const checkAPIAIUsage = async () => {
    const usage: number = await appStorage.getTodayApiAIUsage();
    if (usage > 3) {
      router.push({
        pathname: "/authentication",
        params: { message: "Bạn đã sử dụng hết số lần sử dụng AI hôm nay. T^T" },
      });
      return false;
    }
    return true;
  };

  const transactionAIMutation = useMutation({
    mutationKey: ["transactionAI", userProfile],
    mutationFn: async (message: string) => {
      if (userProfile?.id) {
        return TransactionService.askPrivateAI(message);
      }
      const isValid = await checkAPIAIUsage();
      if (isValid) {
        return TransactionService.askAI(message);
      }
      return Promise.reject(new Error("API_LIMIT_EXCEEDED"));
    },
    onSuccess: async (data) => {
      if ((data as any)?.errorCode === "API_LIMIT_EXCEEDED") {
        toast.show({
          id: uuid.v4(),
          placement: "top",
          duration: 20000,
          render: ({ id }) => {
            return (
              <Toast nativeID={id} action="warning" variant="solid">
                <ToastDescription>Xin lỗi, Bạn đã sử dụng hết số lần sử dụng AI hôm nay rồi. T^T</ToastDescription>
              </Toast>
            );
          },
        });
      }
      if (data && data.category) {
        await appStorage.updateTodayApiAIUsage();
        onSuccess?.(data);
      }
    },
    onError: (error) => {
      toast.show({
        id: uuid.v4(),
        placement: "top",
        duration: 8000,
        render: ({ id }) => {
          return (
            <Toast nativeID={id} action="warning" variant="solid">
              <ToastTitle>AI: Xin lỗi, tôi không hiểu</ToastTitle>
              {/* <ToastDescription>Mô tả nên theo quy tắc "Hành động" + "Số tiền"</ToastDescription> */}
              <ToastDescription>{error.message}</ToastDescription>
            </Toast>
          );
        },
      });
    },
  });

  return {
    getTransactionByAI: transactionAIMutation.mutate,
    isAskingAI: transactionAIMutation.isPending,
  };
};

export default useTransactionAI;
