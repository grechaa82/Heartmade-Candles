import { FC, useState, useEffect } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';

import ListProductsCart from '../../modules/constructor/ListProductsCart';
import ListProductsCartSkeleton from '../../modules/constructor/ListProductsCartSkeleton';
import CandleForm from '../../modules/constructor/CandleForm';
import { CandleDetail, ConfiguredCandleDetail } from '../../typesV2/BaseProduct';
import { Candle, ImageProduct } from '../../typesV2/Candle';
import { OrderItemFilter, validateOrderItemFilter } from '../../typesV2/OrderItemFilter';
import CandleSelectionPanel from '../../modules/constructor/CandleSelectionPanel';
import CandleSelectionPanelSkeleton from '../../modules/constructor/CandleSelectionPanelSkeleton';
import { CandleTypeWithCandles } from '../../typesV2/CandleTypeWithCandles';
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
    async function fetchData() {
      try {
        const candles = await ConstructorApi.getCandles();
        setCandleTypeWithCandles(candles);
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
      }
    }

    fetchData();
  }, []);

  useEffect(() => {
    setIsConfiguredCandleDetailLoading(true);

    let validConfiguredCandleDetail: ConfiguredCandleDetail[] = [];
    let invalidConfiguredCandleDetail: ConfiguredCandleDetail[] = [];

    let allErrorMessages: string[] = [];

    const searchParams = new URLSearchParams(location.search);
    const orderItemFilters: OrderItemFilter[] = searchParams
      .toString()
      .replace(/=$/, '')
      .split('.')
      .map(decodeURIComponent)
      .map(OrderItemFilter.parseToOrderItemFilter);

    getValidConfiguredCandleDetail(orderItemFilters).then((result) => {
      const { candleDetails, errorMessages } = result;

      validConfiguredCandleDetail.push(...candleDetails);

      setConfiguredCandleDetails(candleDetails);
      let newTotalPrice = candleDetails.reduce(
        (sum, detail) => sum + calculatePrice(detail) * detail.quantity,
        0,
      );
      setTotalPrice(newTotalPrice);

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

  function generateErrorMessage(candleTitle: string, property: string, id: string) {
    return `У свечи '${candleTitle}' нет ${property}: ${id}`;
  }

  function validateConfiguredCandleDetail(
    candleDetail: CandleDetail,
    orderItemFilter: OrderItemFilter,
  ): ConfiguredCandleDetail | string[] {
    const errorMessageInvalidCandleComponents: string[] = [];
    let isValid = true;

    if (
      candleDetail.decors &&
      orderItemFilter.decorId &&
      !candleDetail.decors.some((d) => d.id === orderItemFilter.decorId)
    ) {
      errorMessageInvalidCandleComponents.push(
        generateErrorMessage(
          candleDetail.candle.title,
          'декора',
          orderItemFilter.decorId.toString(),
        ),
      );
      isValid = false;
    }

    if (
      candleDetail.layerColors &&
      orderItemFilter.layerColorIds &&
      !orderItemFilter.layerColorIds.every((layerColorId) =>
        candleDetail.layerColors?.some((layerColor) => layerColor.id === layerColorId),
      )
    ) {
      errorMessageInvalidCandleComponents.push(
        generateErrorMessage(
          candleDetail.candle.title,
          'слоя',
          orderItemFilter.layerColorIds.join(','),
        ),
      );
      isValid = false;
    }

    if (
      candleDetail.numberOfLayers &&
      orderItemFilter.numberOfLayerId &&
      !candleDetail.numberOfLayers.some((n) => n.id === orderItemFilter.numberOfLayerId)
    ) {
      errorMessageInvalidCandleComponents.push(
        generateErrorMessage(
          candleDetail.candle.title,
          'количества слоев',
          orderItemFilter.numberOfLayerId.toString(),
        ),
      );
      isValid = false;
    }

    if (
      candleDetail.smells &&
      orderItemFilter.smellId &&
      !candleDetail.smells.some((s) => s.id === orderItemFilter.smellId)
    ) {
      errorMessageInvalidCandleComponents.push(
        generateErrorMessage(
          candleDetail.candle.title,
          'запаха',
          orderItemFilter.smellId.toString(),
        ),
      );
      isValid = false;
    }

    if (
      candleDetail.wicks &&
      orderItemFilter.wickId &&
      !candleDetail.wicks.some((w) => w.id === orderItemFilter.wickId)
    ) {
      errorMessageInvalidCandleComponents.push(
        generateErrorMessage(
          candleDetail.candle.title,
          'фитиля',
          orderItemFilter.wickId.toString(),
        ),
      );
      isValid = false;
    }

    if (!isValid) {
      return errorMessageInvalidCandleComponents;
    } else {
      const newConfiguredCandleDetail = createConfiguredCandleDetail(candleDetail, orderItemFilter);
      return newConfiguredCandleDetail ? newConfiguredCandleDetail : ['Что-то пошло не так'];
    }
  }

  function createConfiguredCandleDetail(
    candleDetail: CandleDetail,
    orderItemFilter: OrderItemFilter,
  ): ConfiguredCandleDetail | undefined {
    const decor = candleDetail.decors?.find((d) => d.id === orderItemFilter.decorId);

    const layerColors = candleDetail.layerColors.filter((layerColor) =>
      orderItemFilter.layerColorIds.includes(layerColor.id),
    );

    const numberOfLayer = candleDetail.numberOfLayers?.find(
      (numberOfLayer) => numberOfLayer.id === orderItemFilter.numberOfLayerId,
    );

    const smell = candleDetail.smells?.find((smell) => smell.id === orderItemFilter.smellId);

    const wick = candleDetail.wicks?.find((wick) => wick.id === orderItemFilter.wickId);

    if (numberOfLayer && wick) {
      const configuredCandleDetail = new ConfiguredCandleDetail(
        candleDetail.candle,
        orderItemFilter.quantity,
        numberOfLayer,
        layerColors,
        wick,
        decor,
        smell,
      );

      return configuredCandleDetail;
    }

    return undefined;
  }

  // */

  const handleChangeConfiguredCandleDetail = (value: ConfiguredCandleDetail[]) => {
    navigate('');
    addQueryString(convertToCandleString(value));
    setConfiguredCandleDetails(value);
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
