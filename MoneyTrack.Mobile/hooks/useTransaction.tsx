import { useMutation } from "@tanstack/react-query";
import dayjs from "dayjs";
import { useLayoutEffect, useMemo, useState } from "react";
import uuid from "react-native-uuid";

import { ITransaction } from "@/models/transaction.model";
import TransactionService from "@/services/TransactionService";
import { useAuthStore } from "@/stores/authStore";
import { useTransactionStore } from "@/stores/transactionStore";
import appStorage from "@/utils/asyncStorage.utils";
import queueTransactionStorage from "@/utils/queueTransaction.utils";
import {
  groupTransactionsByCategoryCode,
  groupTransactionsByDate,
  groupTransactionsByMonth,
} from "@/utils/transaction.utils";

export const useTransaction = ({ defaultSelectedMonth }: any = {}) => {
  const transactions = useTransactionStore((state) => state.transactions);
  const { addTransaction, removeTransaction, updateTransaction, setTransactions } = useTransactionStore();
  const userProfile = useAuthStore((state) => state.userProfile);
  const [selectedMonth, setSelectedMonth] = useState(
    defaultSelectedMonth || dayjs().startOf("month").format("YYYY-MM")
  );

  const createMutation = useMutation({
    mutationKey: ["createTransaction"],
    mutationFn: TransactionService.create,
    onError: (err, val) => {
      queueTransactionStorage.queueTransaction(val as ITransaction, "create");
    },
  });

  const updateMutation = useMutation({
    mutationKey: ["updateTransaction"],
    mutationFn: TransactionService.update,
    onError: (err, val) => {
      const transactionToUpdate = transactions.find((t) => t.id === val.id);
      transactionToUpdate.amount = val.amount;
      transactionToUpdate.description = val.description;
      transactionToUpdate.expenseDate = val.expenseDate;
      queueTransactionStorage.queueTransaction(val as ITransaction, "update");
    },
  });

  const syncToCloudMutation = useMutation({
    mutationKey: ["sync-to-cloud"],
    mutationFn: TransactionService.syncToCloud,
  });

  const deleteMutation = useMutation({
    mutationKey: ["deleteTransaction"],
    mutationFn: TransactionService.delete,
    onError: (err, id) => {
      const transactionToDelete = transactions.find((t) => t.id === id);
      queueTransactionStorage.queueTransaction(transactionToDelete, "delete");
    },
  });

  const initData = async () => {
    if (transactions.length === 0) {
      const listFromStorage: ITransaction[] = await appStorage.getTransactionList();
      setTransactions(listFromStorage.map((tx) => ({ ...tx, expenseDate: tx.expenseDate || tx.createdDate })));
    }
  };

  const createNewTransaction = async (newTransaction: ITransaction) => {
    newTransaction.id = uuid.v4();
    newTransaction.description = newTransaction.description.trim();
    const setting = await appStorage.getSetting();
    addTransaction(newTransaction);
    if (userProfile?.id && setting.syncToCloud) {
      const payload = { ...newTransaction, categoryCode: newTransaction.category?.code };
      createMutation.mutate(payload);
    } else {
      await queueTransactionStorage.queueTransaction(newTransaction, "create");
      const unsynced = await queueTransactionStorage.getQueueTransactions();
      console.table(unsynced);
    }
  };

  const updateNewTransaction = async (newTransaction: ITransaction) => {
    updateTransaction(newTransaction);
    const setting = await appStorage.getSetting();
    if (userProfile?.id && setting.syncToCloud) {
      updateMutation.mutate({
        description: newTransaction.description,
        amount: newTransaction.amount,
        id: newTransaction.id,
        expenseDate: newTransaction.expenseDate,
        categoryCode: newTransaction.category?.code,
      });
    } else {
      await queueTransactionStorage.queueTransaction(newTransaction, "update");
      const unsynced = await queueTransactionStorage.getQueueTransactions();
      console.table(unsynced);
    }
  };

  const handleDeleteTransaction = async (id) => {
    removeTransaction(id);
    const setting = await appStorage.getSetting();
    if (userProfile?.id && setting.syncToCloud) {
      deleteMutation.mutate(id);
    } else {
      const transactionToDelete = transactions.find((t) => t.id === id);
      await queueTransactionStorage.queueTransaction(transactionToDelete, "delete");
      const unsynced = await queueTransactionStorage.getQueueTransactions();
      console.table(unsynced);
    }
  };

  const pullAllTransactionsMutation = useMutation({
    mutationKey: ["pull-all-transactions"],
    mutationFn: TransactionService.getAll,
    onSuccess: async (remoteData) => {
      const localData = await appStorage.getTransactionList();
      // Create a map of transactions by ID for faster lookup
      const localMap = new Map(localData.map((tx) => [tx.id, tx]));
      const remoteMap = new Map(remoteData.map((tx) => [tx.id, tx]));
      // Merge data, prioritizing local transactions
      const mergedData = [...remoteData];
      // Override remote data with local data where IDs match
      localData.forEach((localTx) => {
        const index = mergedData.findIndex((tx) => tx.id === localTx.id);
        if (index !== -1) {
          mergedData[index] = localTx;
        } else {
          mergedData.push(localTx);
        }
      });
      // Update state and storage with merged data
      setTransactions(mergedData);
      await appStorage.storeTransactionList(mergedData);
    },
  });

  const transactionsBySelectedMonth = useMemo(
    () => transactions.filter((transaction) => dayjs(transaction.expenseDate).isSame(selectedMonth, "month")),
    [transactions, selectedMonth]
  );

  useLayoutEffect(() => {
    initData();
  }, []);

  const totalAmount = useMemo(
    () =>
      transactionsBySelectedMonth.reduce((acc, trans) => {
        return (acc += trans.amount);
      }, 0),
    [transactionsBySelectedMonth]
  );

  return {
    create: createNewTransaction,
    update: updateNewTransaction,
    remove: handleDeleteTransaction,
    syncToCloud: syncToCloudMutation.mutate,
    isSyncing: syncToCloudMutation.isPending,
    syncSuccess: syncToCloudMutation.isSuccess,
    groupTransactionsByDate,
    groupTransactionsByMonth,
    groupTransactionsByCategoryCode,
    setSelectedMonth,
    pullAllTransactionsMutation: pullAllTransactionsMutation.mutate,
    isPullingTransactions: pullAllTransactionsMutation.isPending,
    selectedMonth,
    isCreating: createMutation.isPending,
    transactionsBySelectedMonth,
    totalAmount,
  };
};
