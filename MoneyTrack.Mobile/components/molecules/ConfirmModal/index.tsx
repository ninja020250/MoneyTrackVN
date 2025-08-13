import { Button, ButtonText } from "@/components/ui/button";
import { Heading } from "@/components/ui/heading";
import { CloseIcon, Icon } from "@/components/ui/icon";
import {
  IModalProps,
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

export type ConfirmModalProps = {
  onOk: () => void;
  onCancel: () => void;
  title: string;
  description: string;
} & IModalProps;

export const ConfirmModal = ({ isOpen, onClose, onOk, onCancel, title, description }: ConfirmModalProps) => {
  return (
    <Modal isOpen={isOpen} onClose={onClose} size="md">
      <ModalBackdrop />
      <ModalContent>
        <ModalHeader>
          <Heading size="md" className="text-typography-950">
            {title}
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
            {description}
          </Text>
        </ModalBody>
        <ModalFooter>
          <Button variant="outline" action="secondary" onPress={onClose}>
            <ButtonText>Hủy</ButtonText>
          </Button>
          <Button onPress={onOk}>
            <ButtonText>Xác nhận</ButtonText>
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default ConfirmModal;
