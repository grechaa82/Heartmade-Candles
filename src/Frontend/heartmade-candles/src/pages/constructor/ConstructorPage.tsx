import { FC, useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';

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
import ErrorPopUp from '../../components/constructor/ErrorPopUp';

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

  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';
  const firstImage =
    candleDetail?.candle.images && candleDetail?.candle.images.length > 0
      ? candleDetail?.candle.images[0]
      : null;

  const [showError, setShowError] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');

  async function showCandleForm(candleId: number) {
    try {
      const candleDetail = await ConstructorApi.getCandleById(candleId.toString());
      setCandleDetail(candleDetail);
    } catch (error) {
      console.error('Произошла ошибка при загрузке данных:', error);
    }
  }

  function hideCandleForm() {
    setCandleDetail(undefined);
  }

  const addCandleToListProductsCart = (candleDetail: CandleDetail): void => {
    const validCandleDetail = checkCandleDetail(candleDetail);
    if (validCandleDetail) {
      setErrorMessage(validCandleDetail);
      setShowError(true);
      return;
    }

    const newCandleDetailWithQuantity: CandleDetailWithQuantity = {
      candle: candleDetail.candle,
      decors: candleDetail.decors,
      layerColors: candleDetail.layerColors,
      numberOfLayers: candleDetail.numberOfLayers,
      smells: candleDetail.smells,
      wicks: candleDetail.wicks,
      quantity: 1,
    };

    setCandleDetailWithQuantity([...candleDetailWithQuantity, newCandleDetailWithQuantity]);
    setCandleDetail(undefined);
  };

  const checkCandleDetail = (candleDetail: CandleDetail): string | undefined => {
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
      const errorMessage = `Не выбрано следующее обязательное поле(я): ${errorMessageParts.join(
        ', ',
      )}`;
      return errorMessage;
    }

    if (candleDetail.numberOfLayers && candleDetail.layerColors) {
      const numberOfLayer: NumberOfLayer = candleDetail.numberOfLayers[0];
      const layerColors: LayerColor[] = candleDetail.layerColors;

      if (numberOfLayer.number !== layerColors.length) {
        return 'Количество слоев не совпадает с количеством выбранных слоев';
      }
    }

    return undefined;
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
    /*  
    1 - привести url к массиву строк состояний свечей

    2 - проверить валидна ли эта строка 

    3 - приводим к типу CandleDetailIdsWithQuantity

    4 - вытянуть свечу с сервера по id из CandleDetailIdsWithQuantity

    5 - проверяем если все ли такие элементы из CandleDetailIdsWithQuantity есть в CadndleDetail, 
    который с сервера

    6 - создать новый CandleDetailIdsWithQuantityState, 
    добавить в него все выбранные элементы из CandleDetailIdsWithQuantity и CadndleDetail

    7 - добавить CandleDetailIdsWithQuantityState в candleDetailWithQuantity, 
    которое поле, которое для ListProductsCart
    */

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

    const processCandleDetail = async () => {
      let newCandleDetailWithQuantity: CandleDetailWithQuantity[] = [];

      for (const candleDetailIdsWithQuantity of arrayCandleDetailIdsWithQuantity) {
        const candleDetail = await ConstructorApi.getCandleById(
          candleDetailIdsWithQuantity.candleId.toString(),
        );

        const isValid = checkCandleComponentsExist(candleDetail, candleDetailIdsWithQuantity);
        if (isValid) {
          const candleDetailWithQuantityState = createCandleDetailWithQuantity(
            candleDetail,
            candleDetailIdsWithQuantity,
          );
          if (
            !candleDetailWithQuantity.find(
              (item) =>
                item.candle === candleDetailWithQuantityState.candle &&
                item.decors === candleDetailWithQuantityState.decors &&
                item.numberOfLayers === candleDetailWithQuantityState.numberOfLayers &&
                item.layerColors === candleDetailWithQuantityState.layerColors &&
                item.smells === candleDetailWithQuantityState.smells &&
                item.wicks === candleDetailWithQuantityState.wicks,
            )
          ) {
            newCandleDetailWithQuantity.push(candleDetailWithQuantityState);
          }
        } else {
          console.log('что-то не так');
        }
      }

      setCandleDetailWithQuantity(newCandleDetailWithQuantity);
    };

    processCandleDetail();
  }, [location.search]);

  function getCandleStatusStrings(queryString: string): string[] {
    return decodeURIComponent(queryString).split('.');
  }

  function validateCandleDetailIdsWithQuantityString(strArray: string[]): string[] {
    const invalidStrings: string[] = [];

    for (const str of strArray) {
      const components = str.split(',');

      const validTypes = ['c', 'n', 'l', 'w', 'q', 'd', 's'];

      for (const component of components) {
        const [type, value] = component.split('-');

        if (!validTypes.includes(type)) {
          invalidStrings.push(`Невалидный тип компонента в строке '${str}': ${type}`);
        }

        if (type === 'l') {
          const layers = value.split('_');
          for (const layerValue of layers) {
            if (isNaN(Number(layerValue)) || Number(layerValue) < 0) {
              invalidStrings.push(
                `Невалидное значение для компонента в строке '${str}': ${type}: ${layerValue}`,
              );
            }
          }
        } else if (isNaN(Number(value)) || Number(value) < 0) {
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
      const candleDetailIdsWithQuantity: CandleDetailIdsWithQuantity = {
        candleId: 0,
        quantity: 0,
      };

      const components = str.split(',');

      for (const component of components) {
        const [type, value] = component.split('-');

        switch (type) {
          case 'c':
            candleDetailIdsWithQuantity.candleId = parseInt(value);
            break;
          case 'd':
            candleDetailIdsWithQuantity.decorIds = value.split('_').map(Number);
            break;
          case 'l':
            candleDetailIdsWithQuantity.layerColorIds = value.split('_').map(Number);
            break;
          case 'n':
            candleDetailIdsWithQuantity.numberOfLayerIds = value.split('_').map(Number);
            break;
          case 's':
            candleDetailIdsWithQuantity.smellIds = value.split('_').map(Number);
            break;
          case 'w':
            candleDetailIdsWithQuantity.wickIds = value.split('_').map(Number);
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

  function checkCandleComponentsExist(
    candleDetail: CandleDetail,
    candleDetailIdsWithQuantity: CandleDetailIdsWithQuantity,
  ): boolean {
    if (candleDetail.decors && candleDetailIdsWithQuantity.decorIds) {
      for (const decorId of candleDetailIdsWithQuantity.decorIds) {
        if (!candleDetail.decors.find((decor) => decor.id === decorId)) {
          return false;
        }
      }
    }

    if (candleDetail.layerColors && candleDetailIdsWithQuantity.layerColorIds) {
      for (const layerColorId of candleDetailIdsWithQuantity.layerColorIds) {
        if (!candleDetail.layerColors.find((layerColor) => layerColor.id === layerColorId)) {
          return false;
        }
      }
    }

    if (candleDetail.numberOfLayers && candleDetailIdsWithQuantity.numberOfLayerIds) {
      for (const numberOfLayerId of candleDetailIdsWithQuantity.numberOfLayerIds) {
        if (
          !candleDetail.numberOfLayers.find(
            (numberOfLayers) => numberOfLayers.id === numberOfLayerId,
          )
        ) {
          return false;
        }
      }
    }

    if (candleDetail.smells && candleDetailIdsWithQuantity.smellIds) {
      for (const smellId of candleDetailIdsWithQuantity.smellIds) {
        if (!candleDetail.smells.find((smell) => smell.id === smellId)) {
          return false;
        }
      }
    }

    if (candleDetail.wicks && candleDetailIdsWithQuantity.wickIds) {
      for (const wickId of candleDetailIdsWithQuantity.wickIds) {
        if (!candleDetail.wicks.find((wick) => wick.id === wickId)) {
          return false;
        }
      }
    }

    return true;
  }

  function createCandleDetailWithQuantity(
    candleDetail: CandleDetail,
    candleDetailIdsWithQuantity: CandleDetailIdsWithQuantity,
  ): CandleDetailWithQuantity {
    const candleWithQuantity: CandleDetailWithQuantity = {
      candle: candleDetail.candle,
      quantity: candleDetailIdsWithQuantity.quantity,
    };

    if (candleDetail.decors && candleDetailIdsWithQuantity.decorIds) {
      candleWithQuantity.decors = candleDetail.decors.filter((decor) =>
        candleDetailIdsWithQuantity.decorIds?.includes(decor.id),
      );
    }

    if (candleDetail.layerColors && candleDetailIdsWithQuantity.layerColorIds) {
      candleWithQuantity.layerColors = candleDetail.layerColors.filter((layerColor) =>
        candleDetailIdsWithQuantity.layerColorIds?.includes(layerColor.id),
      );
    }

    if (candleDetail.numberOfLayers && candleDetailIdsWithQuantity.numberOfLayerIds) {
      candleWithQuantity.numberOfLayers = candleDetail.numberOfLayers.filter((numberOfLayer) =>
        candleDetailIdsWithQuantity.numberOfLayerIds?.includes(numberOfLayer.id),
      );
    }

    if (candleDetail.smells && candleDetailIdsWithQuantity.smellIds) {
      candleWithQuantity.smells = candleDetail.smells.filter((smell) =>
        candleDetailIdsWithQuantity.smellIds?.includes(smell.id),
      );
    }

    if (candleDetail.wicks && candleDetailIdsWithQuantity.wickIds) {
      candleWithQuantity.wicks = candleDetail.wicks.filter((wick) =>
        candleDetailIdsWithQuantity.wickIds?.includes(wick.id),
      );
    }

    return candleWithQuantity;
  }

  const handleChangeCandleDetailWithQuantity = (products: CandleDetailWithQuantity[]) => {
    setCandleDetailWithQuantity(products);
  };

  const handleSelectCandle = (candle: ImageProduct) => {
    showCandleForm(candle.id);
    setPrice(0);
  };

  const calculatePriceCandleDetail = (candleDetail: CandleDetail) => {
    setPrice(calculatePrice(candleDetail));
  };

  const closeErrorPopUp = () => {
    setShowError(false);
  };

  const createOrder = () => {};

  return (
    <>
      <div className={Style.container}>
        <div className={Style.popUpNotification}>
          {showError && <ErrorPopUp message={errorMessage} onClose={closeErrorPopUp} />}
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
