import { TransactionIconProps } from "@/components/Atom/TransactionIcon";
import { ITransaction } from "@/models/transaction.model";
import dayjs from "dayjs";

export const getMainColorByCategoryCode = (code: ITransaction["category"]["code"]): string | undefined => {
  const colors: Record<TransactionIconProps["categoryCode"], string> = {
    SHOPPING_001: "#FADA7A",
    ENTERTAINMENT_001: "#FF8A8A",
    TRAVEL_001: "#A6CDC6",
    FOOD_001: "#9ABF80",
    FIXED_001: "#1B4D3E",
    EDUCATION_001: "#3E5879",
    HEALTH_CARE_001: "#C890A7",
    INVESTMENT_001: "#7E5CAD",
    LIVING_001: "#FFB22C",
    UNFORESEEN_001: "#A31D1D",
  };
  return colors[code] ?? "grey";
};

export const getTextByCategoryCode = (code: ITransaction["category"]["code"]): string | undefined => {
  const colors: Record<TransactionIconProps["categoryCode"], string> = {
    SHOPPING_001: "Mua sắm",
    ENTERTAINMENT_001: "Giải trí",
    TRAVEL_001: "Du lịch",
    FOOD_001: "Ăn uống",
    FIXED_001: "Cố định",
    EDUCATION_001: "Giáo dục",
    HEALTH_CARE_001: "Sức khỏe",
    INVESTMENT_001: "Đầu tư",
    LIVING_001: "Sinh hoạt",
    UNFORESEEN_001: "Phát sinh",
  };
  return colors[code] ?? "Khác";
};

export const groupTransactionsByDate = (transactions) => {
  const grouped = transactions.reduce((acc, transaction) => {
    const date = dayjs(transaction.expenseDate).format("YYYY-MM-DD");
    if (!acc[date]) {
      acc[date] = [];
    }
    acc[date].push(transaction);
    return acc;
  }, {});

  return Object.keys(grouped)
    .sort((a, b) => dayjs(b).diff(dayjs(a))) // Sort by nearest date
    .map((date) => ({
      title: dayjs(date).isSame(dayjs(), "day") ? "Hôm nay" : dayjs(date).format("DD/MM/YYYY"),
      expenseDate: date,
      data: grouped[date],
    }));
};

export const sortTransactionByDate = (transactions) => {
  return transactions.sort((a, b) => dayjs(b.expenseDate).diff(dayjs(a.expenseDate)));
};

export const groupTransactionsByMonth = (transactions) => {
  const grouped = sortTransactionByDate(transactions).reduce((acc, transaction) => {
    const month = dayjs(transaction.expenseDate).format("YYYY-MM");
    if (!acc[month]) {
      acc[month] = [];
    }
    acc[month].push(transaction);
    return acc;
  }, {});

  return Object.keys(grouped).map((month) => ({
    title: dayjs(month).format("MM/YYYY"),
    month,
    data: grouped[month],
  }));
};

export const groupTransactionsByCategoryCode = (transactions) => {
  let total = 0;
  const grouped = transactions.reduce((acc, transaction) => {
    const categoryCode = transaction.category.code;
    total += transaction.amount;
    if (!acc[categoryCode]) {
      acc[categoryCode] = [];
    }
    acc[categoryCode].push(transaction);
    return acc;
  }, {});

  const group = Object.keys(grouped).map((categoryCode) => ({
    categoryCode,
    total,
    value: grouped[categoryCode].reduce((acc, item) => acc + item.amount, 0),
    data: grouped[categoryCode],
  }));

  return group;
};
