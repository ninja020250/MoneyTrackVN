import React, { useMemo } from "react";
import Rocket from "./Rocket";
import Logo from "./Logo";
import { View } from "@/components/Themed";

export const Illustration = ({
  name,
  scale = 1,
  width,
  height,
}: {
  name: "rocket" | "logo";
  scale: number;
  width?: any;
  height?: any;
}) => {
  const illustration = useMemo(() => {
    switch (name) {
      case "rocket":
        return <Rocket />;
      case "logo":
        return <Logo />;
      default:
        return <Rocket />;
    }
  }, [name]);

  return (
    <View style={{ backgroundColor: "transparent", transform: `scale(${scale})`, width, height }}>{illustration}</View>
  );
};

export default Illustration;
