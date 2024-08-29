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
  loadMoreCandlesByType: (
    typeCandle: string,
    pageSize: number,
    pageIndex: number,
  ) => void;
  isLoadingCandlesByType: boolean;
};

const initialValue: IConstructorContext = {
  candlesByType: [],
  totalPrice: 0,
  customCandles: [],
  setCustomCandles: () => {},
  loadMoreCandlesByType: () => {},
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

  const loadMoreCandlesByType = async (
    typeCandle: string,
    pageSize: number,
    pageIndex: number,
  ) => {
    const candleDetailResponse = await ConstructorApi.getCandlesByType(
      typeCandle,
      pageSize,
      pageIndex,
    );

    if (candleDetailResponse.data && !candleDetailResponse.error) {
      setCandlesByType((prevCandlesByType) => {
        const existingType = prevCandlesByType.find(
          (candles) => candles.type === typeCandle,
        );
        if (existingType) {
          return prevCandlesByType.map((candles) => {
            if (candles.type === typeCandle) {
              return {
                ...candles,
                candles: [...candles.candles, ...candleDetailResponse.data],
                pageIndex: pageIndex,
              };
            }
            return candles;
          });
        } else {
          return [
            ...prevCandlesByType,
            { type: typeCandle, candles: candleDetailResponse.data },
          ];
        }
      });
    }
  };

  useEffect(() => {
    async function fetchData() {
      const candlesResponse = await ConstructorApi.getCandles();
      if (candlesResponse.data && !candlesResponse.error) {
        const updatedCandlesByType = candlesResponse.data.map((candles) => ({
          ...candles,
          pageSize: candles.candles.length,
          pageIndex: 0,
        }));
        setCandlesByType(updatedCandlesByType);
        setIsLoadingCandlesByType(false);
      }
    }

    fetchData();
  }, []);

  // useEffect(() => {
  //   const delay = 45000;
  //   async function fetchData() {
  //     const currentCandlesByType = candlesByType;
  //     const updatedCandlesByType: CandlesByType[] = new Array(
  //       currentCandlesByType.length,
  //     ).fill(null);

  //     const promises = currentCandlesByType.map(async (item, index) => {
  //       const candlesResponse = await ConstructorApi.getCandlesByType(
  //         item.type,
  //         item.candles.length,
  //         0,
  //       );
  //       if (candlesResponse.data && !candlesResponse.error) {
  //         const newCandlesByType: CandlesByType = {
  //           type: item.type,
  //           candles: candlesResponse.data,
  //           pageSize: item.pageSize,
  //           pageIndex: item.pageIndex,
  //         };
  //         updatedCandlesByType[index] = newCandlesByType;
  //       }
  //     });

  //     await Promise.all(promises);

  //     if (updatedCandlesByType.length > 0) {
  //       setCandlesByType(updatedCandlesByType);
  //     }
  //     setIsLoadingCandlesByType(false);

  //     await checkConditionOfCandles(customCandles);
  //   }

  //   fetchData();
  //   const interval = setInterval(fetchData, delay);
  //   return () => clearInterval(interval);
  // }, []);

  const contextValue = useMemo(
    () => ({
      candlesByType,
      totalPrice,
      customCandles,
      setCustomCandles: handleSetCustomCandles,
      loadMoreCandlesByType,
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
