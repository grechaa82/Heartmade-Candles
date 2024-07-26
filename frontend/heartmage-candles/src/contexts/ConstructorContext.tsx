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

export interface ConstructorProviderProps {
  children?: ReactNode;
}

type IConstructorContext = {
  candleTypeWithCandles: CandleTypeWithCandles[];
};

const initialValue: IConstructorContext = {
  candleTypeWithCandles: [],
};

const ConstructorContext = createContext<IConstructorContext>(initialValue);

export const ConstructorProvider: FC<ConstructorProviderProps> = ({
  children,
}) => {
  const [candleTypeWithCandles, setCandleTypeWithCandles] = useState<
    CandleTypeWithCandles[]
  >(initialValue.candleTypeWithCandles);

  useEffect(() => {
    async function fetchData() {
      const candlesResponse = await ConstructorApi.getCandles();
      console.log('Candles response:', candlesResponse);
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
    }),
    [candleTypeWithCandles],
  );

  return (
    <ConstructorContext.Provider value={contextValue}>
      {children}
    </ConstructorContext.Provider>
  );
};

export const useConstructorContext = () => useContext(ConstructorContext);
