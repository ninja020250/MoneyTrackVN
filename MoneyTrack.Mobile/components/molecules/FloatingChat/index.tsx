import { HStack } from "@/components/ui/hstack";
import { Icon } from "@/components/ui/icon";
import { Input, InputField } from "@/components/ui/input";
import { Spinner } from "@/components/ui/spinner";
import { VStack } from "@/components/ui/vstack";
import { ArrowUpIcon, Plus, SparklesIcon } from "lucide-react-native";
import { useEffect, useRef, useState } from "react";
import {
  Animated,
  Keyboard,
  KeyboardAvoidingView,
  Platform,
  TouchableOpacity,
  TouchableWithoutFeedback,
  View,
} from "react-native";

export type FloatingChatProps = {
  isLoading?: boolean;
  onMessage?: (message: string) => void;
  onPressCreate: () => void;
  placeholder?: string;
};

export default function FloatingChat({
  isLoading = true,
  onMessage,
  onPressCreate,
  placeholder = "VD: Đi bách hóa xanh 100k",
}: FloatingChatProps) {
  const [keyboardOffset, setKeyboardOffset] = useState(0);
  const inputRef = useRef(null);
  const [message, setMessage] = useState("");
  const fadeAnim = useRef(new Animated.Value(0)).current;

  useEffect(() => {
    const keyboardDidShowListener = Keyboard.addListener("keyboardDidShow", (event) => {
      setKeyboardOffset(event.endCoordinates.height);
      fadeIn();
    });
    const keyboardDidHideListener = Keyboard.addListener("keyboardDidHide", () => {
      setKeyboardOffset(0);
      fadeOut();
    });

    return () => {
      keyboardDidShowListener.remove();
      keyboardDidHideListener.remove();
    };
  }, []);

  const fadeIn = () => {
    Animated.timing(fadeAnim, {
      toValue: 1,
      duration: 500,
      useNativeDriver: true,
    }).start();
  };

  const fadeOut = () => {
    Animated.timing(fadeAnim, {
      toValue: 0,
      duration: 500,
      useNativeDriver: true,
    }).start();
  };

  const handleSubmitMessage = () => {
    if (message) {
      onMessage(message);
      setMessage("");
      Keyboard.dismiss();
    } else {
      inputRef.current?.focus();
    }
  };

  const getBehavior = () => {
    if (Platform.OS === "ios") {
      return "padding";
    } else {
      return keyboardOffset > 0 ? "padding" : "height";
    }
  };

  return (
    <TouchableWithoutFeedback className="flex-1 relative" onPress={Keyboard.dismiss}>
      <KeyboardAvoidingView style={{ bottom: 0, flex: 1 }} className="absolute w-full" behavior={getBehavior()}>
        <View
          style={{
            shadowOffset: {
              width: 0,
              height: 4,
            },
            shadowOpacity: 0.45,
            shadowRadius: 8,
            shadowColor: "#000",
            elevation: 1,
          }}
          className="bg-neutral-200 px-4 pb-10 pt-2 rounded-t-3xl flex-row items-center shadow-black shadow-opacity-20 shadow-radius-4"
        >
          <VStack>
            <Input className="pl-1 border-0 bg-transparent">
              <InputField
                ref={inputRef}
                value={message}
                onChangeText={(txt) => setMessage(txt)}
                className="text-typography-950 placeholder-neutral-500 text-lg font-semibold"
                placeholder={placeholder}
              />
            </Input>
            <HStack className="w-full mt-1 gap-2 px-2">
              <TouchableOpacity
                onPress={onPressCreate}
                className="w-10 h-10 rounded-full bg-white items-center justify-center border border-neutral-400"
              >
                <Plus size={24} color="black" />
              </TouchableOpacity>

              <TouchableOpacity
                onPress={handleSubmitMessage}
                className="ml-auto w-10 h-10 rounded-full bg-primary-900 items-center justify-center border border-neutral-900"
              >
                {!isLoading && (
                  <View>
                    {keyboardOffset <= 0 && <Icon as={SparklesIcon} size="xl" className="text-purple-50" />}
                    <Animated.View style={{ opacity: fadeAnim }}>
                      {keyboardOffset > 0 && <Icon as={ArrowUpIcon} size="xl" className="text-purple-50" />}
                    </Animated.View>
                  </View>
                )}
                {isLoading && <Spinner size="small" className="text-purple-50" />}
              </TouchableOpacity>
            </HStack>
          </VStack>
        </View>
      </KeyboardAvoidingView>
    </TouchableWithoutFeedback>
  );
}
