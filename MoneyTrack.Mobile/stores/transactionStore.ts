import { ITransaction } from "@/models/transaction.model";
import appStorage from "@/utils/asyncStorage.utils";
import { create } from "zustand";
import { immer } from "zustand/middleware/immer";

type State = {
  transactions: ITransaction[];
};

type Actions = {
  addTransaction: (transaction: ITransaction) => void;
  removeTransaction: (transactionId: string) => void;
  updateTransaction: (transaction: ITransaction) => void;
  setTransactions: (transactions: ITransaction[]) => void;
};

export const useTransactionStore = create<State & Actions>()(
  immer((set) => ({
    transactions: [],
    addTransaction: (transaction: ITransaction) =>
      set((state) => {
        state.transactions.unshift(transaction);
        appStorage.storeTransactionList(state.transactions);
      }),
    removeTransaction: (transactionId: string) =>
      set((state) => {
        state.transactions = state.transactions.filter((transaction) => transaction.id !== transactionId);
        appStorage.storeTransactionList(state.transactions);
      }),
    updateTransaction: (updatedTransaction: ITransaction) =>
      set((state) => {
        const index = state.transactions.findIndex((transaction) => transaction.id === updatedTransaction.id);
        if (index !== -1) {
          state.transactions[index] = updatedTransaction;
        }
        appStorage.storeTransactionList(state.transactions);
      }),
    setTransactions: (transactions: ITransaction[]) =>
      set((state) => {
        state.transactions = transactions;
        appStorage.storeTransactionList(state.transactions);
      }),
  }))
);
