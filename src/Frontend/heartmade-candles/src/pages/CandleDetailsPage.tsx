import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import MainInfoCandles, { FetchTypeCandles } from '../modules/MainInfoCandles';
import { CandleDetail } from '../types/CandleDetail';
import ProductsGrid, { FetchProducts } from '../modules/ProductsGrid';
import TagsGrid from '../modules/TagsGrid';
import {
  getCandleById,
  getDecors,
  getLayerColors,
  getSmells,
  getTypeCandles,
  getWicks,
  putCandle,
} from '../Api';
import { UpdateCandleDetailsRequest } from '../types/Requests/UpdateCandleDetailsRequest';
import { CandleRequest } from '../types/Requests/CandleRequest';
import { Candle } from '../types/Candle';
import { Decor } from '../types/Decor';
import { NumberOfLayer } from '../types/NumberOfLayer';
import { LayerColor } from '../types/LayerColor';
import { Smell } from '../types/Smell';
import { Wick } from '../types/Wick';
import { BaseProduct } from '../types/BaseProduct';

import Style from './CandleDetailsPage.module.css';

type CandleDetailsParams = {
  id: string;
};

const CandleDetailsPage: FC = () => {
  const { id } = useParams<CandleDetailsParams>();
  const [candleDetailData, setCandleDetailData] = useState<CandleDetail>();
  const [isChanges, setIsChanges] = useState<boolean>(false);

  const fetchTypeCandles: FetchTypeCandles = async () => {
    try {
      const data = await getTypeCandles();
      return data;
    } catch (error) {
      console.error('Произошла ошибка при загрузке типов свечей:', error);
      return [];
    }
  };

  const fetchDecors: FetchProducts<Decor> = async () => {
    try {
      const data = await getDecors();
      return data;
    } catch (error) {
      console.error('Произошла ошибка при загрузке типов свечей:', error);
      return [];
    }
  };

  const fetchLayerColors: FetchProducts<LayerColor> = async () => {
    try {
      const data = await getLayerColors();
      return data;
    } catch (error) {
      console.error('Произошла ошибка при загрузке типов свечей:', error);
      return [];
    }
  };

  const fetchSmells: FetchProducts<Smell> = async () => {
    try {
      const data = await getSmells();
      return data;
    } catch (error) {
      console.error('Произошла ошибка при загрузке типов свечей:', error);
      return [];
    }
  };

  const fetchWicks: FetchProducts<Wick> = async () => {
    try {
      const data = await getWicks();
      return data;
    } catch (error) {
      console.error('Произошла ошибка при загрузке типов свечей:', error);
      return [];
    }
  };

  const UpdateCandleDetail = async () => {
    console.log('UpdateCandleDetail', candleDetailData);

    try {
      if (candleDetailData && id) {
        const {
          candle,
          decors = [],
          layerColors = [],
          numberOfLayers = [],
          smells = [],
          wicks = [],
        } = candleDetailData;
        const { title, description, price, weightGrams, imageURL, typeCandle, isActive } = candle;

        const candleRequest: CandleRequest = {
          title,
          description,
          price,
          weightGrams,
          imageURL,
          typeCandle,
          isActive,
        };

        const decorsIds = decors.map((decor) => decor.id);
        const layerColorsIds = layerColors.map((layerColor) => layerColor.id);
        const numberOfLayersIds = numberOfLayers.map((numberOfLayer) => numberOfLayer.id);
        const smellsIds = smells.map((smell) => smell.id);
        const wicksIds = wicks.map((wick) => wick.id);

        const updateCandleDetailsRequest: UpdateCandleDetailsRequest = {
          candleRequest,
          decorsIds,
          layerColorsIds,
          numberOfLayersIds,
          smellsIds,
          wicksIds,
        };
        await putCandle(id, updateCandleDetailsRequest);

        setIsChanges(false);

        console.log('UpdateCandleDetail', candleDetailData);
      }
    } catch (error) {
      console.error('Произошла ошибка при обновлении свечи:', error);
    }
  };

  const handleChanges = () => {
    setIsChanges(true);
  };

  const handleChangesCandle = (updatedcandle: Candle) => {
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData,
      candle: {
        ...candleDetailData?.candle,
        ...updatedcandle,
      },
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesDecors = (updatedItems: BaseProduct[]) => {
    const updatedDecors = updatedItems as Decor[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      decors: updatedDecors,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesLayerColors = (updatedItems: BaseProduct[]) => {
    const updatedLayerColors = updatedItems as LayerColor[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      layerColors: updatedLayerColors,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesNumberOfLayers = (updatedNumberOfLayers: NumberOfLayer[]) => {
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      numberOfLayers: updatedNumberOfLayers,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesSmells = (updatedItems: BaseProduct[]) => {
    const updatedSmells = updatedItems as Smell[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      smells: updatedSmells,
    };

    setCandleDetailData(newCandleDetailData);
  };

  const handleChangesWicks = (updatedItems: BaseProduct[]) => {
    const updatedWicks = updatedItems as Wick[];
    const newCandleDetailData: CandleDetail = {
      ...candleDetailData!,
      wicks: updatedWicks,
    };

    setCandleDetailData(newCandleDetailData);
  };

  useEffect(() => {}, [isChanges]);

  useEffect(() => {
    async function fetchCandle() {
      try {
        if (id) {
          const data = await getCandleById(id);
          setCandleDetailData(data);
        }
      } catch (error) {
        console.log(error);
      }
    }
    fetchCandle();
  }, [id]);

  return (
    <>
      {isChanges && candleDetailData && (
        <button className={Style.saveBtn} type="button" onClick={() => UpdateCandleDetail()}>
          Сохранить
        </button>
      )}
      <div className="candles">
        {candleDetailData && (
          <MainInfoCandles
            data={candleDetailData.candle}
            fetchTypeCandles={fetchTypeCandles}
            onChanges={handleChanges}
            handleChangesCandle={handleChangesCandle}
          />
        )}
      </div>
      {candleDetailData?.numberOfLayers && (
        <TagsGrid data={candleDetailData.numberOfLayers} title="Количество слоев" />
      )}
      {candleDetailData?.decors && (
        <ProductsGrid
          data={candleDetailData.decors}
          title="Декоры"
          fetchProducts={fetchDecors}
          onChanges={handleChanges}
          handleChangesProduct={handleChangesDecors}
        />
      )}
      {candleDetailData?.layerColors && (
        <ProductsGrid
          data={candleDetailData.layerColors}
          title="Слои"
          fetchProducts={fetchLayerColors}
          onChanges={handleChanges}
          handleChangesProduct={handleChangesLayerColors}
        />
      )}
      {candleDetailData?.smells && (
        <ProductsGrid
          data={candleDetailData.smells}
          title="Запахи"
          fetchProducts={fetchSmells}
          onChanges={handleChanges}
          handleChangesProduct={handleChangesSmells}
        />
      )}
      {candleDetailData?.wicks && (
        <ProductsGrid
          data={candleDetailData.wicks}
          title="Фитили"
          fetchProducts={fetchWicks}
          onChanges={handleChanges}
          handleChangesProduct={handleChangesWicks}
        />
      )}
    </>
  );
};

export default CandleDetailsPage;
