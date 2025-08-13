import { HStack } from "@/components/ui/hstack";
import { Icon } from "@/components/ui/icon";
import { Text } from "@/components/ui/text";
import { useAuthStore } from "@/stores/authStore";
import { router } from "expo-router";
import { CircleUser, EllipsisIcon } from "lucide-react-native";
import React from "react";
import { TouchableOpacity, TouchableWithoutFeedback } from "react-native";

export const HomeHeader = () => {
  const userProfile = useAuthStore((state) => state.userProfile);

  return (
    <HStack className="px-4 w-full justify-between items-center position">
      <HStack className="items-center">
        <Text className="text-primary-400" size="lg" bold>
          {userProfile?.email ?? "MoneyTrack xin chào"}
        </Text>
      </HStack>
      {!userProfile?.id ? (
        <TouchableOpacity onPress={() => router.push("/authentication")}>
          <Icon size="xl" className="text-typography-600" as={CircleUser} />
        </TouchableOpacity>
      ) : (
        <TouchableOpacity onPress={() => router.push("/setting")}>
          <Icon size="xl" className="text-typography-800" as={EllipsisIcon} />
        </TouchableOpacity>
      )}
    </HStack>
  );
};

export default HomeHeader;
