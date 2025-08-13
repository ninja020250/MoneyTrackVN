import { ITransaction } from "@/models/transaction.model";
import { getMainColorByCategoryCode } from "@/utils/transaction.utils";
import React from "react";
import { PieChart } from "react-native-svg-charts";

// Function to process data for PieChart
const processChartData = (transactions: ITransaction[]) => {
  const categoryTotals: { [key: string]: number } = {};
  let totalAmount = 0;

  // Sum up amounts per category and calculate total amount
  transactions.forEach((item) => {
    const categoryCode = item.category.code;
    categoryTotals[categoryCode] = (categoryTotals[categoryCode] || 0) + item.amount;
    totalAmount += item.amount;
  });

  // Convert into PieChart format
  return Object.keys(categoryTotals).map((category) => ({
    key: category,
    value: categoryTotals[category],
    percent: ((categoryTotals[category] / totalAmount) * 100).toFixed(2), // Calculate percentage
    svg: { fill: getMainColorByCategoryCode(category) }, // Use the function to get color
  }));
};

const TransactionPieChart = ({ transactions }: any) => {
  const chartData = processChartData(transactions);

  return <PieChart style={{ height: 100, width: 100 }} data={chartData} />;
};

export default TransactionPieChart;
