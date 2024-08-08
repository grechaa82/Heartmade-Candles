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
import {
  CustomCandle,
  getFilter,
} from '../../typesV2/constructor/CustomCandle';
import { CandleDetailFilterBasketRequest } from '../../typesV2/order/CandleDetailFilterBasketRequest';
import { CandleDetailFilterRequest } from '../../typesV2/order/CandleDetailFilterRequest';
import { OrderItemFilter } from '../../typesV2/shared/OrderItemFilter';
import { CustomCandleBuilder } from '../../typesV2/constructor/CustomCandleBuilder';

const ConstructorPageV2: FC = () => {
  const {
    candlesByType,
    customCandles,
    isLoadingCandlesByType: isLoading,
  } = useConstructorContext();
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

  useEffect(() => {
    if (customCandles.length > 0) {
      const newFilterString = customCandles
        .map((customCandle) => getFilter(customCandle))
        .join('.');

      const newUrlSearchParams = new URLSearchParams(`?${newFilterString}`);

      const newQueryString = newUrlSearchParams.toString().replace('=', '');

      navigate(`?${newQueryString}`);
    }
  }, [customCandles]);

  useEffect(() => {
    let newCustomCandles: CustomCandle[] = [];

    let allErrorMessages: string[] = [];

    const searchParams = new URLSearchParams(location.search);
    const filters = decodeURI(searchParams.toString().replace(/=$/, ''));

    if (filters.length < OrderItemFilter.MIN_LENGTH_FILTER) {
      return;
    }

    const orderItemFilters: OrderItemFilter[] = filters
      .split('.')
      .map(OrderItemFilter.parseToOrderItemFilter);

    orderItemFilters.map(async (orderItemFilter) => {
      const candleDetailResponse = await ConstructorApi.getCandleById(
        orderItemFilter.candleId.toString(),
      );
      if (candleDetailResponse.data && !candleDetailResponse.error) {
        const customCandleBuilder = new CustomCandleBuilder();

        const selectedNumberOfLayer =
          candleDetailResponse.data.numberOfLayers.find(
            (numberOfLayer) =>
              numberOfLayer.id === orderItemFilter.numberOfLayerId,
          );

        const selectedLayerColors =
          candleDetailResponse.data.layerColors.filter((color) =>
            orderItemFilter.layerColorIds.includes(color.id),
          );

        const selectedWick = candleDetailResponse.data.wicks.find(
          (wick) => wick.id === orderItemFilter.wickId,
        );

        const selectedDecor = candleDetailResponse.data.decors?.find(
          (decor) => decor.id === orderItemFilter.decorId,
        );

        const selectedSmell = candleDetailResponse.data.smells?.find(
          (smell) => smell.id === orderItemFilter.smellId,
        );

        customCandleBuilder
          .setCandle(candleDetailResponse.data.candle)
          .setNumberOfLayer(selectedNumberOfLayer)
          .setLayerColor(selectedLayerColors)
          .setWick(selectedWick)
          .setDecor(selectedDecor)
          .setSmell(selectedSmell)
          .setQuantity(orderItemFilter.quantity);

        if (
          customCandleBuilder.checkCustomCandleAgainstCandleDetail(
            candleDetailResponse.data,
          )
        ) {
        }

        const buildResult = customCandleBuilder.build();
        if (buildResult.success) {
          newCustomCandles.push(buildResult.customCandle);
        }
      } else {
        console.error(
          'Error fetching candle details:',
          candleDetailResponse.error,
        );
      }
    });

    console.log('закончился вызов location.search');
  }, [location.search]);

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

    if (existingCandleIndex >= 0) {
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
        ) : isLoading ? (
          <CandleSelectionPanelSkeleton />
        ) : (
          <CandleSelectionPanelV2 />
        )}
      </div>
    </div>
  );
};

export default ConstructorPageV2;
