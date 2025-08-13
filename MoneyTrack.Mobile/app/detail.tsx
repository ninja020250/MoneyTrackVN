import BaseContainer from "@/components/Atom/BaseContainer";
import CategoryLegendGroup from "@/components/molecules/CategoryLegendGroup";
import TransactionCategoryCard from "@/components/molecules/TransactionCategoryCard";
import TransactionPieChart from "@/components/molecules/TransactionPieChart";
import { View } from "@/components/Themed";
import { Box } from "@/components/ui/box";
import { Button, ButtonIcon, ButtonText } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Divider } from "@/components/ui/divider";
import { HStack } from "@/components/ui/hstack";
import { Text } from "@/components/ui/text";
import { VStack } from "@/components/ui/vstack";
import { useTransaction } from "@/hooks/useTransaction";
import { formatNumberWithCommas } from "@/utils/number.utils";
import { getTextByCategoryCode } from "@/utils/transaction.utils";
import dayjs from "dayjs";
import { router, useLocalSearchParams } from "expo-router";
import { ArrowLeftIcon, ChevronLeftIcon, ChevronRightIcon } from "lucide-react-native";
import React, { useMemo } from "react";
import { FlatList } from "react-native";

export const Detail = () => {
  const { selectedMonth: defaultSelectedMonth } = useLocalSearchParams();

  const { selectedMonth, setSelectedMonth, transactionsBySelectedMonth, groupTransactionsByCategoryCode, totalAmount } =
    useTransaction({ defaultSelectedMonth });

  const onBack = () => {
    router.dismissTo("/");
  };

  const groupedByCategoryCode = useMemo(
    () => groupTransactionsByCategoryCode(transactionsBySelectedMonth),
    [transactionsBySelectedMonth]
  );

  return (
    <BaseContainer>
      <HStack className="justify-between items-center w-full gap-1 px-4">
        <Button variant="link" onPress={onBack} className="p-0">
          <ButtonIcon as={ArrowLeftIcon} className="h-6 w-6 text-background-900" />
        </Button>
        <Text className="text-lg text-typography-800 mb-2 mt-4" bold>
          Chi tiết danh mục
        </Text>
        <View className="w-2" />
      </HStack>
      <VStack className="w-full p-4">
        <Card variant="outline" className="w-full rounded-xl bg-white">
          <HStack className="justify-between mb-2">
            <VStack>
              <Text className="font-bold" size="lg">
                Quản lý chi tiêu
              </Text>
              <HStack className="gap-4">
                <Button
                  variant="link"
                  onPress={() => {
                    const prevMonth = dayjs(selectedMonth).subtract(1, "month").toISOString();
                    setSelectedMonth(prevMonth);
                  }}
                >
                  <ButtonIcon as={ChevronLeftIcon} className="h-5 w-5 text-background-900 ml-1" />
                </Button>
                <Button variant="link">
                  <ButtonText size="md">Tháng {dayjs(selectedMonth).format("M")}</ButtonText>
                </Button>
                <Button
                  variant="link"
                  onPress={() => {
                    const prevMonth = dayjs(selectedMonth).add(1, "month").toISOString();
                    setSelectedMonth(prevMonth);
                  }}
                >
                  <ButtonIcon as={ChevronRightIcon} className="h-5 w-5 text-background-900 ml-1" />
                </Button>
              </HStack>
            </VStack>
            <HStack className="gap-1">
              <Text className="font-bold mt-1.5" size="lg">
                {formatNumberWithCommas(totalAmount)}đ
              </Text>
            </HStack>
          </HStack>
          {totalAmount > 0 && (
            <HStack>
              <HStack className="p-3 bg-neutral-200 rounded-xl relative items-center justify-center w-[calc(100%/2.6)]">
                <TransactionPieChart transactions={transactionsBySelectedMonth} />
              </HStack>
              <CategoryLegendGroup data={groupedByCategoryCode} />
            </HStack>
          )}
        </Card>
      </VStack>
      <FlatList
        keyboardShouldPersistTaps="handled"
        className="flex-1 mt-2"
        scrollEnabled
        data={groupedByCategoryCode}
        ListFooterComponent={<Box className="h-40" />}
        renderItem={({ item, index }) => (
          <VStack>
            <TransactionCategoryCard
              key={item.categoryCode}
              amount={item.value}
              code={item.categoryCode}
              name={getTextByCategoryCode(item.categoryCode)}
              onPress={() =>
                router.push({
                  pathname: "/categoryDetail",
                  params: { categoryCode: item.categoryCode, selectedMonth },
                })
              }
            />
            {index < groupedByCategoryCode.length - 1 && (
              <Box className="w-full px-4">
                <Divider />
              </Box>
            )}
          </VStack>
        )}
        keyExtractor={(item) => item.categoryCode}
      />
    </BaseContainer>
  );
};

export default Detail;
