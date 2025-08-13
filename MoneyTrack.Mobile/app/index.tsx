import CategoryLegendGroup from "@/components/molecules/CategoryLegendGroup";
import FloatingChat from "@/components/molecules/FloatingChat";
import HomeHeader from "@/components/molecules/HomeHeader";
import TransactionCard from "@/components/molecules/TransactionCard";
import TransactionCategoryPieChart from "@/components/molecules/TransactionPieChart";
import TransactionDetailActionsheet from "@/components/Organism/TransactionDetailActionsheet";
import { Box } from "@/components/ui/box";
import { Button, ButtonIcon, ButtonText } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Divider } from "@/components/ui/divider";
import { HStack } from "@/components/ui/hstack";
import { Text } from "@/components/ui/text";
import { VStack } from "@/components/ui/vstack";
import useAuthentication from "@/hooks/useAuthentication";
import { useTransaction } from "@/hooks/useTransaction";
import useTransactionAI from "@/hooks/useTransactionAI";
import { useAuthStore } from "@/stores/authStore";
import { formatNumberWithCommas } from "@/utils/number.utils";
import { getTextByCategoryCode } from "@/utils/transaction.utils";
import dayjs from "dayjs";
import { router } from "expo-router";
import { ChevronLeftIcon, ChevronRightIcon } from "lucide-react-native";
import { useEffect, useLayoutEffect, useMemo, useState } from "react";
import { Platform, SectionList, View } from "react-native";
import { useSafeAreaInsets } from "react-native-safe-area-context";

export default function App() {
  const { top } = useSafeAreaInsets();
  const { checkLoginAsync } = useAuthentication();
  const [openActionSheet, setOpenActionSheet] = useState(false);
  const [selectedTransaction, setSelectedTransaction] = useState(null);
  const { userProfile } = useAuthStore();
  const {
    create,
    update,
    remove,
    isCreating,
    selectedMonth,
    setSelectedMonth,
    groupTransactionsByDate,
    transactionsBySelectedMonth,
    groupTransactionsByCategoryCode,
    pullAllTransactionsMutation,
  } = useTransaction();

  useLayoutEffect(() => {
    checkLoginAsync();
  }, []);

  const { getTransactionByAI, isAskingAI } = useTransactionAI({
    onSuccess: (data) => create(data),
  });

  useEffect(() => {
    if (selectedTransaction) {
      setOpenActionSheet(true);
    }
  }, [selectedTransaction]);

  useEffect(() => {
    if (!openActionSheet) {
      setSelectedTransaction(null);
    }
  }, [openActionSheet]);

  const handleManualSubmitTransaction = (data) => {
    if (selectedTransaction) {
      update({
        ...selectedTransaction,
        amount: data.amount,
        description: data.description,
        expenseDate: data.expenseDate,
        category: {
          id: "",
          name: getTextByCategoryCode(data.categoryCode),
          code: data.categoryCode,
        },
      });
      setSelectedTransaction(null);
      setOpenActionSheet(false);
      return;
    }
    create({
      id: "",
      amount: data.amount,
      expenseDate: data.expenseDate,
      description: data.description,
      category: {
        id: "",
        name: getTextByCategoryCode(data.categoryCode),
        code: data.categoryCode,
      },
    });
    setSelectedTransaction(null);
    setOpenActionSheet(false);
  };

  const sections = useMemo(() => groupTransactionsByDate(transactionsBySelectedMonth), [transactionsBySelectedMonth]);

  const groupedByCategoryCode = useMemo(
    () => groupTransactionsByCategoryCode(transactionsBySelectedMonth),
    [transactionsBySelectedMonth]
  );

  const totalAmount = useMemo(
    () =>
      transactionsBySelectedMonth.reduce((acc, trans) => {
        return (acc += trans.amount);
      }, 0),
    [transactionsBySelectedMonth]
  );

  return (
    <View
      className="flex-1 justify-start items-center relative bg-neutral-50"
      style={{ paddingTop: Platform.OS === "ios" ? top : 40 }}
    >
      <HomeHeader />
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
              <Button
                variant="link"
                className="p-0"
                onPress={() => {
                  router.push({
                    pathname: "/detail",
                    params: { selectedMonth },
                  });
                }}
              >
                <ButtonIcon as={ChevronRightIcon} className="h-5 w-5 text-background-900" />
              </Button>
            </HStack>
          </HStack>
          {totalAmount > 0 && (
            <HStack>
              <HStack className="p-3 bg-neutral-200 rounded-xl relative items-center justify-center w-[calc(100%/2.6)]">
                <TransactionCategoryPieChart transactions={transactionsBySelectedMonth} />
              </HStack>
              <CategoryLegendGroup data={groupedByCategoryCode} />
            </HStack>
          )}
        </Card>
      </VStack>
      <SectionList
        keyboardShouldPersistTaps="handled"
        className="flex-1 mt-2"
        scrollEnabled
        sections={sections}
        ListFooterComponent={<Box className="h-40" />}
        renderItem={({ item, index, section }) => (
          <VStack>
            <TransactionCard
              onPressDelete={() => remove(item.id)}
              amount={item.amount}
              categoryCode={item.category?.code}
              description={item.description}
              onPress={() => setSelectedTransaction(item)}
            />
            {index < section.data.length - 1 && (
              <Box className="w-full px-4">
                <Divider />
              </Box>
            )}
          </VStack>
        )}
        renderSectionHeader={({ section }) => (
          <HStack className="w-full px-4 py-2 bg-background-50 justify-between">
            <Text className="font-bold text-lg text-neutral-900">{section.title}</Text>
            <Text className="font-bold text-lg text-neutral-800">
              {formatNumberWithCommas(section.data?.reduce((total, trans) => (total += trans.amount), 0))}đ
            </Text>
          </HStack>
        )}
        keyExtractor={(item) => item.id}
      />
      <TransactionDetailActionsheet
        isSubmitting={isCreating}
        isOpen={openActionSheet}
        handleClose={() => setOpenActionSheet(false)}
        onSubmit={handleManualSubmitTransaction}
        initValues={
          selectedTransaction && {
            description: selectedTransaction.description,
            amount: selectedTransaction.amount,
            expenseDate: selectedTransaction.expenseDate,
            categoryCode: selectedTransaction.category?.code,
          }
        }
      />
      <FloatingChat
        isLoading={isAskingAI}
        onPressCreate={() => setOpenActionSheet(true)}
        onMessage={getTransactionByAI}
        placeholder="VD: Đi bách hóa xanh 100k"
      />
    </View>
  );
}
