import BaseContainer from "@/components/Atom/BaseContainer";
import TransactionCard from "@/components/molecules/TransactionCard";
import TransactionDetailActionsheet from "@/components/Organism/TransactionDetailActionsheet";
import { View } from "@/components/Themed";
import { Box } from "@/components/ui/box";
import { Button, ButtonIcon } from "@/components/ui/button";
import { Divider } from "@/components/ui/divider";
import { HStack } from "@/components/ui/hstack";
import { Text } from "@/components/ui/text";
import { VStack } from "@/components/ui/vstack";
import { useTransaction } from "@/hooks/useTransaction";
import { formatNumberWithCommas } from "@/utils/number.utils";
import { getTextByCategoryCode } from "@/utils/transaction.utils";
import { router, useLocalSearchParams } from "expo-router";
import { ArrowLeftIcon } from "lucide-react-native";
import React, { useEffect, useMemo, useState } from "react";
import { SectionList } from "react-native";

export const CategoryDetail = () => {
  const { categoryCode, selectedMonth } = useLocalSearchParams();
  const { create, update, remove, isCreating, groupTransactionsByDate, transactionsBySelectedMonth } = useTransaction({
    defaultSelectedMonth: selectedMonth,
  });

  const [openActionSheet, setOpenActionSheet] = useState(false);
  const [selectedTransaction, setSelectedTransaction] = useState(null);

  const onBack = () => {
    router.dismissTo("/detail");
  };

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

  const sections = useMemo(() => {
    const filterByCategoryCode = transactionsBySelectedMonth.filter((trans) => trans.category?.code === categoryCode);
    return groupTransactionsByDate(filterByCategoryCode);
  }, [transactionsBySelectedMonth]);

  return (
    <BaseContainer>
      <HStack className="justify-between items-center w-full gap-1 px-4">
        <Button variant="link" onPress={onBack} className="p-0">
          <ButtonIcon as={ArrowLeftIcon} className="h-6 w-6 text-background-900" />
        </Button>
        <Text className="text-lg text-typography-800 mb-2 mt-4" bold>
          {getTextByCategoryCode(categoryCode as string)}
        </Text>
        <View className="w-2" />
      </HStack>
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
    </BaseContainer>
  );
};

export default CategoryDetail;
