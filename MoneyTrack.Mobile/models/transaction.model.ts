export interface ITransaction {
  id: string;
  description?: string;
  amount: number;
  createdDate?: Date;
  updatedDate?: Date;
  userId?: string;
  categoryId?: string;
  category?: ITransactionCategory;
  expenseDate?: Date;
}

export interface ITransactionDto extends Omit<ITransaction, "id"> {
  id?: string;
  categoryCode?: string;
}

export interface ITransactionCategory {
  id: string;
  name: string;
  code:
    | "SHOPPING_001"
    | "ENTERTAINMENT_001"
    | "TRAVEL_001"
    | "FOOD_001"
    | "FIXED_001"
    | "EDUCATION_001"
    | "HEALTH_CARE_001"
    | "INVESTMENT_001"
    | string;
}

export interface ITransactionCategoryStatistics extends ITransactionCategory {
  amount: number;
}
