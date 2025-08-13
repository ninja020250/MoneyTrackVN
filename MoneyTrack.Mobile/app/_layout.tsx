import { GluestackUIProvider } from "@/components/ui/gluestack-ui-provider";
import { useColorScheme } from "@/components/useColorScheme";
import FontAwesome from "@expo/vector-icons/FontAwesome";
import { DarkTheme, DefaultTheme, ThemeProvider } from "@react-navigation/native";
import { useFonts } from "expo-font";
import { router, Stack } from "expo-router";
import * as SplashScreen from "expo-splash-screen";
import { useEffect, useState } from "react";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ChevronLeftIcon } from "lucide-react-native";
import { Dimensions, TouchableOpacity } from "react-native";
import "../global.css";

export {
  // Catch any errors thrown by the Layout component.
  ErrorBoundary,
} from "expo-router";

const queryClient = new QueryClient();
const screenHeight = Dimensions.get("window").height;

// export const unstable_settings = {
//   // Ensure that reloading on `/modal` keeps a back button present.
//   initialRouteName: "gluestack",
// };

// Prevent the splash screen from auto-hiding before asset loading is complete.
SplashScreen.preventAutoHideAsync();

export default function RootLayout() {
  const [loaded, error] = useFonts({
    SpaceMono: require("../assets/fonts/SpaceMono-Regular.ttf"),
    ...FontAwesome.font,
  });

  const [styleLoaded, setStyleLoaded] = useState(false);
  // Expo Router uses Error Boundaries to catch errors in the navigation tree.
  useEffect(() => {
    if (error) throw error;
  }, [error]);

  useEffect(() => {
    if (loaded) {
      SplashScreen.hideAsync();
    }
  }, [loaded]);

  return <RootLayoutNav />;
}

function RootLayoutNav() {
  const colorScheme = useColorScheme();

  return (
    <QueryClientProvider client={queryClient}>
      <GluestackUIProvider mode={colorScheme === "dark" ? "dark" : "light"}>
        <ThemeProvider value={colorScheme === "dark" ? DarkTheme : DefaultTheme}>
          <Stack>
            <Stack.Screen name="index" options={{ headerShown: false }} />
            <Stack.Screen
              name="authentication"
              options={{
                presentation: "modal", // Makes it appear as a modal
                animation: "slide_from_bottom", // Slide-up effect
                headerShown: false, // Hide header for a clean modal look
              }}
            />
            <Stack.Screen
              name="setting"
              options={{
                presentation: "containedTransparentModal", // Makes it appear as a modal
                animation: "slide_from_left", // Slide-up effect
                headerShown: false, // Hide header for a clean modal look
              }}
            />
            <Stack.Screen
              name="detail"
              options={{
                presentation: "containedTransparentModal", // Makes it appear as a modal
                animation: "slide_from_left", // Slide-up effect
                headerShown: false, // Hide header for a clean modal look
              }}
            />
            <Stack.Screen
              name="categoryDetail"
              options={{
                presentation: "containedTransparentModal", // Makes it appear as a modal
                animation: "slide_from_left", // Slide-up effect
                headerShown: false, // Hide header for a clean modal look
              }}
            />
            <Stack.Screen
              name="termAndCondition"
              options={{
                presentation: "fullScreenModal", // Makes it appear as a modal
                animation: "slide_from_left", // Slide-up effect
                title: "",
                headerLeft: () => {
                  return (
                    <TouchableOpacity onPress={() => router.dismissTo("/authentication")}>
                      <ChevronLeftIcon size={24} color="black" />
                    </TouchableOpacity>
                  );
                },
              }}
            />
          </Stack>
        </ThemeProvider>
      </GluestackUIProvider>
    </QueryClientProvider>
  );
}
