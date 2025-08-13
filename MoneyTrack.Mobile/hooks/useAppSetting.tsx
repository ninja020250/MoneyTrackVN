import appStorage, { ISetting } from "@/utils/asyncStorage.utils";
import { useEffect, useState } from "react";

export const useAppSetting = () => {
  const [setting, setSetting] = useState<ISetting>({});

  useEffect(() => {
    appStorage.getSetting().then((setting) => {
      setSetting(setting);
    });
  }, []);

  const toggleSyncToCloud = (value: boolean) => {
    appStorage.getSetting().then((setting) => {
      appStorage.setSetting({ ...setting, syncToCloud: value });
      setSetting({ ...setting, syncToCloud: value });
    });
  };

  return { setting, toggleSyncToCloud };
};
