import Illustration from "@/components/Atom/Illustration";
import InputFormField from "@/components/molecules/InputFormField";
import { Button, ButtonText } from "@/components/ui/button";
import { Divider } from "@/components/ui/divider";
import { Spinner } from "@/components/ui/spinner";
import { Text } from "@/components/ui/text";
import { VStack } from "@/components/ui/vstack";
import { useAuthentication } from "@/hooks/useAuthentication";
import { useAuthStore } from "@/stores/authStore";
import { yupResolver } from "@hookform/resolvers/yup";
import { router, useLocalSearchParams } from "expo-router";
import React, { useEffect, useRef, useState } from "react";
import { useForm, useWatch } from "react-hook-form";
import { Keyboard, KeyboardAvoidingView, Linking, Platform, ScrollView, TouchableWithoutFeedback } from "react-native";
import * as yup from "yup";

interface FormData {
  email: string;
  otp: string;
}

const loginSchema = yup.object().shape({
  email: yup.string().email("Email chưa đúng định dạng").required("Nhập email để tiếp tục nào!"),
});

export default function AuthenticationScreen() {
  const [isOtp, setIsOtp] = useState(false);
  const { message } = useLocalSearchParams();
  const { isLoading, requestOtpAuthenticationByEmailAsync, verifyOtpAsync, isOtpError } = useAuthentication();
  const userProfile = useAuthStore((state) => state.userProfile);
  const otpRef = useRef(null);

  const { control, handleSubmit, setError, clearErrors, reset } = useForm<any>({
    resolver: yupResolver(loginSchema),
    defaultValues: {
      email: "",
      otp: "",
    },
  });

  const email = useWatch({ control, name: "email" });

  useEffect(() => {
    return () => {
      reset();
    };
  }, []);

  useEffect(() => {
    if (userProfile?.id) {
      router.dismissTo("/");
    }
  }, [userProfile?.id]);

  useEffect(() => {
    if (isOtpError) {
      setError("otp", { message: "OTP không đúng!" });
    } else {
      clearErrors("otp");
    }
  }, [isOtpError]);

  useEffect(() => {
    if (!email) {
      clearErrors("otp");
    }
  }, [email]);

  const onSubmit = (data: FormData) => {
    if (!isOtp) {
      return requestOtpAuthenticationByEmailAsync({
        email: data.email,
      })
        .then(() => {
          setIsOtp(true);
          otpRef.current?.focus(true);
        })
        .catch((error) => {
          alert(error);
        });
    }
    return verifyOtpAsync({ email: data.email, otp: data.otp });
  };

  return (
    <KeyboardAvoidingView keyboardVerticalOffset={47} behavior={"padding"} style={{ flex: 1 }}>
      <ScrollView contentContainerStyle={{ flexGrow: 1 }} keyboardShouldPersistTaps="handled">
        <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
          <VStack className="p-6 flex-1 justify-center">
            {/* <Image alt="logo" source={require("@/assets/images/moneytrack_logo.png")} className="h-20 w-20 mb-2" /> */}
            <Illustration name="logo" scale={0.4} width={0} height={110} />
            <VStack className="mb-8">
              <Text size="2xl" className="font-bold">
                Không còn giới hạn
              </Text>
              <Text size="2xl" className="font-bold text-typography-400">
                Đăng nhập để mở khóa tính năng
              </Text>
              {message && (
                <Text size="md" className="text-warning-600 mt-2">
                  {message}
                </Text>
              )}
            </VStack>
            {/* TODO: will apply soon */}
            {/* <VStack className="gap-2">
              <Button
                variant="outline"
                className="w-full border-background-300 relative"
                size="xl"
                onPress={() => googleSignin()}
              >
                <Image
                  alt="google"
                  source={require("@/assets/images/google.png")}
                  className="h-6 w-6 absolute left-4"
                />
                <ButtonText>Tiếp tục với Google Account</ButtonText>
              </Button>
              <Button
                variant="outline"
                className="w-full border-background-300 relative"
                size="xl"
                onPress={() => appleSignin()}
              >
                <Image
                  alt="apple"
                  source={require("@/assets/images/apple-logo.png")}
                  className="h-6 w-6 absolute left-4"
                />
                <ButtonText>Tiếp tục với Apple Account</ButtonText>
              </Button>
            </VStack> */}
            <Divider className="mt-2 mb-6" />
            <VStack className="w-full mb-4 gap-4">
              <InputFormField
                clearable
                label="Email"
                placeholder="Tiếp tục bằng email"
                control={control}
                name="email"
              />
              <InputFormField
                hidden={!isOtp || !email}
                clearable
                ref={otpRef}
                label="Mã xác thực"
                placeholder="Mã xác thực đã gửi qua email"
                control={control}
                name="otp"
              />
            </VStack>
            <Text className="text-center text-sm text-muted-foreground mb-10">
              Bằng cách tiếp tục, bạn đồng ý với{" "}
              <Text className="underline text-sm" onPress={() => router.push("/termAndCondition")}>
                Điều khoản sử dụng
              </Text>{" "}
              và{" "}
              <Text className="underline text-sm" onPress={() => router.push("/termAndCondition")}>
                Chính sách bảo mật{" "}
              </Text>
              của chúng tôi
            </Text>
            <Button className="mt-0" variant="solid" size="xl" onPress={handleSubmit(onSubmit)} disabled={isLoading}>
              {isLoading ? <Spinner /> : <ButtonText>Tiếp tục</ButtonText>}
            </Button>
            {Platform.OS === "android" && (
              <Button
                className="mt-4"
                variant="link"
                size="lg"
                onPress={() => {
                  router.dismissTo("/");
                }}
              >
                <ButtonText>Đăng nhập sau</ButtonText>
              </Button>
            )}
          </VStack>
        </TouchableWithoutFeedback>
      </ScrollView>
    </KeyboardAvoidingView>
  );
}
