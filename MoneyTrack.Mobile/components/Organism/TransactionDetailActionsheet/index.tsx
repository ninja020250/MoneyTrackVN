import DatePickerFormField from "@/components/molecules/DatePickerFormField";
import InputFormField from "@/components/molecules/InputFormField";
import MoneyInputFormField from "@/components/molecules/MoneyInputFormField";
import TransactionCateFormField from "@/components/molecules/TransactionCateFormField";
import {
  Actionsheet,
  ActionsheetBackdrop,
  ActionsheetContent,
  ActionsheetDragIndicator,
  ActionsheetDragIndicatorWrapper,
} from "@/components/ui/actionsheet";
import { Button, ButtonText } from "@/components/ui/button";
import { VStack } from "@/components/ui/vstack";
import { yupResolver } from "@hookform/resolvers/yup";
import { useEffect } from "react";
import { FormProvider, useForm } from "react-hook-form";
import { KeyboardAvoidingView, Platform, useWindowDimensions } from "react-native";
import * as yup from "yup";

const schema = yup.object().shape({
  description: yup.string().required("Nhập nội dung nha bạn ơi"),
  amount: yup.number().required("Nhập số tiền nha bạn ơi.").positive("Amount must be positive"),
  categoryCode: yup.string().required("Chọn danh 1 mục nha bạn ơi"),
});

export const TransactionDetailActionsheet = ({
  isOpen = true,
  isSubmitting = false,
  initValues = {},
  handleClose = () => {},
  onSubmit,
}: {
  isOpen: boolean;
  isSubmitting: boolean;
  initValues: any;
  onSubmit: (values: any) => void;
  handleClose: () => void;
}) => {
  const form = useForm<any>({
    resolver: yupResolver(schema),
  });
  const { width } = useWindowDimensions();

  useEffect(() => {
    if (isOpen && initValues) {
      form.reset(initValues);
    } else {
      form.reset({
        expenseDate: new Date(),
      });
    }
  }, [isOpen, initValues]);

  return (
    <Actionsheet isOpen={isOpen} onClose={handleClose}>
      <KeyboardAvoidingView behavior={"padding"}>
        <ActionsheetBackdrop onPress={handleClose} />
        <ActionsheetContent>
          <ActionsheetDragIndicatorWrapper>
            <ActionsheetDragIndicator />
          </ActionsheetDragIndicatorWrapper>
          <VStack className="gap-4 w-full p-2 pb-6">
            <FormProvider {...form}>
              <InputFormField
                clearable
                label="Nội dung"
                placeholder="nhập nội dung chi tiêu"
                control={form.control}
                name="description"
              />
              <MoneyInputFormField
                clearable
                label="Số tiền"
                placeholder="Nhập số tiền"
                control={form.control}
                name="amount"
              />
              <DatePickerFormField label="Ngày chi" control={form.control} name="expenseDate" />
              <TransactionCateFormField label="Danh mục" control={form.control} name="categoryCode" />
            </FormProvider>
          </VStack>
          <Button size="xl" className="w-full" onPress={form.handleSubmit(onSubmit)}>
            <ButtonText>{initValues ? "Lưu" : "Tạo mới"}</ButtonText>
          </Button>
        </ActionsheetContent>
      </KeyboardAvoidingView>
    </Actionsheet>
  );
};

export default TransactionDetailActionsheet;
