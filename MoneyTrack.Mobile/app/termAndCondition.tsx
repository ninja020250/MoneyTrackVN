import { WebView } from "react-native-webview";
import Constants from "expo-constants";
import { StyleSheet } from "react-native";

export default function TermAndCondition() {
  return <WebView style={styles.container} source={{ uri: `${process.env.EXPO_PUBLIC_API_URL}/term-condition` }} />;
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
});
