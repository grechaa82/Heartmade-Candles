import {
  FC,
  useContext,
  useMemo,
  useState,
  createContext,
  ReactNode,
} from 'react';
import { CandleDetail } from '../typesV2/constructor/CandleDetail';
import { ConstructorApi } from '../services/ConstructorApi';
import { CustomCandleBuilder } from '../typesV2/constructor/CustomCandleBuilder';

export interface CandleProviderProps {
  children?: ReactNode;
}

type ICandleContext = {
  candle: CandleDetail | undefined;
  customCandleBuilder: CustomCandleBuilder;
  fetchCandleById: (id: string) => Promise<void>;
  setCustomCandleBuilder: (customCandleBuilder: CustomCandleBuilder) => void;
  updateCustomCandleBuilder: () => void;
};

const initialValue: ICandleContext = {
  candle: undefined,
  customCandleBuilder: new CustomCandleBuilder(),
  fetchCandleById: async () => {},
  setCustomCandleBuilder: () => {},
  updateCustomCandleBuilder: () => {},
};

const CandleContext = createContext<ICandleContext>(initialValue);

export const CandleProvider: FC<CandleProviderProps> = ({ children }) => {
  const [candle, setCandle] = useState<CandleDetail | undefined>(
    initialValue.candle,
  );
  const [customCandleBuilder, setCustomCandleBuilder] =
    useState<CustomCandleBuilder>(initialValue.customCandleBuilder);

  const fetchCandleById = async (id: string) => {
    const candleDetailResponse = await ConstructorApi.getCandleById(id);
    if (candleDetailResponse.data && !candleDetailResponse.error) {
      setCandle(candleDetailResponse.data);
      customCandleBuilder.setCandle(candleDetailResponse.data.candle);
    } else {
      console.error(
        'Ошибка при загрузке данных с сервера',
        candleDetailResponse.error,
      );
    }
  };

  const updateCustomCandleBuilder = () => {
    setCandle(undefined);
    setCustomCandleBuilder(new CustomCandleBuilder());
  };

  const contextValue = useMemo(
    () => ({
      candle,
      customCandleBuilder,
      fetchCandleById,
      setCustomCandleBuilder,
      updateCustomCandleBuilder,
    }),
    [candle, customCandleBuilder],
  );

  return (
    <CandleContext.Provider value={contextValue}>
      {children}
    </CandleContext.Provider>
  );
};

export const useCandleContext = () => useContext(CandleContext);
