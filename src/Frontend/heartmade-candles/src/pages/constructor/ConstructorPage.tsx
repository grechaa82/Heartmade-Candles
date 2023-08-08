import { FC, useState, useEffect } from 'react';

import ListProductsCart from '../../modules/constructor/ListProductsCart';
import CandleForm from '../../modules/constructor/CandleForm';
import {
  CandleDetail,
  CandleDetailWithQuantity,
  ImageProduct,
  NumberOfLayer,
  LayerColor,
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
