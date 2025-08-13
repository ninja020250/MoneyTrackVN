import React, { useState } from "react";
import { Controller, Control } from "react-hook-form";
import { Input, InputField, InputIcon, InputSlot } from "@/components/ui/input";
import { TouchableOpacity } from "react-native";
import { EyeIcon, EyeOffIcon } from "@/components/ui/icon";
import {
  FormControl,
  FormControlLabel,
  FormControlErrorText,
  FormControlLabelText,
} from "@/components/ui/form-control";

interface PasswordFormFieldProps {
  control: Control<any>;
  name: string;
  placeholder?: string;
  label: string;
}

export const PasswordFormField: React.FC<PasswordFormFieldProps> = ({ control, name, placeholder, label }) => {
  const [secureTextEntry, setSecureTextEntry] = useState(true);

  return (
    <Controller
      control={control}
      name={name}
      render={({ field: { onChange, onBlur, value }, fieldState }) => (
        <FormControl className="w-full">
          <FormControlLabel>
            <FormControlLabelText>{label}</FormControlLabelText>
          </FormControlLabel>
          <Input variant="outline" size="md" isInvalid={fieldState.invalid}>
            <InputField
              placeholder={placeholder}
              secureTextEntry={secureTextEntry}
              onBlur={onBlur}
              onChangeText={onChange}
              value={value}
              className="flex-1"
            />
            <InputSlot onPress={() => setSecureTextEntry(!secureTextEntry)}>
              <InputIcon>{secureTextEntry ? <EyeIcon /> : <EyeOffIcon />}</InputIcon>
            </InputSlot>
          </Input>
          {fieldState.error?.message && <FormControlErrorText>{fieldState.error?.message}</FormControlErrorText>}
        </FormControl>
      )}
    />
  );
};

export default PasswordFormField;
