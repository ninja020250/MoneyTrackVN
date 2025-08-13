import { ITransaction, ITransactionCategory, ITransactionDto } from "@/models/transaction.model";
import queueTransactionStorage from "@/utils/queueTransaction.utils";
import BaseService from "./BaseService";

export type LoginParams = {
  username: string;
  password: string;
};

class TransactionService extends BaseService {
  public session: any = {};

  constructor() {
    super(`${process.env.EXPO_PUBLIC_API_URL}/api/Transaction`);
  }

  getAll = async (): Promise<ITransaction[]> => {
    try {
      const res = await this.instance.get("/");
      return res.data["$values"].map((transaction: ITransaction) => ({
        ...transaction,
        categoryCode: transaction.category?.code,
      }));
    } catch (error) {
      return [];
    }
  };

  create = async (transaction: ITransactionDto): Promise<ITransaction | null> => {
    try {
      const res = await this.instance.post("/", transaction);
      return res.data;
    } catch (error) {
      return null;
    }
  };

  update = async (req: {
    description: string;
    amount: number;
    id: string;
    expenseDate: Date;
    categoryCode: ITransactionCategory["code"];
  }): Promise<ITransaction | null> => {
    try {
      const res = await this.instance.put("/" + req.id, req);
      return res.data;
    } catch (error) {
      return null;
    }
  };

  delete = async (id: string): Promise<ITransaction | null> => {
    try {
      const res = await this.instance.delete("/" + id);
      return res.data;
    } catch (error) {
      return null;
    }
  };

  askAI = async (message: string): Promise<ITransaction | null> => {
    try {
      const res = await this.instance.post("/transaction-by-ai", { message, isPublic: true });
      return res.data;
    } catch (error) {
      throw error;
    }
  };

  askPrivateAI = async (message: string): Promise<ITransaction | null> => {
    try {
      const res = await this.instance.post("/transaction-by-private-ai", { message });
      return res.data;
    } catch (error) {
      throw error;
    }
  };

  syncToCloud = async () => {
    try {
      let transactionToSync = await queueTransactionStorage.getQueueTransactions();
      const groupedTransactions = {
        create: transactionToSync
          .filter((tx) => tx.operation === "create")
          .map((tx: ITransactionDto) => {
            tx.categoryCode = tx.category?.code;
            delete tx["$id"];
            return tx;
          }),
        update: transactionToSync
          .filter((tx) => tx.operation === "update")
          .map((tx: ITransactionDto) => {
            tx.categoryCode = tx.category?.code;
            delete tx["$id"];
            return tx;
          }),
        delete: transactionToSync
          .filter((tx) => tx.operation === "delete")
          .map((tx: ITransactionDto) => {
            return tx.id;
          }),
      };
      if (groupedTransactions.create?.length) {
        await this.instance.post("/bulk-create", groupedTransactions.create);
      }
      if (groupedTransactions.update?.length) {
        await this.instance.post("/bulk-update", groupedTransactions.update);
      }
      if (groupedTransactions.delete?.length) {
        await this.instance.post("/bulk-delete", groupedTransactions.delete);
      }
      await queueTransactionStorage.clear();
    } catch (error) {
      return null;
    }
  };
}

// Export singleton instance
export default new TransactionService();
