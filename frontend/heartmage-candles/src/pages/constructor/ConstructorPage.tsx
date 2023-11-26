import { FC, useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

import ListProductsCart from '../../modules/constructor/ListProductsCart';
import CandleForm from '../../modules/constructor/CandleForm';
import { CandleDetail } from '../../typesV2/constructor/CandleDetail';
import {
  ConfiguredCandleDetail,
  validateConfiguredCandleDetail,
} from '../../typesV2/constructor/ConfiguredCandleDetail';
import { ImageProduct } from '../../typesV2/shared/BaseProduct';
import { OrderItemFilter } from '../../typesV2/shared/OrderItemFilter';
import CandleSelectionPanel from '../../modules/constructor/CandleSelectionPanel';
import CandleSelectionPanelSkeleton from '../../modules/constructor/CandleSelectionPanelSkeleton';
import { CandleTypeWithCandles } from '../../typesV2/constructor/CandleTypeWithCandles';
import { calculatePrice } from '../../helpers/CalculatePrice';
import ListErrorPopUp from '../../modules/constructor/ListErrorPopUp';
import ImageSlider from '../../components/constructor/ImageSlider';
import { CandleDetailFilterRequest } from '../../typesV2/order/CandleDetailFilterRequest';
import { CandleDetailFilterBasketRequest } from '../../typesV2/order/CandleDetailFilterBasketRequest';
import ConstructorBanner1 from '../../assets/constructor-banner-1.png';

import { ConstructorApi } from '../../services/ConstructorApi';
import { BasketApi } from '../../services/BasketApi';

import Style from './ConstructorPage.module.css';

const ConstructorPage: FC = () => {
  const [candleDetail, setCandleDetail] = useState<CandleDetail>();
  const [configuredCandleDetails, setConfiguredCandleDetails] = useState<
    ConfiguredCandleDetail[]
  >([]);
  const [isConfiguredCandleDetailLoading, setIsConfiguredCandleDetailLoading] =
    useState(true);
  const [candleTypeWithCandles, setCandleTypeWithCandles] =
    useState<CandleTypeWithCandles[]>();
  const [priceConfiguredCandleDetail, setPriceConfiguredCandleDetail] =
    useState<number>(0);
  const [totalPrice, setTotalPrice] = useState<number>(0);
  const location = useLocation();
  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  async function showCandleForm(candleId: number) {
    const candleDetailResponse = await ConstructorApi.getCandleById(
      candleId.toString()
    );
    if (candleDetailResponse.data && !candleDetailResponse.error) {
      setCandleDetail(candleDetailResponse.data);
    } else {
      setErrorMessage([...errorMessage, candleDetailResponse.error as string]);
    }
  }

  function handleHideCandleForm() {
    setErrorMessage([]);
    setCandleDetail(undefined);
  }

  const addConfiguredCandleDetailToListProductsCart = (
    configuredCandleDetailToAdd: ConfiguredCandleDetail
  ): void => {
    const validCandleDetail: string[] = checkConfiguredCandleDetail(
      configuredCandleDetailToAdd
    );
    if (validCandleDetail.length > 0) {
      setErrorMessage((prev) => [...prev, ...validCandleDetail.flat()]);
      return;
    }
    addQueryString(convertToCandleString([configuredCandleDetailToAdd]));
    setConfiguredCandleDetails((prev) => [
      ...prev,
      configuredCandleDetailToAdd,
    ]);
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
    configuredCandleDetail: ConfiguredCandleDetail
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
          ', '
        )}`
      );
    }
    if (
      configuredCandleDetail.numberOfLayer?.number !==
      configuredCandleDetail.layerColors?.length
    ) {
      errorMessageInConfiguredCandleDetail.push(
        'Количество слоев не совпадает с количеством выбранных цветовых слоев'
      );
    }
    return errorMessageInConfiguredCandleDetail;
  };

  useEffect(() => {
    setIsConfiguredCandleDetailLoading(true);
    async function fetchData() {
      const candlesResponse = await ConstructorApi.getCandles();
      if (candlesResponse.data && !candlesResponse.error) {
        setCandleTypeWithCandles(candlesResponse.data);
      } else {
        setErrorMessage([...errorMessage, candlesResponse.error as string]);
      }
    }

    fetchData();
    setIsConfiguredCandleDetailLoading(false);
  }, []);

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

      setConfiguredCandleDetails(candleDetails);

      setNewTotalPrice(candleDetails);

      allErrorMessages = [...allErrorMessages, ...errorMessages];

      if (allErrorMessages.length) {
        setErrorMessage(allErrorMessages);
      }

      setConfiguredCandleDetails(validConfiguredCandleDetail);

      setIsConfiguredCandleDetailLoading(false);
    });
  }, [location.search]);

  async function getValidConfiguredCandleDetail(
    orderItemFilters: OrderItemFilter[]
  ) {
    let validConfiguredCandleDetail: ConfiguredCandleDetail[] = [];
    let allErrorMessages: string[] = [];

    for (const filter of orderItemFilters) {
      const candleDetailResponse = await ConstructorApi.getCandleById(
        filter.candleId.toString()
      );
      if (candleDetailResponse.data && !candleDetailResponse.error) {
        const validationResult = validateConfiguredCandleDetail(
          candleDetailResponse.data,
          filter
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

  const handleChangeConfiguredCandleDetail = (
    value: ConfiguredCandleDetail[]
  ) => {
    navigate('');
    addQueryString(convertToCandleString(value));
    setConfiguredCandleDetails(value);
    setNewTotalPrice(value);
  };

  const handleSelectCandle = (candle: ImageProduct) => {
    showCandleForm(candle.id);
    setPriceConfiguredCandleDetail(0);
  };

  const calculatePriceConfiguredCandleDetail = (
    configuredCandleDetail: ConfiguredCandleDetail
  ): number => {
    const priceConfiguredCandleDetail = Math.round(
      calculatePrice(configuredCandleDetail)
    );
    setPriceConfiguredCandleDetail(priceConfiguredCandleDetail);
    return priceConfiguredCandleDetail;
  };

  const handleOnCreateBasket = async () => {
    if (configuredCandleDetails.length > 0) {
      let candleDetailFilterBasketRequest: CandleDetailFilterBasketRequest = {
        candleDetailFilterRequests: [],
        configuredCandleFiltersString: convertToCandleString(
          configuredCandleDetails
        ),
      };

      configuredCandleDetails.forEach((configuredCandleDetail) => {
        const filterRequest: CandleDetailFilterRequest = {
          candleId: configuredCandleDetail.candle.id,
          decorId: configuredCandleDetail.decor
            ? configuredCandleDetail.decor.id
            : 0,
          numberOfLayerId: configuredCandleDetail.numberOfLayer!.id,
          layerColorIds: configuredCandleDetail.layerColors!.map(
            (layerColor) => layerColor.id
          ),
          smellId: configuredCandleDetail.smell
            ? configuredCandleDetail.smell.id
            : 0,
          wickId: configuredCandleDetail.wick!.id,
          quantity: configuredCandleDetail.quantity,
          filterString: configuredCandleDetail.getFilter(),
        };

        candleDetailFilterBasketRequest.candleDetailFilterRequests.push(
          filterRequest
        );
      });

      var basketIdResponse = await BasketApi.createBasket(
        candleDetailFilterBasketRequest
      );

      if (basketIdResponse.data && !basketIdResponse.error) {
        navigate(`/baskets/${basketIdResponse.data}`);
      } else {
        setErrorMessage([...errorMessage, basketIdResponse.error as string]);
      }
    } else {
      setErrorMessage([
        ...errorMessage,
        'В корзине пока пусто, добавьте свечи',
      ]);
    }
  };

  function setNewTotalPrice(configuredCandleDetails: ConfiguredCandleDetail[]) {
    let newTotalPrice = 0;
    for (const configuredCandleDetail of configuredCandleDetails) {
      newTotalPrice += Math.round(
        calculatePrice(configuredCandleDetail) * configuredCandleDetail.quantity
      );
    }
    setTotalPrice(newTotalPrice);
  }

  return (
    <div className={Style.container}>
      <div className={Style.popUpNotification}>
        <ListErrorPopUp messages={errorMessage} />
      </div>
      <div className={Style.leftPanel}>
        <ListProductsCart
          products={configuredCandleDetails}
          onChangeCandleDetailWithQuantity={handleChangeConfiguredCandleDetail}
          price={totalPrice}
          onCreateBasket={handleOnCreateBasket}
        />
      </div>
      <div className={Style.imagePanel}>
        {candleDetail ? (
          <ImageSlider images={candleDetail.candle.images} />
        ) : (
          <div className={Style.imageBlock}>
            <img
              alt="Explanation of how the constructor works"
              src={ConstructorBanner1}
            />
          </div>
        )}
      </div>
      <div className={Style.rightPanel}>
        {candleDetail ? (
          <CandleForm
            candleDetail={candleDetail}
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
