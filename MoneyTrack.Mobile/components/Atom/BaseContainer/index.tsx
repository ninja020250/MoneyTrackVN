import { Platform, View } from "react-native";
import React from "react";
import { useSafeAreaInsets } from "react-native-safe-area-context";

export type BaseContainerProps = {
  children: React.ReactNode;
  className?: string;
};

export const BaseContainer = ({ children, className }: BaseContainerProps) => {
  const { top } = useSafeAreaInsets();
  return (
    <View
      className={`flex-1 w-100 justify-start items-center relative bg-neutral-50 pb-3 ${className}`}
      style={{ paddingTop: Platform.OS === "ios" ? top : 40 }}
    >
      {children}
    </View>
  );
};

export default BaseContainer;
