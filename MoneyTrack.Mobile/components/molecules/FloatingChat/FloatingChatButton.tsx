import React, { ReactNode } from "react";
import { TouchableOpacity } from "react-native";

export type FloatingChatButtonProps = {
  onPress: () => void;
  children: ReactNode;
  color?: "white" | "black";
};

const colors = {
  white: "bg-white border border-neutral-400",
  black: "bg-primary-900 border border-neutral-900",
};

export const FloatingChatButton = ({ onPress, children, color = "white" }) => {
  return (
    <TouchableOpacity
      onPress={onPress}
      className={`w-10 h-10 rounded-full  items-center justify-center ${colors[color]}`}
    >
      {children}
    </TouchableOpacity>
  );
};

export default FloatingChatButton;
