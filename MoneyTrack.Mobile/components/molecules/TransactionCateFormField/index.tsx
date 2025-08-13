import { transactionCategoryIcons } from "@/components/Atom/TransactionIcon";
import { Badge, BadgeIcon, BadgeText } from "@/components/ui/badge";
import {
  FormControl,
  FormControlErrorText,
  FormControlLabel,
  FormControlLabelText,
} from "@/components/ui/form-control";
import { HStack } from "@/components/ui/hstack";
import { IInputProps, InputField } from "@/components/ui/input";
import { ITransactionCategory } from "@/models/transaction.model";
import { getTextByCategoryCode } from "@/utils/transaction.utils";
import React, { forwardRef } from "react";
import { Control, Controller } from "react-hook-form";
import { TouchableOpacity, TouchableWithoutFeedback } from "react-native";

type TransactionCateFormFieldProps = {
  control: Control<any>;
  name: string;
  label: string;
  hidden?: boolean;
  options?: ITransactionCategory["code"][];
} & IInputProps;

const defaultOptions = [
  "SHOPPING_001",
  "ENTERTAINMENT_001",
  "TRAVEL_001",
  "FOOD_001",
  "FIXED_001",
  "EDUCATION_001",
  "HEALTH_CARE_001",
  "INVESTMENT_001",
  "LIVING_001",
  "UNFORESEEN_001",
];

export const TransactionCateFormField = forwardRef<React.ElementRef<typeof InputField>, TransactionCateFormFieldProps>(
  ({ control, name, label, options = defaultOptions, hidden }, ref) => {
    return (
      <Controller
        control={control}
        name={name}
        render={({ field: { onChange, value }, fieldState }) => (
          <FormControl className={`w-full ${hidden && "hidden"}`}>
            <FormControlLabel>
              <FormControlLabelText>{label}</FormControlLabelText>
            </FormControlLabel>
            <HStack className="flex-wrap gap-4">
              {options.map((code) => (
                <TouchableOpacity key={code} onPress={() => onChange(code)}>
                  <Badge size="lg" variant="outline" action={value === code ? "info" : "muted"}>
                    {transactionCategoryIcons[code] && (
                      <BadgeIcon as={transactionCategoryIcons[code]} className="mr-2" />
                    )}
                    <BadgeText>{getTextByCategoryCode(code)}</BadgeText>
                  </Badge>
                </TouchableOpacity>
              ))}
            </HStack>
            {fieldState.error?.message && <FormControlErrorText>{fieldState.error?.message}</FormControlErrorText>}
          </FormControl>
        )}
      />
    );
  }
);

export default TransactionCateFormField;
