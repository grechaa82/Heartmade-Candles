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
import { calculateCustomCandlePrice } from '../helpers/CalculatePrice';
import { CustomCandle } from '../typesV2/constructor/CustomCandle';
import { CandleDetail } from '../typesV2/constructor/CandleDetail';
import { CustomCandleBuilder } from '../typesV2/constructor/CustomCandleBuilder';

export interface ConstructorProviderProps {
  children?: ReactNode;
}

type IConstructorContext = {
  candlesByType: CandlesByType[];
  totalPrice: number;
  customCandles: CustomCandle[];
  setCustomCandles: (customCandle: CustomCandle[]) => void;
  isLoadingCandlesByType: boolean;
};

const initialValue: IConstructorContext = {
  candlesByType: [],
  totalPrice: 0,
  customCandles: [],
  setCustomCandles: () => {},
  isLoadingCandlesByType: true,
};

const ConstructorContext = createContext<IConstructorContext>(initialValue);

export const ConstructorProvider: FC<ConstructorProviderProps> = ({
  children,
}) => {
  const [candlesByType, setCandlesByType] = useState<CandlesByType[]>(
    initialValue.candlesByType,
  );
  const [totalPrice, setTotalPrice] = useState<number>(initialValue.totalPrice);
  const [customCandles, setCustomCandles] = useState<CustomCandle[]>(
    initialValue.customCandles,
  );
  const [isLoadingCandlesByType, setIsLoadingCandlesByType] = useState(
    initialValue.isLoadingCandlesByType,
  );

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
    const delay = 60000;

    async function fetchData() {
      const candlesResponse = await ConstructorApi.getCandles();
      if (candlesResponse.data && !candlesResponse.error) {
        setCandlesByType(candlesResponse.data);
        setIsLoadingCandlesByType(false);
      } else {
      }
    }

    fetchData();
    const interval = setInterval(fetchData, delay);

    return () => clearInterval(interval);
  }, []);

  useEffect(() => {
    let newCustomCandles: CustomCandle[] = [];
    let candleDetails: CandleDetail[] = [];

    const fetchCandleDetails = async (customCandle) => {
      const existingDetail = candleDetails.find(
        (detail) => detail.candle?.id === customCandle.candle?.id,
      );

      if (existingDetail) {
        const newCustomCandle =
          CustomCandleBuilder.checkCustomCandleAgainstCandleDetail(
            customCandle,
            existingDetail,
          );
        if (newCustomCandles) {
          newCustomCandles.push(newCustomCandle);
        } else {
        }
      } else {
        const candleDetailResponse = await ConstructorApi.getCandleById(
          customCandle.candle.id.toString(),
        );

        if (candleDetailResponse.data && !candleDetailResponse.error) {
          const newCustomCandle =
            CustomCandleBuilder.checkCustomCandleAgainstCandleDetail(
              customCandle,
              candleDetailResponse.data,
            );
          if (newCustomCandles) {
            newCustomCandles.push(newCustomCandle);
            candleDetails.push(candleDetailResponse.data);
          } else {
          }
        } else {
        }
      }
    };

    const fetchAllCustomCandles = async () => {
      await Promise.all(customCandles.map(fetchCandleDetails));
      setCustomCandles(newCustomCandles);
    };

    fetchAllCustomCandles();
  }, [candlesByType, setCandlesByType]);

  const contextValue = useMemo(
    () => ({
      candlesByType,
      totalPrice,
      customCandles,
      setCustomCandles: handleSetCustomCandles,
      isLoadingCandlesByType,
    }),
    [candlesByType, totalPrice, customCandles],
  );

  return (
    <ConstructorContext.Provider value={contextValue}>
      {children}
    </ConstructorContext.Provider>
  );
};

export const useConstructorContext = () => useContext(ConstructorContext);
