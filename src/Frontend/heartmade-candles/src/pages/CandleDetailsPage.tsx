import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import MainInfoCandles, { FetchTypeCandles, UpdateCandle } from '../modules/MainInfoCandles';
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
import { Candle } from '../types/Candle';
import { CandleRequest } from '../types/Requests/CandleRequest';
import { Decor } from '../types/Decor';
import { LayerColor } from '../types/LayerColor';
import { Smell } from '../types/Smell';
import { Wick } from '../types/Wick';

type CandleDetailsParams = {
  id: string;
};

const CandleDetailsPage: FC = () => {
  const { id } = useParams<CandleDetailsParams>();
  const [candleDetailData, setCandleDetailData] = useState<CandleDetail | undefined>();

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

  const updateCandle: UpdateCandle = async (candle: Candle): Promise<void> => {
    const { id, title, description, price, weightGrams, imageURL, typeCandle, isActive } = candle;
    const candleRequest: CandleRequest = {
      title,
      description,
      price,
      weightGrams,
      imageURL,
      typeCandle,
      isActive,
    };
    try {
      await putCandle(id.toString(), candleRequest);
    } catch (error) {
      console.error('Произошла ошибка при обновлении свечи:', error);
    }
  };

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
      <div className="candles">
        {candleDetailData && (
          <MainInfoCandles
            candleData={candleDetailData.candle}
            fetchTypeCandles={fetchTypeCandles}
            updateCandle={updateCandle}
          />
        )}
      </div>
      {candleDetailData?.numberOfLayers && (
        <TagsGrid data={candleDetailData.numberOfLayers} title="Количество слоев" />
      )}
      {candleDetailData?.decors && (
        <ProductsGrid data={candleDetailData.decors} title="Декоры" fetchProducts={fetchDecors} />
      )}
      {candleDetailData?.layerColors && (
        <ProductsGrid
          data={candleDetailData.layerColors}
          title="Слои"
          fetchProducts={fetchLayerColors}
        />
      )}
      {candleDetailData?.smells && (
        <ProductsGrid data={candleDetailData.smells} title="Запахи" fetchProducts={fetchSmells} />
      )}
      {candleDetailData?.wicks && (
        <ProductsGrid data={candleDetailData.wicks} title="Фитили" fetchProducts={fetchWicks} />
      )}
    </>
  );
};

export default CandleDetailsPage;
