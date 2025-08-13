import { Box } from "@/components/ui/box";
import { HStack } from "@/components/ui/hstack";
import { Text } from "@/components/ui/text";
import { ITransaction, ITransactionCategory } from "@/models/transaction.model";
import { formatNumberWithCommas } from "@/utils/number.utils";
import { getMainColorByCategoryCode, getTextByCategoryCode } from "@/utils/transaction.utils";
import React, { useState } from "react";
import { ScrollView, TouchableOpacity } from "react-native";

export const CategoryLegendGroup = ({
  data,
}: {
  data: { categoryCode: ITransactionCategory["code"]; data: ITransaction[]; total: number; value: number }[];
}) => {
  const [displayTotalAmount, setDisplayTotalAmount] = useState(false);
  return (
    <ScrollView className="w-full pl-6 max-h-[120]">
      {data.map((group) => {
        const proportion = (group.value / group.total) * 100;
        return (
          <HStack key={group.categoryCode} className="w-full items-center justify-between gap-4 mb-2">
            <HStack className="items-center gap-2">
              <Box
                className="rounded-xl w-4 h-2"
                style={{ backgroundColor: getMainColorByCategoryCode(group.categoryCode) }}
              />
              <Text>{getTextByCategoryCode(group.categoryCode)}</Text>
            </HStack>
            <TouchableOpacity onPress={() => setDisplayTotalAmount(!displayTotalAmount)}>
              {!displayTotalAmount ? (
                <Text className="font-bold ml-auto" style={{ color: getMainColorByCategoryCode(group.categoryCode) }}>
                  {proportion < 1 ? proportion.toFixed(1) : proportion.toFixed(0)}%
                </Text>
              ) : (
                <Text className="font-bold ml-auto" style={{ color: getMainColorByCategoryCode(group.categoryCode) }}>
                  {formatNumberWithCommas(group.value)}đ
                </Text>
              )}
            </TouchableOpacity>
          </HStack>
        );
      })}
    </ScrollView>
  );
};

export default CategoryLegendGroup;
