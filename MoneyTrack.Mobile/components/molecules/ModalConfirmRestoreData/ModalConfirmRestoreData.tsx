import { Button, ButtonText } from "@/components/ui/button";
import { Heading } from "@/components/ui/heading";
import { CloseIcon, Icon } from "@/components/ui/icon";
import {
  Modal,
  ModalBackdrop,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalFooter,
  ModalHeader,
} from "@/components/ui/modal";
import { Text } from "@/components/ui/text";
import React from "react";

const ModalConfirmRestoreData = ({ showModal, onClose, onConfirm }) => {
  return (
    <Modal isOpen={showModal} onClose={onClose} size="md">
      <ModalBackdrop />
      <ModalContent>
        <ModalHeader>
          <Heading size="md" className="text-typography-950">
            Khôi phục dữ liệu
          </Heading>
          <ModalCloseButton>
            <Icon
              as={CloseIcon}
              size="md"
              className="stroke-background-400 group-[:hover]/modal-close-button:stroke-background-700 group-[:active]/modal-close-button:stroke-background-900 group-[:focus-visible]/modal-close-button:stroke-background-900"
            />
          </ModalCloseButton>
        </ModalHeader>
        <ModalBody>
          <Text size="sm" className="text-typography-500">
            Sau khi khôi phục, dữ liệu sẽ được gộp chung với dữ liệu hiện có của bạn. Bạn có muốn tiếp tục không, Mọi dữ
            liệu hiện có trên app vẫn được giữ lại.
          </Text>
        </ModalBody>
        <ModalFooter>
          <Button variant="outline" action="secondary" onPress={onClose}>
            <ButtonText>Hủy</ButtonText>
          </Button>
          <Button onPress={onConfirm}>
            <ButtonText>Tiếp tục</ButtonText>
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default ModalConfirmRestoreData;
