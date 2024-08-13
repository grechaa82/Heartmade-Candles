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
import { calculateCustomCandlePrice } from '../helpers/CalculatePrice';
import { CustomCandle } from '../typesV2/constructor/CustomCandle';
import { CandleDetail } from '../typesV2/constructor/CandleDetail';
import { CustomCandleBuilder } from '../typesV2/constructor/CustomCandleBuilder';

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
  isLoadingCandlesByType: boolean;
};

const initialValue: IConstructorContext = {
  candlesByType: [],
  configuredCandles: [],
  totalPrice: 0,
  setConfiguredCandles: () => {},
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
  const [configuredCandles, setConfiguredCandles] = useState<
    ConfiguredCandleDetail[]
  >([]);
  const [totalPrice, setTotalPrice] = useState<number>(initialValue.totalPrice);
  const [customCandles, setCustomCandles] = useState<CustomCandle[]>(
    initialValue.customCandles,
  );
  const [isLoadingCandlesByType, setIsLoadingCandlesByType] = useState(
    initialValue.isLoadingCandlesByType,
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
    const delay = 30000;

    async function fetchData() {
      const candlesResponse = await ConstructorApi.getCandles();
      if (candlesResponse.data && !candlesResponse.error) {
        setCandlesByType(candlesResponse.data);
        setIsLoadingCandlesByType(false);
      } else {
        console.error(
          'Ошибка при загрузке данных с сервера',
          candlesResponse.error,
        );
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
        (detail) => detail.candle.id === customCandle.candle.id,
      );

      if (existingDetail) {
        const newCustomCandle = checkCustomCandleAgainstCandleDetail(
          customCandle,
          existingDetail,
        );
        newCustomCandles.push(newCustomCandle);
      } else {
        const candleDetailResponse = await ConstructorApi.getCandleById(
          customCandle.candle.id.toString(),
        );

        if (candleDetailResponse.data && !candleDetailResponse.error) {
          const newCustomCandle = checkCustomCandleAgainstCandleDetail(
            customCandle,
            candleDetailResponse.data,
          );
          newCustomCandles.push(newCustomCandle);
          candleDetails.push(candleDetailResponse.data);
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

  function checkCustomCandleAgainstCandleDetail(
    customCandle: CustomCandle,
    candleDetail: CandleDetail,
  ): CustomCandle {
    let errors: string[] = customCandle.errors;
    const customCandleBuilder = new CustomCandleBuilder();

    if (!candleDetail.candle) {
      errors.push('Необходимо указать свечу');
      return;
    } else customCandleBuilder.setCandle(customCandle.candle);

    if (
      customCandle.numberOfLayer &&
      !candleDetail.numberOfLayers.some(
        (layer) => layer.id === customCandle.numberOfLayer.id,
      )
    ) {
      errors.push('Выбранное количество слоев недоступно в списке');
    } else customCandleBuilder.setNumberOfLayer(customCandle.numberOfLayer);

    if (customCandle.layerColors) {
      customCandle.layerColors.forEach((color) => {
        if (
          !candleDetail.layerColors.some(
            (layerColor) => layerColor.id === color.id,
          )
        ) {
          errors.push(`Цвет слоя ${color.title} недоступен`);
        } else customCandleBuilder.setLayerColor(customCandle.layerColors);
      });
    }

    if (
      customCandle.decor &&
      !candleDetail.decors?.some((decor) => decor.id === customCandle.decor.id)
    ) {
      errors.push('Выбранный декор недоступен');
    } else customCandleBuilder.setDecor(customCandle.decor);

    if (
      customCandle.smell &&
      !candleDetail.smells?.some((smell) => smell.id === customCandle.smell.id)
    ) {
      errors.push('Выбранный запах недоступен');
    } else customCandleBuilder.setSmell(customCandle.smell);

    if (
      customCandle.wick &&
      !candleDetail.wicks?.some((wick) => wick.id === customCandle.wick?.id)
    ) {
      errors.push('Выбранный фитиль не доступен');
    } else customCandleBuilder.setWick(customCandle.wick);

    customCandleBuilder.setQuantity(customCandle.quantity);

    const customCandleBuildResult = customCandleBuilder.build();

    return {
      candle: customCandleBuildResult.customCandle.candle,
      numberOfLayer: customCandleBuildResult.customCandle.numberOfLayer,
      layerColors: customCandleBuildResult.customCandle.layerColors,
      wick: customCandleBuildResult.customCandle.wick,
      decor: customCandleBuildResult.customCandle.decor,
      smell: customCandleBuildResult.customCandle.smell,
      quantity: customCandleBuildResult.customCandle.quantity,
      filter: customCandleBuildResult.customCandle.filter,
      isValid: errors.length === 0,
      errors: errors,
    };
  }

  const contextValue = useMemo(
    () => ({
      candlesByType,
      configuredCandles,
      totalPrice,
      setConfiguredCandles: handleSetConfiguredCandles,
      customCandles,
      setCustomCandles: handleSetCustomCandles,
      isLoadingCandlesByType,
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
