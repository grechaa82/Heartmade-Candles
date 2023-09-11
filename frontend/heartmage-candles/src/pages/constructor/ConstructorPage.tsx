import { FC, useState, useEffect } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';

import ListProductsCart from '../../modules/constructor/ListProductsCart';
import ListProductsCartSkeleton from '../../modules/constructor/ListProductsCartSkeleton';
import CandleForm from '../../modules/constructor/CandleForm';
import {
  CandleDetails,
  CandleDetailWithQuantity,
  ImageProduct,
  CandleDetailIdsWithQuantity,
} from '../../typesV2/BaseProduct';
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
  const [candleDetail, setCandleDetail] = useState<CandleDetails>();
  const [candleDetailWithQuantity, setCandleDetailWithQuantity] = useState<
    CandleDetailWithQuantity[]
  >([]);
  const [isCandleDetailWithQuantityLoading, setIsCandleDetailWithQuantityLoading] = useState(true);
  const [candleTypeWithCandles, setCandleTypeWithCandles] = useState<CandleTypeWithCandles[]>();
  const [price, setPrice] = useState<number>(0);
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

  const addCandleToListProductsCart = (candleDetail: CandleDetails): void => {
    const validCandleDetail: string[] = checkCandleDetail(candleDetail);
    if (validCandleDetail.length > 0) {
      setErrorMessage((prev) => [...prev, ...validCandleDetail.flat()]);
      return;
    }

    const newCandleDetailWithQuantity: CandleDetailWithQuantity = {
      ...candleDetail,
      quantity: 1,
    };

    addQueryString(convertToCandleString(newCandleDetailWithQuantity));

    setCandleDetailWithQuantity((prevCandleDetailWithQuantity) => [
      ...prevCandleDetailWithQuantity,
      newCandleDetailWithQuantity,
    ]);
    setErrorMessage([]);
    setCandleDetail(undefined);
  };

  function convertToCandleString(candleDetail: CandleDetailWithQuantity): string {
    const { candle, decors, layerColors, numberOfLayers, smells, wicks, quantity } = candleDetail;

    const candleStr = `c-${candle.id}`;
    const numberOfLayersStr = numberOfLayers
      ? `n-${numberOfLayers.map((item) => item.number).join('_')}`
      : '';
    const layerColorsStr = layerColors ? `l-${layerColors.map((item) => item.id).join('_')}` : '';
    const decorStr = decors ? `d-${decors.map((item) => item.id).join('_')}` : '';
    const smellStr = smells ? `s-${smells.map((item) => item.id).join('_')}` : '';
    const wickStr = wicks ? `w-${wicks.map((item) => item.id).join('_')}` : '';
    const quantityStr = `q-${quantity}`;

    const result = [
      candleStr,
      numberOfLayersStr,
      layerColorsStr,
      decorStr,
      smellStr,
      wickStr,
      quantityStr,
    ]
      .filter((str) => str !== '')
      .join('~');

    return result;
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

  const addQueryStringFromCandleDetailWithQuantity = (
    arrayCandleDetailWithQuantity: CandleDetailWithQuantity[],
  ) => {
    let newQueryString = '';

    for (let i = 0; i < arrayCandleDetailWithQuantity.length; i++) {
      const candleDetail = arrayCandleDetailWithQuantity[i];
      const candle = candleDetail.candle;
      const decors = candleDetail.decors?.map((decor) => decor.id).join('_');
      const layerColors = candleDetail.layerColors?.map((layerColor) => layerColor.id).join('_');
      const numberOfLayers = candleDetail.numberOfLayers
        ?.map((numberOfLayer) => numberOfLayer.id)
        .join('_');
      const smells = candleDetail.smells?.map((smell) => smell.id).join('_');
      const wicks = candleDetail.wicks?.map((wick) => wick.id).join('_');
      const quantity = candleDetail.quantity;

      let queryString = `c-${candle.id}`;

      if (decors) {
        queryString += `~d-${decors}`;
      }

      if (layerColors) {
        queryString += `~l-${layerColors}`;
      }

      if (numberOfLayers) {
        queryString += `~n-${numberOfLayers}`;
      }

      if (smells) {
        queryString += `~s-${smells}`;
      }

      if (wicks) {
        queryString += `~w-${wicks}`;
      }

      queryString += `~q-${quantity}`;

      newQueryString += queryString;

      if (i < arrayCandleDetailWithQuantity.length - 1) {
        newQueryString += '.';
      }
    }

    navigate(`?${newQueryString}`);
  };

  const checkCandleDetail = (candleDetail: CandleDetails): string[] => {
    const errorMessageInCandleDetail: string[] = [];

    const errorMessageParts: string[] = [];
    if (!candleDetail.numberOfLayers) {
      errorMessageParts.push('количество слоев');
    }
    if (!candleDetail.layerColors) {
      errorMessageParts.push('восковые слои');
    }
    if (!candleDetail.wicks) {
      errorMessageParts.push('фитиль');
    }

    if (errorMessageParts.length > 0) {
      errorMessageInCandleDetail.push(
        `Не выбрано следующее обязательное поле(я): ${errorMessageParts.join(', ')}`,
      );
    }

    const numberOfLayers = candleDetail.numberOfLayers?.[0];
    const layerColors = candleDetail.layerColors;

    if (numberOfLayers && layerColors && numberOfLayers.number !== layerColors.length) {
      errorMessageInCandleDetail.push(
        'Количество слоев не совпадает с количеством выбранных цветовых слоев',
      );
    }

    return errorMessageInCandleDetail;
  };

  useEffect(() => {}, [candleDetailWithQuantity]);

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
    let newTotalPrice: number = 0;
    for (let i = 0; i < candleDetailWithQuantity.length; i++) {
      newTotalPrice +=
        calculatePrice(candleDetailWithQuantity[i]) * candleDetailWithQuantity[i].quantity;
    }
    setTotalPrice(newTotalPrice);
  }, [candleDetailWithQuantity, totalPrice]);

  useEffect(() => {
    setIsCandleDetailWithQuantityLoading(true);
    const searchParams = new URLSearchParams(location.search);
    let queryString = searchParams.toString().replace(/=$/, '');

    const arrayCandleDetailIdsWithQuantityString: string[] = getCandleStatusStrings(queryString);

    const validArrayCandleDetailIdsWithQuantityString: string[] =
      validateCandleDetailIdsWithQuantityString(arrayCandleDetailIdsWithQuantityString);

    if (
      validArrayCandleDetailIdsWithQuantityString.length !==
      arrayCandleDetailIdsWithQuantityString.length
    ) {
      const newQueryString = validArrayCandleDetailIdsWithQuantityString.join('.');
      navigate(`?${newQueryString}`);
    }

    const arrayCandleDetailIdsWithQuantity: CandleDetailIdsWithQuantity[] =
      parseCandleDetailIdsWithQuantityString(validArrayCandleDetailIdsWithQuantityString);

    processCandleDetail(arrayCandleDetailIdsWithQuantity);
    setIsCandleDetailWithQuantityLoading(false);
  }, [location.search]);

  function getCandleStatusStrings(queryString: string): string[] {
    return queryString.split('.').map(decodeURIComponent);
  }

  function validateCandleDetailIdsWithQuantityString(strArray: string[]): string[] {
    const validStrings: string[] = [...strArray];
    const errorMessageInvalidStrings: string[] = [];

    const validTypes = ['c', 'n', 'l', 'w', 'q', 'd', 's'];

    for (const str of strArray) {
      if (str === '' || str === '=') {
        continue;
      }

      const components = str.split('~');
      let removeString = false;

      for (const component of components) {
        const [type, value] = component.split('-');

        if (!validTypes.includes(type)) {
          errorMessageInvalidStrings.push(`Невалидный тип компонента в строке '${str}': ${type}`);
          removeString = true;
        }

        if (type === 'l') {
          const layers = value.split('_');
          for (const layerValue of layers) {
            if (isNaN(Number(layerValue)) || Number(layerValue) <= 0) {
              errorMessageInvalidStrings.push(
                `Невалидное значение для компонента в строке '${str}': ${type}: ${layerValue}`,
              );
              removeString = true;
            }
          }
        } else if (isNaN(Number(value)) || Number(value) <= 0) {
          errorMessageInvalidStrings.push(
            `Невалидное значение для компонента в строке '${str}': ${type}: ${value}`,
          );
          removeString = true;
        }
      }

      if (removeString) {
        const index = validStrings.indexOf(str);
        if (index > -1) {
          validStrings.splice(index, 1);
        }
      }
    }

    setErrorMessage((prev) => [...prev, ...errorMessageInvalidStrings]);
    return validStrings;
  }

  function parseCandleDetailIdsWithQuantityString(
    strings: string[],
  ): CandleDetailIdsWithQuantity[] {
    const candleDetailIdsWithQuantities: CandleDetailIdsWithQuantity[] = [];

    for (const str of strings) {
      if (str === '') {
        continue;
      }

      const candleDetailIdsWithQuantity: CandleDetailIdsWithQuantity = {
        candleId: 0,
        quantity: 0,
      };

      const components = str.split('~');

      for (const component of components) {
        const [type, value] = component.split('-');

        switch (type) {
          case 'c':
            candleDetailIdsWithQuantity.candleId = parseInt(value);
            break;
          case 'd':
            candleDetailIdsWithQuantity.decorIds = mapParseToInt(value);
            break;
          case 'l':
            candleDetailIdsWithQuantity.layerColorIds = mapParseToInt(value);
            break;
          case 'n':
            candleDetailIdsWithQuantity.numberOfLayerIds = mapParseToInt(value);
            break;
          case 's':
            candleDetailIdsWithQuantity.smellIds = mapParseToInt(value);
            break;
          case 'w':
            candleDetailIdsWithQuantity.wickIds = mapParseToInt(value);
            break;
          case 'q':
            candleDetailIdsWithQuantity.quantity = parseInt(value);
            break;
        }
      }
      candleDetailIdsWithQuantities.push(candleDetailIdsWithQuantity);
    }

    return candleDetailIdsWithQuantities;
  }

  function mapParseToInt(value: string): number[] {
    return value.split('_').map(Number);
  }

  function checkCandleComponentsExist(
    candleDetail: CandleDetails,
    candleDetailIdsWithQuantity: CandleDetailIdsWithQuantity,
  ): boolean {
    const errorMessageInvalidCandleComponents: string[] = [];
    let isValid = true;

    if (
      candleDetail.decors &&
      candleDetailIdsWithQuantity.decorIds &&
      !candleDetailIdsWithQuantity.decorIds.every((decorId) =>
        candleDetail.decors?.find((decor) => decor.id === decorId),
      )
    ) {
      errorMessageInvalidCandleComponents.push(
        `У свечи '${candleDetail.candle.title}' нет декора: ${candleDetailIdsWithQuantity.decorIds}`,
      );
      isValid = false;
    }

    if (
      candleDetail.layerColors &&
      candleDetailIdsWithQuantity.layerColorIds &&
      !candleDetailIdsWithQuantity.layerColorIds.every((layerColorId) =>
        candleDetail.layerColors?.some((layerColor) => layerColor.id === layerColorId),
      )
    ) {
      errorMessageInvalidCandleComponents.push(
        `У свечи '${candleDetail.candle.title}' нет слоя: ${candleDetailIdsWithQuantity.layerColorIds}`,
      );
      isValid = false;
    }

    if (
      candleDetail.numberOfLayers &&
      candleDetailIdsWithQuantity.numberOfLayerIds &&
      !candleDetailIdsWithQuantity.numberOfLayerIds.every((numberOfLayerId) =>
        candleDetail.numberOfLayers?.some((numberOfLayer) => numberOfLayer.id === numberOfLayerId),
      )
    ) {
      errorMessageInvalidCandleComponents.push(
        `У свечи '${candleDetail.candle.title}' нет количество слоев: ${candleDetailIdsWithQuantity.numberOfLayerIds}`,
      );
      isValid = false;
    }

    if (
      candleDetail.smells &&
      candleDetailIdsWithQuantity.smellIds &&
      !candleDetailIdsWithQuantity.smellIds.every((smellId) =>
        candleDetail.smells?.some((smell) => smell.id === smellId),
      )
    ) {
      errorMessageInvalidCandleComponents.push(
        `У свечи '${candleDetail.candle.title}' нет запаха: ${candleDetailIdsWithQuantity.smellIds}`,
      );
      isValid = false;
    }

    if (
      candleDetail.wicks &&
      candleDetailIdsWithQuantity.wickIds &&
      !candleDetailIdsWithQuantity.wickIds.every((wickId) =>
        candleDetail.wicks?.some((wick) => wick.id === wickId),
      )
    ) {
      errorMessageInvalidCandleComponents.push(
        `У свечи '${candleDetail.candle.title}' нет фитиля: ${candleDetailIdsWithQuantity.wickIds}`,
      );
      isValid = false;
    }

    if (!isValid) {
      setErrorMessage((prev) => [...prev, ...errorMessageInvalidCandleComponents]);
    }

    return isValid;
  }

  function createCandleDetailWithQuantity(
    candleDetail: CandleDetails,
    candleDetailIdsWithQuantity: CandleDetailIdsWithQuantity,
  ) {
    const candleDetailWithQuantity: CandleDetailWithQuantity = {
      candle: candleDetail.candle,
      quantity: candleDetailIdsWithQuantity.quantity,
    };

    if (candleDetail.decors && candleDetailIdsWithQuantity.decorIds) {
      candleDetailWithQuantity.decors = candleDetail.decors.filter((decor) =>
        candleDetailIdsWithQuantity.decorIds?.includes(decor.id),
      );
    }

    if (candleDetail.layerColors && candleDetailIdsWithQuantity.layerColorIds) {
      candleDetailWithQuantity.layerColors = candleDetail.layerColors.filter((layerColor) =>
        candleDetailIdsWithQuantity.layerColorIds?.includes(layerColor.id),
      );
    }

    if (candleDetail.numberOfLayers && candleDetailIdsWithQuantity.numberOfLayerIds) {
      candleDetailWithQuantity.numberOfLayers = candleDetail.numberOfLayers.filter(
        (numberOfLayer) => candleDetailIdsWithQuantity.numberOfLayerIds?.includes(numberOfLayer.id),
      );
    }

    if (candleDetail.smells && candleDetailIdsWithQuantity.smellIds) {
      candleDetailWithQuantity.smells = candleDetail.smells.filter((smell) =>
        candleDetailIdsWithQuantity.smellIds?.includes(smell.id),
      );
    }

    if (candleDetail.wicks && candleDetailIdsWithQuantity.wickIds) {
      candleDetailWithQuantity.wicks = candleDetail.wicks.filter((wick) =>
        candleDetailIdsWithQuantity.wickIds?.includes(wick.id),
      );
    }

    return candleDetailWithQuantity;
  }

  async function processCandleDetail(
    arrayCandleDetailIdsWithQuantity: CandleDetailIdsWithQuantity[],
  ) {
    const newCandleDetailWithQuantity: CandleDetailWithQuantity[] = [];

    for (const candleDetailIdsWithQuantity of arrayCandleDetailIdsWithQuantity) {
      const candleDetail = await ConstructorApi.getCandleById(
        candleDetailIdsWithQuantity.candleId.toString(),
      );

      if (checkCandleComponentsExist(candleDetail, candleDetailIdsWithQuantity)) {
        const candleDetailWithQuantityState = createCandleDetailWithQuantity(
          candleDetail,
          candleDetailIdsWithQuantity,
        );

        if (
          !candleDetailWithQuantityExists(candleDetailWithQuantity, candleDetailWithQuantityState)
        ) {
          newCandleDetailWithQuantity.push(candleDetailWithQuantityState);
        }
      }
    }

    addQueryStringFromCandleDetailWithQuantity(newCandleDetailWithQuantity);
    setCandleDetailWithQuantity(newCandleDetailWithQuantity);
  }

  function candleDetailWithQuantityExists(
    candleDetailWithQuantity: CandleDetailWithQuantity[],
    candleDetailWithQuantityState: CandleDetailWithQuantity,
  ) {
    return candleDetailWithQuantity.some(
      (item) =>
        item.candle === candleDetailWithQuantityState.candle &&
        item.decors === candleDetailWithQuantityState.decors &&
        item.numberOfLayers === candleDetailWithQuantityState.numberOfLayers &&
        item.layerColors === candleDetailWithQuantityState.layerColors &&
        item.wicks === candleDetailWithQuantityState.wicks &&
        item.smells === candleDetailWithQuantityState.smells,
    );
  }

  const handleChangeCandleDetailWithQuantity = (products: CandleDetailWithQuantity[]) => {
    navigate('');
    products.forEach((product) => {
      const queryString = convertToCandleString(product);
      addQueryString(queryString);
    });
    setCandleDetailWithQuantity(products);
  };

  const handleSelectCandle = (candle: ImageProduct) => {
    showCandleForm(candle.id);
    setPrice(0);
  };

  const calculatePriceCandleDetail = (candleDetail: CandleDetails) => {
    setPrice(calculatePrice(candleDetail));
  };

  const getCreateOrderLink = (): string => {
    var configuredCandlesString = location.search;
    return `/order${configuredCandlesString}`;
  };

  return (
    <div className={Style.container}>
      <div className={Style.popUpNotification}>
        <ListErrorPopUp messages={errorMessage} />
      </div>
      <div className={Style.leftPanel}>
        {isCandleDetailWithQuantityLoading ? (
          <ListProductsCartSkeleton />
        ) : (
          <ListProductsCart
            products={candleDetailWithQuantity}
            onChangeCandleDetailWithQuantity={handleChangeCandleDetailWithQuantity}
          />
        )}
      </div>
      <div className={Style.imagePanel}>
        {candleDetail && <ImageSlider images={candleDetail.candle.images} />}
        {candleDetail && (
          <div className={Style.hideCandleForm}>
            <button onClick={() => hideCandleForm()}>
              <IconArrowLeftLarge color="#777" />
            </button>
          </div>
        )}
        {candleDetail && (
          <div className={Style.priceCandle}>
            <span>{price} р</span>
          </div>
        )}
        <div className={Style.orderInfo}>
          {/* <Link to={getProductLink()} className={Style.link}>
            <p className={Style.title}>{product.title}</p>
            <p className={Style.description}>{product.description}</p>
          </Link> */}
          <div className={Style.orderBtn}>
            <Link to={getCreateOrderLink()}>Заказать</Link>
          </div>
          <div className={Style.totalPrice}>
            <span className={Style.title}>Итого </span>
            <span className={Style.price}>{totalPrice} р</span>
          </div>
        </div>
      </div>
      {candleDetail ? (
        <div className={Style.rightPanel}>
          <CandleForm
            candleDetailData={candleDetail}
            addCandleDetail={addCandleToListProductsCart}
            calculatePriceCandleDetail={calculatePriceCandleDetail}
          />
        </div>
      ) : !candleTypeWithCandles ? (
        <div className={Style.rightPanel}>
          <CandleSelectionPanelSkeleton />
        </div>
      ) : (
        <div className={Style.rightPanel}>
          <CandleSelectionPanel data={candleTypeWithCandles} onSelectCandle={handleSelectCandle} />
        </div>
      )}
    </div>
  );
};

export default ConstructorPage;
