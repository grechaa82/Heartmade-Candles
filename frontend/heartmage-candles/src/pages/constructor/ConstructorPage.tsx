import { FC, useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';

import CandleSelectionPanelSkeleton from '../../modules/constructor/CandleSelectionPanelSkeleton';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import ImageSlider from '../../components/constructor/ImageSlider';
import TutorialBlock from '../../modules/constructor/TutorialBlock';
import { useConstructorContext } from '../../contexts/ConstructorContext';
import { useCandleContext } from '../../contexts/CandleContext';
import ListProductsCart from '../../modules/constructor/ListProductsCart';
import CandleForm from '../../modules/constructor/CandleForm';
import CandleSelectionPanel from '../../modules/constructor/CandleSelectionPanel';
import {
  CustomCandle,
  getFilterFromCustomCandle,
} from '../../typesV2/constructor/CustomCandle';
import { CandleDetailFilterBasketRequest } from '../../typesV2/order/CandleDetailFilterBasketRequest';
import { CandleDetailFilterRequest } from '../../typesV2/order/CandleDetailFilterRequest';
import { CustomCandleBuilder } from '../../typesV2/constructor/CustomCandleBuilder';

import { BasketApi } from '../../services/BasketApi';

import Style from './ConstructorPage.module.css';

const ConstructorPage: FC = () => {
  const { customCandles, isLoadingCandlesByType: isLoading } =
    useConstructorContext();
  const {
    candle,
    fetchCandleById,
    setCustomCandleBuilder,
    updateCustomCandleBuilder,
  } = useCandleContext();
  const [isEditing, setIsEditing] = useState(false);

  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const blockCandleFormRef = useRef<HTMLDivElement>(null);

  const handleOnSelectInProductCart = (selectedCustomCandle: CustomCandle) => {
    const existingCandleIndex = customCandles.findIndex(
      (item) =>
        item.candle.id === selectedCustomCandle.candle.id &&
        item.numberOfLayer === selectedCustomCandle.numberOfLayer &&
        JSON.stringify(item.layerColors) ===
          JSON.stringify(selectedCustomCandle.layerColors) &&
        item.wick === selectedCustomCandle.wick &&
        item.decor === selectedCustomCandle.decor &&
        item.smell === selectedCustomCandle.smell &&
        item.quantity === selectedCustomCandle.quantity,
    );

    if (existingCandleIndex >= 0) {
      customCandles.splice(existingCandleIndex, 1);
      setIsEditing(true);
    }

    const newCustomCandleBuilder = new CustomCandleBuilder()
      .setCandle(selectedCustomCandle.candle)
      .setNumberOfLayer(selectedCustomCandle.numberOfLayer)
      .setLayerColor(selectedCustomCandle.layerColors)
      .setWick(selectedCustomCandle.wick)
      .setDecor(selectedCustomCandle.decor)
      .setSmell(selectedCustomCandle.smell)
      .setQuantity(selectedCustomCandle.quantity)
      .setErrors(selectedCustomCandle.errors);

    fetchCandleById(selectedCustomCandle.candle.id.toString());
    setCustomCandleBuilder(newCustomCandleBuilder);
  };

  const handleHideCandleForm = () => {
    updateCustomCandleBuilder();
    setIsEditing(false);
  };

  const handleOnCreateBasket = async () => {
    if (candle !== undefined) {
      setErrorMessage([
        ...errorMessage,
        'Пожалуйста закончите настройку свечи',
      ]);
    } else if (customCandles.length <= 0) {
      setErrorMessage([
        ...errorMessage,
        'В корзине пока пусто, добавьте свечи',
      ]);
    } else if (customCandles.some((candle) => !candle.isValid)) {
      setErrorMessage([
        ...errorMessage,
        'Некоторые свечи в корзине имеют ошибки, пожалуйста, исправьте их',
      ]);
    } else if (customCandles.length > 0) {
      let candleDetailFilterBasketRequest: CandleDetailFilterBasketRequest = {
        candleDetailFilterRequests: [],
        configuredCandleFiltersString: customCandles
          .map((detail) => detail.filter)
          .join('.'),
      };
      customCandles.forEach((configuredCandle) => {
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
          filterString: configuredCandle.filter,
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

  useEffect(() => {
    if (customCandles.length > 0) {
      const newFilterString = customCandles
        .map((customCandle) => getFilterFromCustomCandle(customCandle))
        .join('.');

      const newUrlSearchParams = new URLSearchParams(`?${newFilterString}`);

      const newQueryString = newUrlSearchParams.toString().replace('=', '');

      navigate(`?${newQueryString}`);
    }
  }, [customCandles]);

  return (
    <div className={Style.container}>
      <ListErrorPopUp messages={errorMessage} />
      <div
        className={`${Style.leftPanel} ${
          customCandles.length === 0 ? Style.noElements : ''
        }`}
      >
        <ListProductsCart
          buttonState={
            candle !== undefined ||
            customCandles.length <= 0 ||
            customCandles.some((candle) => !candle.isValid)
              ? 'invalid'
              : 'valid'
          }
          onSelect={handleOnSelectInProductCart}
          onCreateBasket={handleOnCreateBasket}
        />
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
            hideCandleForm={handleHideCandleForm}
            isEditing={isEditing}
          />
        ) : isLoading ? (
          <CandleSelectionPanelSkeleton />
        ) : (
          <CandleSelectionPanel />
        )}
      </div>
    </div>
  );
};

export default ConstructorPage;
