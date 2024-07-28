import {
  FC,
  useContext,
  useEffect,
  useMemo,
  useState,
  createContext,
  ReactNode,
} from 'react';
import { CandleDetail } from '../typesV2/constructor/CandleDetail';
import { ConfiguredCandle } from '../typesV2/order/ConfiguredCandle';
import { ConstructorApi } from '../services/ConstructorApi';

export interface CandleProviderProps {
  children?: ReactNode;
}

type ICandleContext = {
  candle: CandleDetail | undefined;
  configuredCandle: ConfiguredCandle | undefined;
  priceConfiguredCandle: number;
  setCandle: (candleDetail: CandleDetail) => void;
  setConfiguredCandle: (configuredCandle: ConfiguredCandle) => void;
  fetchCandleById: (id: string) => Promise<void>;
};

const initialValue: ICandleContext = {
  candle: undefined,
  configuredCandle: undefined,
  priceConfiguredCandle: 0,
  setCandle: () => {},
  setConfiguredCandle: () => {},
  fetchCandleById: async () => {},
};

const CandleContext = createContext<ICandleContext>(initialValue);

export const CandleProvider: FC<CandleProviderProps> = ({ children }) => {
  const [candle, setCandle] = useState<CandleDetail | undefined>(
    initialValue.candle,
  );
  const [configuredCandle, setConfiguredCandle] = useState<
    ConfiguredCandle | undefined
  >(initialValue.configuredCandle);
  const [priceConfiguredCandle, setPriceConfiguredCandle] = useState<number>(
    initialValue.priceConfiguredCandle,
  );

  const fetchCandleById = async (id: string) => {
    const candleDetailResponse = await ConstructorApi.getCandleById(id);
    if (candleDetailResponse.data && !candleDetailResponse.error) {
      setCandle(candleDetailResponse.data);
    } else {
      console.error(
        'Ошибка при загрузке данных с сервера',
        candleDetailResponse.error,
      );
    }
  };

  const handleSetConfiguredCandle = (configuredCandle: ConfiguredCandle) => {
    setConfiguredCandle(configuredCandle);
    setCandle(undefined);
  };

  useEffect(() => {}, []);

  const contextValue = useMemo(
    () => ({
      candle,
      configuredCandle,
      priceConfiguredCandle,
      setCandle,
      setConfiguredCandle: handleSetConfiguredCandle,
      fetchCandleById,
    }),
    [candle, configuredCandle],
  );

  console.log(candle);

  return (
    <CandleContext.Provider value={contextValue}>
      {children}
    </CandleContext.Provider>
  );
};

export const useCandleContext = () => useContext(CandleContext);
