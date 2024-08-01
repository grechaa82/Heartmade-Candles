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

const ConstructorPageV2: FC = () => {
  const { candlesByType, configuredCandles, totalPrice, setConfiguredCandles } =
    useConstructorContext();
  const {
    candle,
    configuredCandle,
    priceConfiguredCandle,
    setCandle,
    setConfiguredCandle,
    fetchCandleById,
  } = useCandleContext();

  const [isConfiguredCandleDetailLoading, setIsConfiguredCandleDetailLoading] =
    useState(true);

  const location = useLocation();
  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const blockCandleFormRef = useRef<HTMLDivElement>(null);

  return (
    <div className={Style.container}>
      <ListErrorPopUp messages={errorMessage} />
      <div
        className={`${Style.leftPanel} ${
          configuredCandles.length === 0 ? Style.noElements : ''
        }`}
      >
        {/* <ListProductsCartV2 /> */}
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
          <CandleFormV2 />
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
