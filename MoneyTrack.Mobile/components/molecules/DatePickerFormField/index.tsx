import { Button, ButtonText } from "@/components/ui/button";
import {
  FormControl,
  FormControlErrorText,
  FormControlLabel,
  FormControlLabelText,
} from "@/components/ui/form-control";
import { IInputProps } from "@/components/ui/input";
import DateTimePicker from "@react-native-community/datetimepicker";
import dayjs from "dayjs";
import React from "react";
import { Control, Controller, useFormContext } from "react-hook-form";

type DatePickerFormFieldProps = {
  control: Control<any>;
  name: string;
  label: string;
  hidden?: boolean;
} & IInputProps;

export const DatePickerFormField = ({ control, name, label, hidden }: DatePickerFormFieldProps) => {
  const { setValue } = useFormContext();
  const [showPicker, setShowPicker] = React.useState(false);

  return (
    <Controller
      control={control}
      name={name}
      render={({ field: { value }, fieldState }) => (
        <FormControl className={`w-full ${hidden && "hidden"}`}>
          <FormControlLabel>
            <FormControlLabelText>{label}</FormControlLabelText>
          </FormControlLabel>
          <Button variant="outline" size="xl" className="w-full" onPress={() => setShowPicker(!showPicker)}>
            <ButtonText>{value ? dayjs(value).format("DD/MM/YYYY") : dayjs().format("DD/MM/YYYY")}</ButtonText>
          </Button>
          {showPicker && (
            <DateTimePicker
              negativeButton={{ label: "Hủy" }}
              maximumDate={new Date()}
              minimumDate={dayjs().subtract(1, "year").toDate()}
              style={{ marginLeft: -15 }}
              value={value ? new Date(value) : new Date()}
              mode="date"
              display="spinner"
              locale="vi-VN"
              onChange={(event, selectedDate) => {
                if (event.type === "dismissed") {
                  setShowPicker(false);
                } else if (event.type === "set" || (event.type === "neutralButtonPressed" && selectedDate)) {
                  setShowPicker(false);
                  setValue(name, selectedDate);
                }
              }}
            />
          )}
          {fieldState.error?.message && <FormControlErrorText>{fieldState.error?.message}</FormControlErrorText>}
        </FormControl>
      )}
    />
  );
};

export default DatePickerFormField;
