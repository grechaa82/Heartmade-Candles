import { FC, useState, useEffect, useRef } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

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
import {
  CustomCandleFilter,
  tryParseFilterToCustomCandleFilter,
} from '../../typesV2/constructor/CustomCandleFilter';
import { CandleDetailFilterBasketRequest } from '../../typesV2/order/CandleDetailFilterBasketRequest';
import { CandleDetailFilterRequest } from '../../typesV2/order/CandleDetailFilterRequest';
import { CustomCandleBuilder } from '../../typesV2/constructor/CustomCandleBuilder';
import { ConstructorApi } from '../../services/ConstructorApi';
import { CandleDetail } from '../../typesV2/constructor/CandleDetail';
import LoadCandlePopUp from '../../modules/constructor/PopUp/LoadCandlePopUp';
import { ImageProduct } from '../../typesV2/shared/BaseProduct';

import { BasketApi } from '../../services/BasketApi';

import Style from './ConstructorPage.module.css';

const ConstructorPage: FC = () => {
  const {
    customCandles,
    isLoadingCandlesByType: isLoading,
    setCustomCandles,
  } = useConstructorContext();
  const {
    candle,
    fetchCandleById,
    setCustomCandleBuilder,
    updateCustomCandleBuilder,
  } = useCandleContext();

  const [isEditing, setIsEditing] = useState(false);
  const [isPopUpOpen, setIsPopUpOpen] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const location = useLocation();
  const navigate = useNavigate();
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
        checkConditionOfCandles(customCandles);
        const errors =
          'К сожалению, ваш заказ невозможен, необходимо перенастроить свечу';
        setErrorMessage([...errorMessage, errors]);
      }
    } else {
      setErrorMessage([
        ...errorMessage,
        'Что-то пошло не так, попробуйте повторить действие',
      ]);
    }
  };

  const checkConditionOfCandles = async (
    currentCustomCandle: CustomCandle[],
  ) => {
    let newCustomCandles: CustomCandle[] = [];
    let candleDetails: CandleDetail[] = [];
    let errors: string[] = [];

    if (currentCustomCandle.length === 0) {
      return;
    }

    const fetchCandleDetails = async (customCandle: CustomCandle) => {
      const existingDetail = candleDetails.find(
        (detail) => detail.candle?.id === customCandle.candle?.id,
      );
      if (existingDetail) {
        const newCustomCandle =
          CustomCandleBuilder.checkCustomCandleAgainstCandleDetail(
            customCandle,
            existingDetail,
          );

        newCustomCandles.push(newCustomCandle);
      } else {
        const candleDetailResponse = await ConstructorApi.getCandleById(
          customCandle.candle.id.toString(),
        );

        if (candleDetailResponse.data && !candleDetailResponse.error) {
          const newCustomCandle =
            CustomCandleBuilder.checkCustomCandleAgainstCandleDetail(
              customCandle,
              candleDetailResponse.data,
            );
          newCustomCandles.push(newCustomCandle);
          candleDetails.push(candleDetailResponse.data);
        } else {
          errors.push(`Не удалось найти свечу: ${customCandle.candle.title}`);
        }
      }
    };

    await Promise.all(currentCustomCandle.map(fetchCandleDetails));

    setCustomCandles(newCustomCandles);
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
  }, [customCandles, setCustomCandles]);

  useEffect(() => {
    const localSearch = location.search.replace(/^\?/, '');

    if (localSearch) {
      const customCandleFilters: CustomCandleFilter[] = localSearch
        .split('.')
        .map(tryParseFilterToCustomCandleFilter);

      if (customCandleFilters.length > 0) {
        handlePopUpOpen();
      }
    }
  }, []);

  const loadCandles = async () => {
    let newCustomCandles: CustomCandle[] = [];
    let errors: string[] = [];

    const filterString = decodeURI(
      new URLSearchParams(location.search).toString().replace(/=$/, ''),
    );

    const customCandleFilters: CustomCandleFilter[] = filterString
      .split('.')
      .map(tryParseFilterToCustomCandleFilter)
      .filter((item) => item !== null);

    if (customCandleFilters.length === 0) {
      return;
    }

    const promises = customCandleFilters.map(async (item) => {
      const candleDetailResponse = await ConstructorApi.getCandleById(
        item.candleId.toString(),
      );

      if (candleDetailResponse.data && !candleDetailResponse.error) {
        const candleDetail: CandleDetail = candleDetailResponse.data;
        let errors: string[] = [];

        const customCandleBuilder = new CustomCandleBuilder()
          .setCandle(candleDetail.candle)
          .setQuantity(item.quantity);

        if (item.numberOfLayerId) {
          const numberOfLayer = candleDetail.numberOfLayers.find(
            (layer) => layer.id === item.numberOfLayerId,
          );
          if (!numberOfLayer) {
            errors.push(`Не удалось найти слой с ID ${item.numberOfLayerId}`);
          } else {
            customCandleBuilder.setNumberOfLayer(numberOfLayer);
          }
        }

        if (item.layerColorsIds) {
          const layerColors = candleDetail.layerColors.filter((color) =>
            item.layerColorsIds.includes(color.id),
          );
          if (layerColors.length === 0) {
            errors.push(`Не удалось найти цвета для ID ${item.layerColorsIds}`);
          }
          customCandleBuilder.setLayerColor(layerColors);
        }

        if (item.wickId) {
          const wick = candleDetail.wicks.find((w) => w.id === item.wickId);
          if (!wick) {
            errors.push(`Не удалось найти фитиль с ID ${item.wickId}`);
          }
          customCandleBuilder.setWick(wick);
        }

        if (item.decorId) {
          const decor = candleDetail.decors.find((d) => d.id === item.decorId);
          if (!decor) {
            errors.push(`Не удалось найти декор с ID ${item.decorId}`);
          } else {
            customCandleBuilder.setDecor(decor);
          }
        }

        if (item.smellId) {
          const smell = candleDetail.smells.find((s) => s.id === item.smellId);
          if (!smell) {
            errors.push(`Не удалось найти аромат с ID ${item.smellId}`);
          }
          customCandleBuilder.setSmell(smell);
        }

        customCandleBuilder.setErrors(errors);

        const result = customCandleBuilder.build();

        newCustomCandles.push(result.customCandle);
      }
    });

    await Promise.all(promises);

    setCustomCandles(newCustomCandles);
  };

  const handlePopUpOpen = () => {
    setIsPopUpOpen(true);
  };

  const handlePopUpClose = () => {
    navigate('');
    setIsPopUpOpen(false);
  };

  const handleOnSelectProduct = (candle: ImageProduct) => {
    fetchCandleById(candle.id.toString());
    if (blockCandleFormRef.current) {
      blockCandleFormRef.current.scrollTop = 0;
    }
  };

  return (
    <div className={Style.container}>
      <ListErrorPopUp messages={errorMessage} />
      {isPopUpOpen && (
        <LoadCandlePopUp onClose={handlePopUpClose} loadCandles={loadCandles} />
      )}
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
          <CandleSelectionPanel onSelectProduct={handleOnSelectProduct} />
        )}
      </div>
    </div>
  );
};

export default ConstructorPage;
