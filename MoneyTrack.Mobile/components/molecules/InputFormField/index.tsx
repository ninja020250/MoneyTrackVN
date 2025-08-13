import {
  FormControl,
  FormControlErrorText,
  FormControlLabel,
  FormControlLabelText,
} from "@/components/ui/form-control";
import { IInputProps, Input, InputField, InputIcon, InputSlot } from "@/components/ui/input";
import { IInputFieldProps } from "@gluestack-ui/input/lib/types";
import { XIcon } from "lucide-react-native";
import React, { forwardRef } from "react";
import { Control, Controller } from "react-hook-form";

type InputFormFieldProps = {
  control: Control<any>;
  name: string;
  placeholder?: string;
  label: string;
  secureTextEntry?: boolean;
  inputFieldProps?: IInputFieldProps;
  children?: React.ReactNode;
  clearable?: boolean;
  hidden?: boolean;
} & IInputProps;

export const InputFormField = forwardRef<React.ElementRef<typeof InputField>, InputFormFieldProps>(
  (
    { control, name, placeholder, secureTextEntry, label, inputFieldProps, children, clearable, hidden, ...rest },
    ref
  ) => {
    return (
      <Controller
        control={control}
        name={name}
        render={({ field: { onChange, onBlur, value }, fieldState }) => (
          <FormControl className={`w-full ${hidden && "hidden"}`}>
            <FormControlLabel>
              <FormControlLabelText>{label}</FormControlLabelText>
            </FormControlLabel>
            <Input variant="outline" size="xl" {...rest} isInvalid={fieldState.invalid}>
              <InputField
                ref={ref}
                placeholder={placeholder}
                secureTextEntry={secureTextEntry}
                onBlur={onBlur}
                onChangeText={onChange}
                value={value}
                {...inputFieldProps}
              />
              {clearable && value && (
                <InputSlot onPress={(e) => onChange("")}>
                  <InputIcon as={XIcon} className="right-2"></InputIcon>
                </InputSlot>
              )}
              {children}
            </Input>
            {fieldState.error?.message && <FormControlErrorText>{fieldState.error?.message}</FormControlErrorText>}
          </FormControl>
        )}
      />
    );
  }
);

export default InputFormField;
