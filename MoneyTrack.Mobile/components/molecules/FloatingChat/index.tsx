import { HStack } from "@/components/ui/hstack";
import { Icon } from "@/components/ui/icon";
import { Input, InputField } from "@/components/ui/input";
import { Spinner } from "@/components/ui/spinner";
import { VStack } from "@/components/ui/vstack";
import { ExpoSpeechRecognitionModule, useSpeechRecognitionEvent } from "expo-speech-recognition";
import { ArrowUpIcon, CheckIcon, MicIcon, Plus, SparklesIcon, XIcon } from "lucide-react-native";
import { useEffect, useRef, useState } from "react";
import { Animated, Keyboard, KeyboardAvoidingView, Platform, TouchableWithoutFeedback, View } from "react-native";
import FloatingChatButton from "./FloatingChatButton";
import VoiceWave from "./VoiceWave";

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
  const [isRecording, setIsRecording] = useState(false);
  const [speechPermissionGranted, setSpeechPermissionGranted] = useState(false);

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

  // Speech recognition event handlers
  useSpeechRecognitionEvent("start", () => {
    setIsRecording(true);
  });

  useSpeechRecognitionEvent("end", () => {
    setIsRecording(false);
  });

  useSpeechRecognitionEvent("result", (event) => {
    const transcript = event.results[0]?.transcript;
    if (transcript) {
      setMessage(transcript);
      if (transcript.length > 50 && isRecording) {
        ExpoSpeechRecognitionModule.stop();
      }
    }
  });

  useSpeechRecognitionEvent("error", (event) => {
    console.log("Speech recognition error:", event.error, event.message);
    setIsRecording(false);
  });

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

  const handleSpeechRecognition = async () => {
    if (isRecording) {
      // Stop recording
      ExpoSpeechRecognitionModule.stop();
    } else {
      // Request permissions and start recording
      try {
        const result = await ExpoSpeechRecognitionModule.requestPermissionsAsync();
        if (!result.granted) {
          console.warn("Speech recognition permissions not granted", result);
          return;
        }
        setSpeechPermissionGranted(true);

        // Start speech recognition with Vietnamese language
        ExpoSpeechRecognitionModule.start({
          lang: "vi-VN", // Vietnamese language
          interimResults: true,
          continuous: false,
          maxAlternatives: 1,
        });
      } catch (error) {
        console.error("Error starting speech recognition:", error);
        setIsRecording(false);
      }
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
            {!isRecording && (
              <HStack className="w-full mt-1 gap-2 px-2 justify-between">
                <FloatingChatButton onPress={onPressCreate} color="white">
                  <Plus size={24} color="black" />
                </FloatingChatButton>
                <HStack className="gap-4">
                  <FloatingChatButton color="black" onPress={handleSpeechRecognition}>
                    {!isLoading && (
                      <View>{keyboardOffset <= 0 && <Icon as={MicIcon} size="xl" className="text-purple-50" />}</View>
                    )}
                    {isLoading && <Spinner size="small" className="text-purple-50" />}
                  </FloatingChatButton>
                  <FloatingChatButton color="black" onPress={handleSubmitMessage}>
                    {!isLoading && (
                      <View>
                        {keyboardOffset <= 0 && <Icon as={SparklesIcon} size="xl" className="text-purple-50" />}
                        <Animated.View style={{ opacity: fadeAnim }}>
                          {keyboardOffset > 0 && <Icon as={ArrowUpIcon} size="xl" className="text-purple-50" />}
                        </Animated.View>
                      </View>
                    )}
                    {isLoading && <Spinner size="small" className="text-purple-50" />}
                  </FloatingChatButton>
                </HStack>
              </HStack>
            )}
            {isRecording && (
              <HStack className="w-full mt-1 gap-2 px-2 justify-between">
                <FloatingChatButton
                  onPress={() => {
                    handleSpeechRecognition();
                    setTimeout(() => {
                      setMessage("");
                    }, 500);
                  }}
                  color="white"
                >
                  <XIcon size={24} color="black" />
                </FloatingChatButton>
                <VoiceWave isRecording={isRecording} />
                <FloatingChatButton onPress={handleSpeechRecognition} color="black">
                  <CheckIcon size={24} color="white" />
                </FloatingChatButton>
              </HStack>
            )}
          </VStack>
        </View>
      </KeyboardAvoidingView>
    </TouchableWithoutFeedback>
  );
}
