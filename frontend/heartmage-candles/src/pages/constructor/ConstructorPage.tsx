import { FC, useState, useEffect } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';

import ListProductsCart from '../../modules/constructor/ListProductsCart';
import ListProductsCartSkeleton from '../../modules/constructor/ListProductsCartSkeleton';
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
import IconArrowLeftLarge from '../../UI/IconArrowLeftLarge';
import { calculatePrice } from '../../helpers/CalculatePrice';
import ListErrorPopUp from '../../modules/constructor/ListErrorPopUp';
import ImageSlider from '../../components/constructor/ImageSlider';

import Style from './ConstructorPage.module.css';

import { ConstructorApi } from '../../services/ConstructorApi';

const ConstructorPage: FC = () => {
  const [candleDetail, setCandleDetail] = useState<CandleDetail>();
  const [configuredCandleDetails, setConfiguredCandleDetails] = useState<ConfiguredCandleDetail[]>(
    [],
  );
  const [isConfiguredCandleDetailLoading, setIsConfiguredCandleDetailLoading] = useState(true);
  const [candleTypeWithCandles, setCandleTypeWithCandles] = useState<CandleTypeWithCandles[]>();
  const [priceConfiguredCandleDetail, setPriceConfiguredCandleDetail] = useState<number>(0);
  const [totalPrice, setTotalPrice] = useState<number>(0);
  const location = useLocation();
  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  async function showCandleForm(candleId: number) {
    try {
      const candleDetail = await ConstructorApi.getCandleById(candleId.toString());
      setCandleDetail(candleDetail);
    } catch (error) {
      console.error('Произошла ошибка при загрузке данных:', error);
    }
  }

  function hideCandleForm() {
    setErrorMessage([]);
    setCandleDetail(undefined);
  }

  const addConfiguredCandleDetailToListProductsCart = (
    configuredCandleDetailToAdd: ConfiguredCandleDetail,
  ): void => {
    const validCandleDetail: string[] = checkConfiguredCandleDetail(configuredCandleDetailToAdd);
    if (validCandleDetail.length > 0) {
      setErrorMessage((prev) => [...prev, ...validCandleDetail.flat()]);
      return;
    }
    addQueryString(convertToCandleString([configuredCandleDetailToAdd]));
    setConfiguredCandleDetails((prev) => [...prev, configuredCandleDetailToAdd]);
    hideCandleForm();
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
        `Не выбрано следующее обязательное поле(я): ${errorMessageParts.join(', ')}`,
      );
    }
    if (
      configuredCandleDetail.numberOfLayer?.number !== configuredCandleDetail.layerColors?.length
    ) {
      errorMessageInConfiguredCandleDetail.push(
        'Количество слоев не совпадает с количеством выбранных цветовых слоев',
      );
    }
    return errorMessageInConfiguredCandleDetail;
  };

  useEffect(() => {
    setIsConfiguredCandleDetailLoading(true);
    async function fetchData() {
      try {
        const candles = await ConstructorApi.getCandles();
        setCandleTypeWithCandles(candles);
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
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

  async function getValidConfiguredCandleDetail(orderItemFilters: OrderItemFilter[]) {
    let validConfiguredCandleDetail: ConfiguredCandleDetail[] = [];
    let allErrorMessages: string[] = [];

    for (const filter of orderItemFilters) {
      const candleDetail = await ConstructorApi.getCandleById(filter.candleId.toString());
      const validationResult = validateConfiguredCandleDetail(candleDetail, filter);

      if (Array.isArray(validationResult)) {
        allErrorMessages = [...allErrorMessages, ...validationResult];
      } else {
        validConfiguredCandleDetail.push(validationResult);
      }
    }

    return { candleDetails: validConfiguredCandleDetail, errorMessages: allErrorMessages };
  }

  const handleChangeConfiguredCandleDetail = (value: ConfiguredCandleDetail[]) => {
    navigate('');
    addQueryString(convertToCandleString(value));
    setConfiguredCandleDetails(value);
    setNewTotalPrice(value);
  };

  const handleSelectCandle = (candle: ImageProduct) => {
    showCandleForm(candle.id);
    setPriceConfiguredCandleDetail(0);
  };

  const calculatePriceConfiguredCandleDetail = (configuredCandleDetail: ConfiguredCandleDetail) => {
    setPriceConfiguredCandleDetail(calculatePrice(configuredCandleDetail));
  };

  const getCreateOrderLink = (): string => {
    var configuredCandlesString = location.search;
    return `/orders${configuredCandlesString}`;
  };

  function setNewTotalPrice(configuredCandleDetails: ConfiguredCandleDetail[]) {
    let newTotalPrice = configuredCandleDetails.reduce(
      (sum, detail) => sum + calculatePrice(detail) * detail.quantity,
      0,
    );
    setTotalPrice(newTotalPrice);
  }

  return (
    <div className={Style.container}>
      <div className={Style.popUpNotification}>
        <ListErrorPopUp messages={errorMessage} />
      </div>
      <div className={Style.leftPanel}>
        {isConfiguredCandleDetailLoading ? (
          <ListProductsCartSkeleton />
        ) : (
          <ListProductsCart
            products={configuredCandleDetails}
            onChangeCandleDetailWithQuantity={handleChangeConfiguredCandleDetail}
          />
        )}
      </div>
      <div className={Style.imagePanel}>
        {candleDetail && (
          <>
            <ImageSlider images={candleDetail.candle.images} />
            <div className={Style.hideCandleForm}>
              <button onClick={() => hideCandleForm()}>
                <IconArrowLeftLarge color="#777" />
              </button>
            </div>
            <div className={Style.priceCandle}>
              <span>{priceConfiguredCandleDetail} р</span>
            </div>
          </>
        )}
        <div className={Style.orderInfo}>
          <div className={Style.orderBtn}>
            <Link to={getCreateOrderLink()}>Заказать</Link>
          </div>
          <div className={Style.totalPrice}>
            <span className={Style.title}>Итого </span>
            <span className={Style.price}>{totalPrice} р</span>
          </div>
        </div>
      </div>
      <div className={Style.rightPanel}>
        {candleDetail ? (
          <CandleForm
            candleDetail={candleDetail}
            addCandleDetail={addConfiguredCandleDetailToListProductsCart}
            calculatePriceCandleDetail={calculatePriceConfiguredCandleDetail}
          />
        ) : !candleTypeWithCandles ? (
          <CandleSelectionPanelSkeleton />
        ) : (
          <CandleSelectionPanel data={candleTypeWithCandles} onSelectCandle={handleSelectCandle} />
        )}
      </div>
    </div>
  );
};

export default ConstructorPage;
