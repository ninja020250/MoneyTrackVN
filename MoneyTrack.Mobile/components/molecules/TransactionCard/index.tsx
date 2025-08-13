import TransactionIcon from "@/components/Atom/TransactionIcon";
import { Box } from "@/components/ui/box";
import { Button } from "@/components/ui/button";
import { HStack } from "@/components/ui/hstack";
import { Icon } from "@/components/ui/icon";
import { Text } from "@/components/ui/text";
import { ITransaction } from "@/models/transaction.model";
import { formatNumberWithCommas } from "@/utils/number.utils";
import { TrashIcon } from "lucide-react-native";
import React from "react";
import { TouchableOpacity, TouchableWithoutFeedback } from "react-native";
import { GestureHandlerRootView } from "react-native-gesture-handler";
import ReanimatedSwipeable from "react-native-gesture-handler/ReanimatedSwipeable";
import Reanimated, { SharedValue, useAnimatedStyle } from "react-native-reanimated";

export type TransactionCardProps = {
  description: ITransaction["description"];
  categoryCode: ITransaction["category"]["code"];
  amount: ITransaction["amount"];
  onPress: () => void;
  onPressDelete: () => void;
};

function RightAction({
  prog,
  drag,
  onPressDelete,
}: {
  prog: SharedValue<number>;
  drag: SharedValue<number>;
  onPressDelete: () => void;
}) {
  const styleAnimation = useAnimatedStyle(() => {
    return {
      transform: [{ translateX: drag.value + 55 }],
      opacity: prog.value,
    };
  });

  return (
    <Reanimated.View style={styleAnimation}>
      <HStack className="h-full gap-4 items-center">
        <Box className="border-l border-outline-300 ml-4 h-[60%]" />
        <Button onPress={onPressDelete} variant="solid" className="px-3 py-4 mt-auto mb-auto bg-error-700">
          <Icon as={TrashIcon} className="text-white" />
        </Button>
      </HStack>
    </Reanimated.View>
  );
}

export const TransactionCard = ({
  description,
  categoryCode,
  amount,
  onPress,
  onPressDelete,
}: TransactionCardProps) => {
  return (
    <TouchableWithoutFeedback onPress={onPress}>
      <GestureHandlerRootView>
        <ReanimatedSwipeable
          friction={2}
          enableTrackpadTwoFingerGesture
          rightThreshold={40}
          renderRightActions={(prog, drag) => <RightAction prog={prog} drag={drag} onPressDelete={onPressDelete} />}
        >
          <HStack className="justify-start items-center w-full p-4 gap-2">
            <TransactionIcon categoryCode={categoryCode} />
            <Text className="">{description}</Text>
            <HStack className="ml-auto items-center gap-1">
              <Text className="neutral-600 font-bold">{`${formatNumberWithCommas(amount)}đ`}</Text>
              {/* <Icon as={ChevronRight} className="neutral-600" size="sm" /> */}
            </HStack>
          </HStack>
        </ReanimatedSwipeable>
      </GestureHandlerRootView>
    </TouchableWithoutFeedback>
  );
};

export default TransactionCard;
