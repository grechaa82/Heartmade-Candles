import { FC, useContext, useState, useEffect, useRef } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

import ListProductsCart from '../../modules/constructor/ListProductsCart';
import {
  ConfiguredCandleDetail,
  validateConfiguredCandleDetail,
} from '../../typesV2/constructor/ConfiguredCandleDetail';
import { ImageProduct } from '../../typesV2/shared/BaseProduct';
import { OrderItemFilter } from '../../typesV2/shared/OrderItemFilter';
import CandleSelectionPanel from '../../modules/constructor/CandleSelectionPanel';
import CandleSelectionPanelSkeleton from '../../modules/constructor/CandleSelectionPanelSkeleton';
import { calculatePrice } from '../../helpers/CalculatePrice';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import ImageSlider from '../../components/constructor/ImageSlider';
import { CandleDetailFilterRequest } from '../../typesV2/order/CandleDetailFilterRequest';
import { CandleDetailFilterBasketRequest } from '../../typesV2/order/CandleDetailFilterBasketRequest';
import ListProductsCartSkeleton from '../../modules/constructor/ListProductsCartSkeleton';
import TutorialBlock from '../../modules/constructor/TutorialBlock';

import { ConstructorApi } from '../../services/ConstructorApi';
import { BasketApi } from '../../services/BasketApi';

import Style from './ConstructorPage.module.css';
import {
  ConstructorProvider,
  useConstructorContext,
} from '../../contexts/ConstructorContext';
import { CandleProvider, useCandleContext } from '../../contexts/CandleContext';
import CandleForm from '../../modules/constructor/CandleForm';

const ConstructorPage: FC = () => {
  const {
    candlesByType: candleTypeWithCandles,
    configuredCandles,
    totalPrice,
    setConfiguredCandles,
  } = useConstructorContext();
  const {
    candle,
    configuredCandle,
    priceConfiguredCandle,
    setCandle,
    setConfiguredCandle,
    fetchCandleById,
  } = useCandleContext();

  const [isConfiguredCandleDetailLoading, setIsConfiguredCandleDetailLoading] =
    useState(true);

  const location = useLocation();
  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const blockCandleFormRef = useRef<HTMLDivElement>(null);

  function handleHideCandleForm() {
    setErrorMessage([]);
    setCandle(undefined);
    setConfiguredCandle(undefined);
  }

  const addConfiguredCandleDetailToListProductsCart = (
    configuredCandleDetailToAdd: ConfiguredCandleDetail,
  ): void => {
    const validCandleDetail: string[] = checkConfiguredCandleDetail(
      configuredCandleDetailToAdd,
    );
    if (validCandleDetail.length > 0) {
      setErrorMessage((prev) => [...prev, ...validCandleDetail.flat()]);
      return;
    }
    addQueryString(convertToCandleString([configuredCandleDetailToAdd]));

    setConfiguredCandles([...configuredCandles, configuredCandleDetailToAdd]);
    handleHideCandleForm();
  };

  function convertToCandleString(value: ConfiguredCandleDetail[]): string {
    return value.map((detail) => detail.getFilter()).join('.');
  }

  const addQueryString = (queryString: string) => {
    const urlSearchParams = new URLSearchParams(window.location.search);
    let currentQueryString = urlSearchParams.toString();
    if (!currentQueryString) {
      currentQueryString = queryString;
    } else {
      const lastCharacter = currentQueryString.slice(-1);
      currentQueryString += (lastCharacter !== '.' ? '.' : '') + queryString;
    }
    const newUrlSearchParams = new URLSearchParams(`?${currentQueryString}`);
    const newQueryString = newUrlSearchParams.toString().replace('=', '');
    navigate(`?${newQueryString}`);
  };

  const checkConfiguredCandleDetail = (
    configuredCandleDetail: ConfiguredCandleDetail,
  ): string[] => {
    const errorMessageInConfiguredCandleDetail: string[] = [];
    const errorMessageParts: string[] = [];
    if (!configuredCandleDetail.numberOfLayer) {
      errorMessageParts.push('количество слоев');
    }
    if (!configuredCandleDetail.layerColors) {
      errorMessageParts.push('восковые слои');
    }
    if (!configuredCandleDetail.wick) {
      errorMessageParts.push('фитиль');
    }
    if (errorMessageParts.length > 0) {
      errorMessageInConfiguredCandleDetail.push(
        `Не выбрано следующее обязательное поле(я): ${errorMessageParts.join(
          ', ',
        )}`,
      );
    }
    if (
      configuredCandleDetail.numberOfLayer?.number !==
      configuredCandleDetail.layerColors?.length
    ) {
      errorMessageInConfiguredCandleDetail.push(
        'Количество слоев не совпадает с количеством выбранных цветовых слоев',
      );
    }
    return errorMessageInConfiguredCandleDetail;
  };

  useEffect(() => {
    setIsConfiguredCandleDetailLoading(true);
    let validConfiguredCandleDetail: ConfiguredCandleDetail[] = [];
    let invalidConfiguredCandleDetail: ConfiguredCandleDetail[] = [];

    let allErrorMessages: string[] = [];

    const searchParams = new URLSearchParams(location.search);
    const filters = decodeURI(searchParams.toString().replace(/=$/, ''));

    if (filters.length < OrderItemFilter.MIN_LENGTH_FILTER) {
      setIsConfiguredCandleDetailLoading(false);
      return;
    }

    /* 
      Добавить проверку что filters.split('.') можно спарсить в OrderItemFilter
      const orderItemFilters: OrderItemFilter[] = filters.tryParseToOrderItemFilter();
    */
    const orderItemFilters: OrderItemFilter[] = filters
      .split('.')
      .map(OrderItemFilter.parseToOrderItemFilter);

    getValidConfiguredCandleDetail(orderItemFilters).then((result) => {
      const { candleDetails, errorMessages } = result;

      validConfiguredCandleDetail.push(...candleDetails);

      allErrorMessages = [...allErrorMessages, ...errorMessages];

      if (allErrorMessages.length) {
        setErrorMessage(allErrorMessages);
      }

      setConfiguredCandles([...candleDetails]);

      setIsConfiguredCandleDetailLoading(false);
    });
  }, [location.search]);

  async function getValidConfiguredCandleDetail(
    orderItemFilters: OrderItemFilter[],
  ) {
    let validConfiguredCandleDetail: ConfiguredCandleDetail[] = [];
    let allErrorMessages: string[] = [];

    for (const filter of orderItemFilters) {
      const candleDetailResponse = await ConstructorApi.getCandleById(
        filter.candleId.toString(),
      );
      if (candleDetailResponse.data && !candleDetailResponse.error) {
        const validationResult = validateConfiguredCandleDetail(
          candleDetailResponse.data,
          filter,
        );

        if (Array.isArray(validationResult)) {
          allErrorMessages = [...allErrorMessages, ...validationResult];
        } else {
          validConfiguredCandleDetail.push(validationResult);
        }
      } else {
        setErrorMessage([
          ...errorMessage,
          candleDetailResponse.error as string,
        ]);
      }
    }

    return {
      candleDetails: validConfiguredCandleDetail,
      errorMessages: allErrorMessages,
    };
  }

  const handleChangeConfiguredCandle = (value: ConfiguredCandleDetail[]) => {
    navigate('');
    addQueryString(convertToCandleString(value));
    setConfiguredCandles(value);
  };

  const handleSelectCandle = (candle: ImageProduct) => {
    if (blockCandleFormRef.current) {
      blockCandleFormRef.current.scrollTop = 0;
    }
    fetchCandleById(candle.id.toString());
  };

  const calculatePriceConfiguredCandleDetail = (
    configuredCandleDetail: ConfiguredCandleDetail,
  ): number => {
    const priceConfiguredCandleDetail = Math.round(
      calculatePrice(configuredCandleDetail),
    );
    return priceConfiguredCandleDetail;
  };

  const handleOnCreateBasket = async () => {
    if (candle !== undefined) {
      setErrorMessage([
        ...errorMessage,
        'Пожалуйста закончите настройку свечи',
      ]);
    } else if (configuredCandles.length <= 0) {
      setErrorMessage([
        ...errorMessage,
        'В корзине пока пусто, добавьте свечи',
      ]);
    } else if (configuredCandles.length > 0) {
      let candleDetailFilterBasketRequest: CandleDetailFilterBasketRequest = {
        candleDetailFilterRequests: [],
        configuredCandleFiltersString: convertToCandleString(configuredCandles),
      };

      configuredCandles.forEach((configuredCandle) => {
        const filterRequest: CandleDetailFilterRequest = {
          candleId: configuredCandle.candle.id,
          decorId: configuredCandle.decor ? configuredCandle.decor.id : 0,
          numberOfLayerId: configuredCandle.numberOfLayer!.id,
          layerColorIds: configuredCandle.layerColors!.map(
            (layerColor) => layerColor.id,
          ),
          smellId: configuredCandle.smell ? configuredCandle.smell.id : 0,
          wickId: configuredCandle.wick!.id,
          quantity: configuredCandle.quantity,
          filterString: configuredCandle.getFilter(),
        };

        candleDetailFilterBasketRequest.candleDetailFilterRequests.push(
          filterRequest,
        );
      });

      var basketIdResponse = await BasketApi.createBasket(
        candleDetailFilterBasketRequest,
      );

      if (basketIdResponse.data && !basketIdResponse.error) {
        navigate(`/baskets/${basketIdResponse.data}`);
      } else {
        setErrorMessage([...errorMessage, basketIdResponse.error as string]);
      }
    } else {
      setErrorMessage([
        ...errorMessage,
        'Что-то пошло не так, попробуйте повторить действие',
      ]);
    }
  };

  const handleOnSelectConfiguredCandle = (
    configuredCandle: ConfiguredCandleDetail,
  ) => {
    fetchCandleById(configuredCandle.candle.id.toString());
    setConfiguredCandle(configuredCandle);

    console.log(
      'fass handleOnSelectConfiguredCandle',
      configuredCandle,
      candle,
    );
  };

  return (
    <div className={Style.container}>
      <ListErrorPopUp messages={errorMessage} />
      <div
        className={`${Style.leftPanel} ${
          configuredCandles.length === 0 ? Style.noElements : ''
        }`}
      >
        {isConfiguredCandleDetailLoading ? (
          <ListProductsCartSkeleton />
        ) : (
          <ListProductsCart
            products={configuredCandles}
            onChangeCandleDetailWithQuantity={handleChangeConfiguredCandle}
            price={totalPrice}
            onCreateBasket={handleOnCreateBasket}
            buttonState={
              candle !== undefined || configuredCandles.length <= 0
                ? 'invalid'
                : 'valid'
            }
            onSelect={handleOnSelectConfiguredCandle}
          />
        )}
      </div>
      <div className={Style.imagePanel}>
        {candle ? (
          <ImageSlider images={candle.candle.images} />
        ) : (
          <TutorialBlock />
        )}
      </div>
      <div className={Style.rightPanel} ref={blockCandleFormRef}>
        {candle ? (
          <CandleForm
            candleDetail={candle}
            configuredCandleDetail={configuredCandle}
            addCandleDetail={addConfiguredCandleDetailToListProductsCart}
            calculatePriceCandleDetail={calculatePriceConfiguredCandleDetail}
            hideCandleForm={handleHideCandleForm}
          />
        ) : !candleTypeWithCandles ? (
          <CandleSelectionPanelSkeleton />
        ) : (
          <CandleSelectionPanel
            data={candleTypeWithCandles}
            onSelectCandle={handleSelectCandle}
          />
        )}
      </div>
    </div>
  );
};

export default ConstructorPage;
