import { FC, useContext, useState, useEffect, useRef } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

import CandleSelectionPanelSkeleton from '../../modules/constructor/CandleSelectionPanelSkeleton';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import ImageSlider from '../../components/constructor/ImageSlider';
import TutorialBlock from '../../modules/constructor/TutorialBlock';

import { ConstructorApi } from '../../services/ConstructorApi';
import { BasketApi } from '../../services/BasketApi';

import Style from './ConstructorPage.module.css';
import {
  ConstructorProvider,
  useConstructorContext,
} from '../../contexts/ConstructorContext';
import { CandleProvider, useCandleContext } from '../../contexts/CandleContext';
import ListProductsCartV2 from '../../modules/constructor/ListProductsCartV2';
import CandleFormV2 from '../../modules/constructor/CandleFormV2';
import CandleSelectionPanelV2 from '../../modules/constructor/CandleSelectionPanelV2';
import { CustomCandle } from '../../typesV2/constructor/CustomCandle';
import { CandleDetailFilterBasketRequest } from '../../typesV2/order/CandleDetailFilterBasketRequest';

const ConstructorPageV2: FC = () => {
  const { candlesByType, customCandles } = useConstructorContext();
  const {
    candle,
    fetchCandleById,
    customCandleBuilder,
    updateCustomCandleBuilder,
  } = useCandleContext();
  const [isEditing, setIsEditing] = useState(false);
  const [isConfiguredCandleDetailLoading, setIsConfiguredCandleDetailLoading] =
    useState(true);

  const location = useLocation();
  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const blockCandleFormRef = useRef<HTMLDivElement>(null);

  const handleOnSelectInProductCart = (customCandle: CustomCandle) => {
    const existingCandleIndex = customCandles.findIndex(
      (item) =>
        item.candle.id === customCandle.candle.id &&
        item.numberOfLayer === customCandle.numberOfLayer &&
        JSON.stringify(item.layerColors) ===
          JSON.stringify(customCandle.layerColors) &&
        item.wick === customCandle.wick &&
        item.decor === customCandle.decor &&
        item.smell === customCandle.smell &&
        item.quantity === customCandle.quantity,
    );

    console.log('existingCandleIndex', existingCandleIndex);

    if (existingCandleIndex >= 0) {
      console.log('existingCandleIndex >= 0');
      customCandles.splice(existingCandleIndex, 1);
      setIsEditing(true);
    }

    fetchCandleById(customCandle.candle.id.toString());

    customCandleBuilder
      .setCandle(customCandle.candle)
      .setNumberOfLayer(customCandle.numberOfLayer)
      .setLayerColor(customCandle.layerColors)
      .setWick(customCandle.wick)
      .setDecor(customCandle.decor)
      .setSmell(customCandle.smell)
      .setQuantity(customCandle.quantity);
  };

  const handleHideCandleForm = () => {
    updateCustomCandleBuilder();
    setIsEditing(false);
  };

  const handleOnCreateBasket = async () => {
    // if (candle !== undefined) {
    //   setErrorMessage([
    //     ...errorMessage,
    //     'Пожалуйста закончите настройку свечи',
    //   ]);
    // } else if (customCandles.length <= 0) {
    //   setErrorMessage([
    //     ...errorMessage,
    //     'В корзине пока пусто, добавьте свечи',
    //   ]);
    // } else if (customCandles.length > 0) {
    //   let candleDetailFilterBasketRequest: CandleDetailFilterBasketRequest = {
    //     candleDetailFilterRequests: [],
    //     configuredCandleFiltersString: convertToCandleString(configuredCandles),
    //   };
    //   customCandles.forEach((configuredCandle) => {
    //     const filterRequest: CandleDetailFilterRequest = {
    //       candleId: configuredCandle.candle.id,
    //       decorId: configuredCandle.decor ? configuredCandle.decor.id : 0,
    //       numberOfLayerId: configuredCandle.numberOfLayer!.id,
    //       layerColorIds: configuredCandle.layerColors!.map(
    //         (layerColor) => layerColor.id,
    //       ),
    //       smellId: configuredCandle.smell ? configuredCandle.smell.id : 0,
    //       wickId: configuredCandle.wick!.id,
    //       quantity: configuredCandle.quantity,
    //       filterString: configuredCandle.getFilter(),
    //     };
    //     candleDetailFilterBasketRequest.candleDetailFilterRequests.push(
    //       filterRequest,
    //     );
    //   });
    //   var basketIdResponse = await BasketApi.createBasket(
    //     candleDetailFilterBasketRequest,
    //   );
    //   if (basketIdResponse.data && !basketIdResponse.error) {
    //     navigate(`/baskets/${basketIdResponse.data}`);
    //   } else {
    //     setErrorMessage([...errorMessage, basketIdResponse.error as string]);
    //   }
    // } else {
    //   setErrorMessage([
    //     ...errorMessage,
    //     'Что-то пошло не так, попробуйте повторить действие',
    //   ]);
    // }
  };

  return (
    <div className={Style.container}>
      <ListErrorPopUp messages={errorMessage} />
      <div
        className={`${Style.leftPanel} ${
          customCandles.length === 0 ? Style.noElements : ''
        }`}
      >
        <ListProductsCartV2
          buttonState={
            candle !== undefined || customCandles.length <= 0
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
          <CandleFormV2
            hideCandleForm={handleHideCandleForm}
            isEditing={isEditing}
          />
        ) : !candlesByType ? (
          <CandleSelectionPanelSkeleton />
        ) : (
          <CandleSelectionPanelV2 />
        )}
      </div>
    </div>
  );
};

export default ConstructorPageV2;
