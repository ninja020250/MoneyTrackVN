import TransactionIcon from "@/components/Atom/TransactionIcon";
import { HStack } from "@/components/ui/hstack";
import { Icon } from "@/components/ui/icon";
import { Text } from "@/components/ui/text";
import { ITransactionCategory } from "@/models/transaction.model";
import { formatNumberWithCommas } from "@/utils/number.utils";
import { ChevronRight } from "lucide-react-native";
import React from "react";
import { TouchableOpacity, TouchableWithoutFeedback } from "react-native";

export type TransactionCategoryCardProps = {
  amount: number;
  onPress: () => void;
} & Pick<ITransactionCategory, "code" | "name">;

export const TransactionCategoryCard = ({ name, code, amount, onPress }: TransactionCategoryCardProps) => {
  return (
    <TouchableOpacity onPress={onPress}>
      <HStack className="justify-start items-center w-full p-4 gap-2">
        <TransactionIcon categoryCode={code} />
        <Text className="">{name}</Text>
        <HStack className="ml-auto items-center gap-1">
          <Text className="neutral-600 font-bold">{`${formatNumberWithCommas(amount)}đ`}</Text>
          <Icon as={ChevronRight} className="neutral-600" size="sm" />
        </HStack>
      </HStack>
    </TouchableOpacity>
  );
};

export default TransactionCategoryCard;
