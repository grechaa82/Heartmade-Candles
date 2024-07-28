import {
  FC,
  useContext,
  useEffect,
  useMemo,
  useState,
  createContext,
  ReactNode,
} from 'react';
import { CandleTypeWithCandles } from '../typesV2/constructor/CandleTypeWithCandles';
import { ConstructorApi } from '../services/ConstructorApi';
import { ConfiguredCandleDetail } from '../typesV2/constructor/ConfiguredCandleDetail';
import { calculatePrice } from '../helpers/CalculatePrice';

export interface ConstructorProviderProps {
  children?: ReactNode;
}

type IConstructorContext = {
  candleTypeWithCandles: CandleTypeWithCandles[];
  configuredCandles: ConfiguredCandleDetail[];
  totalPrice: number;
  setConfiguredCandles: (configuredCandles: ConfiguredCandleDetail[]) => void;
};

const initialValue: IConstructorContext = {
  candleTypeWithCandles: [],
  configuredCandles: [],
  totalPrice: 0,
  setConfiguredCandles: () => {},
};

const ConstructorContext = createContext<IConstructorContext>(initialValue);

export const ConstructorProvider: FC<ConstructorProviderProps> = ({
  children,
}) => {
  const [candleTypeWithCandles, setCandleTypeWithCandles] = useState<
    CandleTypeWithCandles[]
  >(initialValue.candleTypeWithCandles);

  const [configuredCandles, setConfiguredCandles] = useState<
    ConfiguredCandleDetail[]
  >([]);

  const [totalPrice, setTotalPrice] = useState<number>(initialValue.totalPrice);

  const handleSetConfiguredCandles = (
    configuredCandles: ConfiguredCandleDetail[],
  ) => {
    setConfiguredCandles(configuredCandles);
    setTotalPrice(
      calculateTotalPrice(
        configuredCandles.map((candleWithValidity) => {
          return candleWithValidity;
        }),
      ),
    );
  };

  function calculateTotalPrice(configuredCandles: ConfiguredCandleDetail[]) {
    let newTotalPrice = 0;
    for (const configuredCandle of configuredCandles) {
      newTotalPrice += Math.round(
        calculatePrice(configuredCandle) * configuredCandle.quantity,
      );
    }
    return newTotalPrice;
  }

  useEffect(() => {
    async function fetchData() {
      const candlesResponse = await ConstructorApi.getCandles();
      if (candlesResponse.data && !candlesResponse.error) {
        setCandleTypeWithCandles(candlesResponse.data);
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
      candleTypeWithCandles,
      configuredCandles,
      totalPrice,
      setConfiguredCandles: handleSetConfiguredCandles,
    }),
    [candleTypeWithCandles, configuredCandles, totalPrice],
  );

  return (
    <ConstructorContext.Provider value={contextValue}>
      {children}
    </ConstructorContext.Provider>
  );
};

export const useConstructorContext = () => useContext(ConstructorContext);
