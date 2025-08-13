import { router } from "expo-router";
import {
  ArrowLeftIcon,
  CloudDownloadIcon,
  CloudUploadIcon,
  CloudyIcon,
  LogOutIcon,
  UserRoundXIcon,
} from "lucide-react-native";
import React, { useEffect, useState } from "react";
import { TouchableOpacity } from "react-native";

import BaseContainer from "@/components/Atom/BaseContainer";
import Illustration from "@/components/Atom/Illustration";
import ModalConfirmDeleteAccount from "@/components/molecules/ModalConfirmDeleteAccount/ModalConfirmDeleteAccount";
import ModalConfirmRestoreData from "@/components/molecules/ModalConfirmRestoreData/ModalConfirmRestoreData";
import { View } from "@/components/Themed";
import { Box } from "@/components/ui/box";
import { Button, ButtonIcon } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Divider } from "@/components/ui/divider";
import { HStack } from "@/components/ui/hstack";
import { Icon } from "@/components/ui/icon";
import { Spinner } from "@/components/ui/spinner";
import { Switch } from "@/components/ui/switch";
import { Text } from "@/components/ui/text";
import { Toast, ToastDescription, useToast } from "@/components/ui/toast";
import { VStack } from "@/components/ui/vstack";
import { useAppSetting } from "@/hooks/useAppSetting";
import useAuthentication from "@/hooks/useAuthentication";
import { useTransaction } from "@/hooks/useTransaction";
import { useAuthStore } from "@/stores/authStore";
import queueTransactionStorage from "@/utils/queueTransaction.utils";
import NetInfo from "@react-native-community/netinfo";
import uuid from "react-native-uuid";

const SettingButton = ({
  icon,
  label,
  action,
  color,
  disabled,
}: {
  icon?: any;
  label?: any;
  action?: any;
  color?: any;
  disabled?: boolean;
}) => {
  return (
    <HStack className="p-4 gap-4 border-b border-neutral-100 items-center">
      <Icon size="md" as={icon} className={`${disabled ? "color-neutral-400" : color}`} />
      <Text className={`${disabled ? "color-neutral-400" : color}`}>{label}</Text>
      <Box className="ml-auto">{action}</Box>
    </HStack>
  );
};

export const Setting = () => {
  const toast = useToast();
  const userProfile = useAuthStore((state) => state.userProfile);
  const { logout, deleteMyAccount } = useAuthentication();
  const { syncToCloud, isSyncing, syncSuccess, pullAllTransactionsMutation, isPullingTransactions } = useTransaction();
  const [displayModalConfirm, setDisplayModalConfirm] = useState(false);
  const [displayModalDeleteAccount, setDisplayModalDeleteAccount] = useState(false);
  const { setting, toggleSyncToCloud } = useAppSetting();

  const [numOfSync, setNumOfSync] = useState(0);

  const restoreData = async () => {
    const netInfo = await NetInfo.fetch();
    if (!!userProfile.id && netInfo.isConnected) {
      setDisplayModalConfirm(false);
      pullAllTransactionsMutation();
    }
  };

  const deleteAccount = async () => {
    const netInfo = await NetInfo.fetch();
    if (!!userProfile.id && netInfo.isConnected) {
      setDisplayModalDeleteAccount(false);
      await deleteMyAccount();
      await logout();
      toast.show({
        id: uuid.v4(),
        placement: "top",
        duration: 20000,
        render: ({ id }) => {
          return (
            <Toast nativeID={id} action="success" variant="solid">
              <ToastDescription>Xóa thành công tài khoản. Hic, hẹn gặp lại bạn T^T</ToastDescription>
            </Toast>
          );
        },
      });
      router.dismissTo("/");
    }
  };

  const onBack = () => {
    router.dismissTo("/");
  };

  useEffect(() => {
    queueTransactionStorage.getQueueTransactions().then((trans) => {
      setNumOfSync(trans.length ?? 0);
    });
  }, [isSyncing, syncSuccess]);

  return (
    <BaseContainer className="flex-1 bg-neutral-100 pt-4">
      <HStack className="justify-between items-center w-full gap-1 px-4">
        <Button variant="link" onPress={onBack} className="p-0">
          <ButtonIcon as={ArrowLeftIcon} className="h-6 w-6 text-background-900" />
        </Button>
        <Text className="text-lg text-typography-800 mb-2 mt-4" bold>
          Cài đặt
        </Text>
        <View className="w-2" />
      </HStack>
      <VStack className="px-4 w-full items-start">
        <Text className="text-base text-typography-400" bold>
          {userProfile?.email}
        </Text>
        <Card className="p-0 w-full bg-white mt-4 rounded-xl">
          <SettingButton
            icon={CloudyIcon}
            label="Đồng bộ cloud, giúp khôi phục dữ liệu"
            action={<Switch size="sm" value={setting.syncToCloud} onValueChange={toggleSyncToCloud} />}
          />
          {setting.syncToCloud && (
            <TouchableOpacity onPress={() => syncToCloud()} disabled={!numOfSync || isSyncing}>
              <SettingButton
                disabled={!numOfSync || isSyncing}
                icon={CloudUploadIcon}
                label={numOfSync > 0 ? `Đồng bộ dữ liệu lên đám mây ngay` : "Dữ liệu đã được cập nhật"}
                action={isSyncing && <Spinner />}
              />
            </TouchableOpacity>
          )}
          <TouchableOpacity onPress={() => setDisplayModalConfirm(true)} disabled={isPullingTransactions}>
            <SettingButton
              disabled={isPullingTransactions}
              icon={CloudDownloadIcon}
              label={"Khôi phục dữ liệu từ đám mây"}
              action={isPullingTransactions && <Spinner />}
            />
          </TouchableOpacity>
          <TouchableOpacity
            onPress={async () => {
              await logout();
              router.dismissTo("/");
            }}
          >
            <SettingButton icon={LogOutIcon} label="Đăng xuất" />
          </TouchableOpacity>
        </Card>
      </VStack>
      <HStack className="px-4 w-full justify-center">
        <Divider className="my-4" />
      </HStack>
      <VStack className="p-4 flex-1 w-full ">
        <Text size="md" className="font-bold">
          Nhiều tính năng mới đang trong thời gian phát triển, mọi người ủng hộ app nhé.
        </Text>
        <Illustration name="rocket" scale={0.8} />
      </VStack>
      <HStack>
        <TouchableOpacity onPress={() => setDisplayModalDeleteAccount(true)}>
          <SettingButton icon={UserRoundXIcon} label="Xóa tài khoản" />
        </TouchableOpacity>
      </HStack>
      <ModalConfirmRestoreData
        showModal={displayModalConfirm}
        onClose={() => setDisplayModalConfirm(false)}
        onConfirm={restoreData}
      />
      <ModalConfirmDeleteAccount
        showModal={displayModalDeleteAccount}
        onClose={() => setDisplayModalDeleteAccount(false)}
        onConfirm={deleteAccount}
      />
    </BaseContainer>
  );
};

export default Setting;
