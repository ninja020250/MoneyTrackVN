import { ITransaction } from "@/models/transaction.model";
import { Storage } from "./asyncStorage.utils";
import { ISyncQueueTransaction } from "@/models/transaction-queue.model";

export class QueueTransactionStorage extends Storage {
  private key = "queueTransaction";

  queueTransaction = async (tx: ITransaction, operation: "create" | "update" | "delete") => {
    const _id = `tx_${tx.id}`;
    const listItems: ISyncQueueTransaction[] = await this.getArrayValue(this.key);
    const existing: ISyncQueueTransaction = listItems.find((item) => item._id === _id);

    switch (operation) {
      case "delete": {
        if (existing && existing.operation === "create") {
          const deleted = listItems.filter((item) => item._id !== _id);
          await this.setArrayValue(this.key, deleted);
        } else {
          listItems.push({
            ...tx,
            _id,
            operation,
            isSynced: false,
            timestamp: Date.now(),
          });
          await this.setArrayValue(this.key, listItems);
        }
        break;
      }
      case "update": {
        if (existing) {
          const updatedItems = listItems.map((item) =>
            item._id === _id ? { ...item, ...tx, timestamp: Date.now() } : item
          );
          await this.setArrayValue(this.key, updatedItems);
        } else {
          listItems.push({
            ...tx,
            _id,
            operation,
            isSynced: false,
            timestamp: Date.now(),
          });
          await this.setArrayValue(this.key, listItems);
        }
        break;
      }
      case "create": {
        listItems.push({
          ...tx,
          _id,
          operation,
          isSynced: false,
          timestamp: Date.now(),
        });
        await this.setArrayValue(this.key, listItems);
        break;
      }
      default:
        break;
    }
  };

  getQueueTransactions = (): Promise<ISyncQueueTransaction[]> => {
    return this.getArrayValue(this.key);
  };

  clear = () => {
    return this.setArrayValue(this.key, []);
  };
}

export default new QueueTransactionStorage();
