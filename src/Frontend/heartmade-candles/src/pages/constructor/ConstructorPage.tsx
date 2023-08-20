import { FC, useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

import ListProductsCart from '../../modules/constructor/ListProductsCart';
import CandleForm from '../../modules/constructor/CandleForm';
import {
  CandleDetail,
  CandleDetailWithQuantity,
  ImageProduct,
  NumberOfLayer,
  LayerColor,
  CandleDetailIdsWithQuantity,
} from '../../typesV2/BaseProduct';
import CandleSelectionPanel from '../../modules/constructor/CandleSelectionPanel';
import { CandleTypeWithCandles } from '../../typesV2/CandleTypeWithCandles';
import IconArrowLeftLarge from '../../UI/IconArrowLeftLarge';
import { calculatePrice } from '../../helpers/CalculatePrice';
import ListErrorPopUp from '../../modules/constructor/ListErrorPopUp';

import Style from './ConstructorPage.module.css';

import { ConstructorApi } from '../../services/ConstructorApi';

const ConstructorPage: FC = () => {
  const [candleDetail, setCandleDetail] = useState<CandleDetail>();
  const [candleDetailWithQuantity, setCandleDetailWithQuantity] = useState<
    CandleDetailWithQuantity[]
  >([]);
  const [candleTypeWithCandles, setCandleTypeWithCandles] = useState<CandleTypeWithCandles[]>();
  const [price, setPrice] = useState<number>(0);
  const [totalPrice, setTotalPrice] = useState<number>(0);
  const location = useLocation();
  const navigate = useNavigate();

  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';
  const firstImage =
    candleDetail?.candle.images && candleDetail?.candle.images.length > 0
      ? candleDetail?.candle.images[0]
      : null;

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

  const addCandleToListProductsCart = (candleDetail: CandleDetail): void => {
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
    const newQueryString = newUrlSearchParams.toString();

    navigate(`?${newQueryString}`);
  };

  const checkCandleDetail = (candleDetail: CandleDetail): string[] => {
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
        'Количество слоев не совпадает с количеством выбранных слоев',
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
    const searchParams = new URLSearchParams(location.search);
    let queryString = searchParams.toString().replace(/=$/, '');

    const arrayCandleDetailIdsWithQuantityString: string[] = getCandleStatusStrings(queryString);

    const isValidArrayCandleDetailIdsWithQuantityString = validateCandleDetailIdsWithQuantityString(
      arrayCandleDetailIdsWithQuantityString,
    );

    if (isValidArrayCandleDetailIdsWithQuantityString.length > 0) {
      console.log('что-то не так');
    }

    const arrayCandleDetailIdsWithQuantity: CandleDetailIdsWithQuantity[] =
      parseCandleDetailIdsWithQuantityString(arrayCandleDetailIdsWithQuantityString);

    processCandleDetail(arrayCandleDetailIdsWithQuantity);
  }, [location.search]);

  function getCandleStatusStrings(queryString: string): string[] {
    return queryString.split('.').map(decodeURIComponent);
  }

  function validateCandleDetailIdsWithQuantityString(strArray: string[]): string[] {
    const invalidStrings: string[] = [];

    for (const str of strArray) {
      console.log(str);
      if (str === '') {
        continue;
      }

      const components = str.split('~');

      const validTypes = ['c', 'n', 'l', 'w', 'q', 'd', 's'];

      for (const component of components) {
        const [type, value] = component.split('-');

        if (!validTypes.includes(type)) {
          invalidStrings.push(`Невалидный тип компонента в строке '${str}': ${type}`);
        }

        if (type === 'l') {
          const layers = value.split('_');
          for (const layerValue of layers) {
            if (isNaN(Number(layerValue)) || Number(layerValue) <= 0) {
              invalidStrings.push(
                `Невалидное значение для компонента в строке '${str}': ${type}: ${layerValue}`,
              );
            }
          }
        } else if (isNaN(Number(value)) || Number(value) <= 0) {
          invalidStrings.push(
            `Невалидное значение для компонента в строке '${str}': ${type}: ${value}`,
          );
        }
      }
    }

    return invalidStrings;
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
    candleDetail: CandleDetail,
    candleDetailIdsWithQuantity: CandleDetailIdsWithQuantity,
  ): boolean {
    if (
      candleDetail.decors &&
      candleDetailIdsWithQuantity.decorIds &&
      !candleDetailIdsWithQuantity.decorIds.every((decorId) =>
        candleDetail.decors?.find((decor) => decor.id === decorId),
      )
    ) {
      return false;
    }

    if (
      candleDetail.layerColors &&
      candleDetailIdsWithQuantity.layerColorIds &&
      !candleDetailIdsWithQuantity.layerColorIds.every((layerColorId) =>
        candleDetail.layerColors?.some((layerColor) => layerColor.id === layerColorId),
      )
    ) {
      return false;
    }

    if (
      candleDetail.numberOfLayers &&
      candleDetailIdsWithQuantity.numberOfLayerIds &&
      !candleDetailIdsWithQuantity.numberOfLayerIds.every((numberOfLayerId) =>
        candleDetail.numberOfLayers?.some((numberOfLayer) => numberOfLayer.id === numberOfLayerId),
      )
    ) {
      return false;
    }

    if (
      candleDetail.smells &&
      candleDetailIdsWithQuantity.smellIds &&
      !candleDetailIdsWithQuantity.smellIds.every((smellId) =>
        candleDetail.smells?.some((smell) => smell.id === smellId),
      )
    ) {
      return false;
    }

    if (
      candleDetail.wicks &&
      candleDetailIdsWithQuantity.wickIds &&
      !candleDetailIdsWithQuantity.wickIds.every((wickId) =>
        candleDetail.wicks?.some((wick) => wick.id === wickId),
      )
    ) {
      return false;
    }

    return true;
  }

  function createCandleDetailWithQuantity(
    candleDetail: CandleDetail,
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
    const newCandleDetailWithQuantity = [];

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

  const calculatePriceCandleDetail = (candleDetail: CandleDetail) => {
    setPrice(calculatePrice(candleDetail));
  };

  const createOrder = () => {};

  return (
    <>
      <div className={Style.container}>
        <div className={Style.popUpNotification}>
          <ListErrorPopUp messages={errorMessage} />
        </div>
        <div className={Style.leftPanel}>
          <ListProductsCart
            products={candleDetailWithQuantity}
            onChangeCandleDetailWithQuantity={handleChangeCandleDetailWithQuantity}
          />
        </div>
        <div className={Style.imagePanel}>
          {candleDetail && (
            <div className={Style.image}>
              {firstImage && (
                <img src={urlToImage + firstImage.fileName} alt={firstImage.alternativeName} />
              )}
            </div>
          )}
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
            <div className={Style.orderBtn}>
              <button onClick={() => createOrder()}>Заказать</button>
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
        ) : (
          candleTypeWithCandles && (
            <div className={Style.rightPanel}>
              <CandleSelectionPanel
                data={candleTypeWithCandles}
                onSelectCandle={handleSelectCandle}
              />
            </div>
          )
        )}
      </div>
    </>
  );
};

export default ConstructorPage;
