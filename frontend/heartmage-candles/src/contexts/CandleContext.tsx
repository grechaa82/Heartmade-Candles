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
import { ConstructorApi } from '../services/ConstructorApi';
import { ConfiguredCandleDetail } from '../typesV2/constructor/ConfiguredCandleDetail';
import { CustomCandle } from '../typesV2/constructor/CustomCandle';
import { CustomCandleBuilder } from '../typesV2/constructor/CustomCandleBuilder';

export interface CandleProviderProps {
  children?: ReactNode;
}

type ICandleContext = {
  candle: CandleDetail | undefined;
  configuredCandle: ConfiguredCandleDetail | undefined;
  priceConfiguredCandle: number;
  setCandle: (candleDetail: CandleDetail) => void;
  setConfiguredCandle: (configuredCandle: ConfiguredCandleDetail) => void;
  fetchCandleById: (id: string) => Promise<void>;
  customCandle: CustomCandle;
  customCandleBuilder: CustomCandleBuilder;
  updateCustomCandle: (
    updater: (builder: CustomCandleBuilder) => CustomCandleBuilder,
  ) => void;
};

const initialValue: ICandleContext = {
  candle: undefined,
  configuredCandle: undefined,
  priceConfiguredCandle: 0,
  setCandle: () => {},
  setConfiguredCandle: () => {},
  fetchCandleById: async () => {},
  customCandle: undefined,
  customCandleBuilder: new CustomCandleBuilder(),
  updateCustomCandle: () => {},
};

const CandleContext = createContext<ICandleContext>(initialValue);

export const CandleProvider: FC<CandleProviderProps> = ({ children }) => {
  const [candle, setCandle] = useState<CandleDetail | undefined>(
    initialValue.candle,
  );
  const [configuredCandle, setConfiguredCandle] = useState<
    ConfiguredCandleDetail | undefined
  >(initialValue.configuredCandle);
  const [priceConfiguredCandle, setPriceConfiguredCandle] = useState<number>(
    initialValue.priceConfiguredCandle,
  );
  const [customCandle, setCustomCandle] = useState<CustomCandle>(
    initialValue.customCandle,
  );
  const [customCandleBuilder, setCustomCandleBuilder] =
    useState<CustomCandleBuilder>(initialValue.customCandleBuilder);

  console.log('CandleProvider configuredCandle', configuredCandle);

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

  const handleSetConfiguredCandle = (
    configuredCandle: ConfiguredCandleDetail,
  ) => {
    setConfiguredCandle(configuredCandle);
    setCandle(undefined);
  };

  const updateCustomCandle = (
    builderUpdater: (builder: CustomCandleBuilder) => CustomCandleBuilder,
  ) => {
    setCustomCandleBuilder((prevBuilder) => {
      const updatedBuilder = builderUpdater(prevBuilder);
      setCustomCandle(updatedBuilder.getCustomCandle());
      return updatedBuilder;
    });
  };

  const contextValue = useMemo(
    () => ({
      candle,
      configuredCandle,
      priceConfiguredCandle,
      setCandle,
      setConfiguredCandle: handleSetConfiguredCandle,
      fetchCandleById,
      customCandle,
      customCandleBuilder,
      updateCustomCandle,
    }),
    [
      candle,
      configuredCandle,
      customCandle,
      customCandleBuilder,
      customCandleBuilder.customCandle,
    ],
  );

  console.log(candle);

  return (
    <CandleContext.Provider value={contextValue}>
      {children}
    </CandleContext.Provider>
  );
};

export const useCandleContext = () => useContext(CandleContext);
