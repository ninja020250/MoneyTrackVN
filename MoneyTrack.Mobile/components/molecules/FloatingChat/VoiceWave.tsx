import React, { useEffect, useRef } from "react";
import { Animated, View } from "react-native";

const BAR_COUNT = 28;
const BAR_WIDTH = 4;
const BAR_SPACING = 4;
const MIN_BAR_HEIGHT = 16;
const MAX_BAR_HEIGHT = 40;

const VoiceWave = ({ isRecording }: { isRecording: boolean }) => {
  const animations = useRef(Array.from({ length: BAR_COUNT }, () => new Animated.Value(1))).current;
  const barHeights = useRef(
    Array.from({ length: BAR_COUNT }, () => Math.random() * (MAX_BAR_HEIGHT - MIN_BAR_HEIGHT) + MIN_BAR_HEIGHT)
  ).current;

  useEffect(() => {
    if (isRecording) {
      const animate = () => {
        const center = Math.floor(BAR_COUNT / 2);
        const animationsArr = animations.map((anim, i) => {
          const distanceFromCenter = Math.abs(i - center);
          barHeights[i] = Math.random() * (MAX_BAR_HEIGHT - MIN_BAR_HEIGHT) + MIN_BAR_HEIGHT;
          return Animated.sequence([
            Animated.timing(anim, {
              toValue: Math.random() * 1.5 + 0.5,
              duration: 200 + distanceFromCenter * 30,
              useNativeDriver: false,
            }),
            Animated.timing(anim, {
              toValue: 1,
              duration: 200 + distanceFromCenter * 30,
              useNativeDriver: false,
            }),
          ]);
        });
        Animated.stagger(60, animationsArr).start(() => {
          if (isRecording) animate();
        });
      };
      animate();
    } else {
      animations.forEach((anim) => anim.setValue(1));
    }
  }, [isRecording]);

  return (
    <View className="flex-row items-end h-10 mx-2">
      {animations.map((anim, i) => (
        <Animated.View
          key={i}
          className="rounded bg-primary-900"
          style={{
            width: BAR_WIDTH,
            marginHorizontal: BAR_SPACING / 2,
            height: anim.interpolate({
              inputRange: [0, 2],
              outputRange: [8, barHeights[i]],
            }),
          }}
        />
      ))}
    </View>
  );
};

export default VoiceWave;
