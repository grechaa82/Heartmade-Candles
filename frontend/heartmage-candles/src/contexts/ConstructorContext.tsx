import {
  FC,
  useContext,
  useEffect,
  useMemo,
  useState,
  createContext,
  ReactNode,
} from 'react';
import { CandlesByType } from '../typesV2/constructor/CandlesByType';
import { ConstructorApi } from '../services/ConstructorApi';
import { ConfiguredCandleDetail } from '../typesV2/constructor/ConfiguredCandleDetail';
import {
  calculateCustomCandlePrice,
  calculatePrice,
} from '../helpers/CalculatePrice';
import { CustomCandle } from '../typesV2/constructor/CustomCandle';

export interface ConstructorProviderProps {
  children?: ReactNode;
}

type IConstructorContext = {
  candlesByType: CandlesByType[];
  configuredCandles: ConfiguredCandleDetail[];
  totalPrice: number;
  setConfiguredCandles: (configuredCandles: ConfiguredCandleDetail[]) => void;
  customCandles: CustomCandle[];
  setCustomCandles: (customCandle: CustomCandle[]) => void;
};

const initialValue: IConstructorContext = {
  candlesByType: [],
  configuredCandles: [],
  totalPrice: 0,
  setConfiguredCandles: () => {},
  customCandles: [],
  setCustomCandles: () => {},
};

const ConstructorContext = createContext<IConstructorContext>(initialValue);

export const ConstructorProvider: FC<ConstructorProviderProps> = ({
  children,
}) => {
  const [candlesByType, setCandlesByType] = useState<CandlesByType[]>(
    initialValue.candlesByType,
  );

  const [configuredCandles, setConfiguredCandles] = useState<
    ConfiguredCandleDetail[]
  >([]);

  const [totalPrice, setTotalPrice] = useState<number>(initialValue.totalPrice);

  const [customCandles, setCustomCandles] = useState<CustomCandle[]>(
    initialValue.customCandles,
  );

  const handleSetConfiguredCandles = (
    configuredCandles: ConfiguredCandleDetail[],
  ) => {
    setConfiguredCandles(configuredCandles);
  };

  const handleSetCustomCandles = (customCandles: CustomCandle[]) => {
    setCustomCandles(customCandles);
    setTotalPrice(
      calculateTotalPrice(
        customCandles.map((candleWithValidity) => {
          return candleWithValidity;
        }),
      ),
    );
  };

  function calculateTotalPrice(customCandles: CustomCandle[]) {
    let newTotalPrice = 0;
    for (const customCandle of customCandles) {
      newTotalPrice += Math.round(
        calculateCustomCandlePrice(customCandle) * customCandle.quantity,
      );
    }
    return newTotalPrice;
  }

  useEffect(() => {
    async function fetchData() {
      const candlesResponse = await ConstructorApi.getCandles();
      if (candlesResponse.data && !candlesResponse.error) {
        setCandlesByType(candlesResponse.data);
      } else {
        console.error(
          'Ошибка при загрузке данных с сервера',
          candlesResponse.error,
        );
      }
    }

    fetchData();
  }, []);

  const contextValue = useMemo(
    () => ({
      candlesByType,
      configuredCandles,
      totalPrice,
      setConfiguredCandles: handleSetConfiguredCandles,
      customCandles,
      setCustomCandles: handleSetCustomCandles,
    }),
    [candlesByType, configuredCandles, totalPrice, customCandles],
  );

  return (
    <ConstructorContext.Provider value={contextValue}>
      {children}
    </ConstructorContext.Provider>
  );
};

export const useConstructorContext = () => useContext(ConstructorContext);
