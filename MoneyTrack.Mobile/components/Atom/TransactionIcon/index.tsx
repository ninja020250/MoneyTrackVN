import { Box } from "@/components/ui/box";
import { Icon } from "@/components/ui/icon";
import { ITransaction } from "@/models/transaction.model";
import { getMainColorByCategoryCode } from "@/utils/transaction.utils";
import {
  CircleHelpIcon,
  GiftIcon,
  GraduationCapIcon,
  HeartPulseIcon,
  HouseIcon,
  LandmarkIcon,
  LucideIcon,
  PlaneIcon,
  ReceiptTextIcon,
  ShoppingCartIcon,
  TicketIcon,
  UtensilsIcon,
} from "lucide-react-native";
import React from "react";

export type TransactionIconProps = {
  categoryCode: ITransaction["category"]["code"];
};

export const transactionCategoryIcons: Record<TransactionIconProps["categoryCode"], LucideIcon> = {
  SHOPPING_001: ShoppingCartIcon,
  ENTERTAINMENT_001: TicketIcon,
  TRAVEL_001: PlaneIcon,
  FOOD_001: UtensilsIcon,
  FIXED_001: HouseIcon,
  EDUCATION_001: GraduationCapIcon,
  HEALTH_CARE_001: HeartPulseIcon,
  INVESTMENT_001: LandmarkIcon,
  LIVING_001: ReceiptTextIcon,
  UNFORESEEN_001: GiftIcon,
};

export const TransactionIcon = ({ categoryCode }) => {
  const color = getMainColorByCategoryCode(categoryCode);
  return (
    <Box className="bg-primary-info/50 p-2 rounded-xl border border-neutral-400">
      <Icon as={transactionCategoryIcons[categoryCode] ?? CircleHelpIcon} size="md" style={{ color }} />
    </Box>
  );
};

export default TransactionIcon;
