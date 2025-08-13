import { ITransaction } from "./transaction.model";

export interface ISyncQueueTransaction extends ITransaction {
  _id: string;
  operation: "create" | "update" | "delete";
  isSynced: boolean;
  timestamp: number;
}
