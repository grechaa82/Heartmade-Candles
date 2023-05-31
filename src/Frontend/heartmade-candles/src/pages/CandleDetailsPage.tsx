import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import MainInfoCandles, { FetchTypeCandles, UpdateCandle } from '../modules/MainInfoCandles';
import { CandleDetail } from '../types/CandleDetail';
import ProductsGrid from '../modules/ProductsGrid';
import TagsGrid from '../modules/TagsGrid';
import { getCandleById, getTypeCandles, putCandle } from '../Api';
import { Candle } from '../types/Candle';
import { CandleRequest } from '../types/Requests/CandleRequest';

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
    console.log('updateCandle candleRequest', candleRequest);
    try {
      await putCandle(id.toString(), candleRequest);
    } catch (error) {
      console.error('Error updating candle:', error);
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
      {candleDetailData?.decors && <ProductsGrid data={candleDetailData.decors} title="Декоры" />}
      {candleDetailData?.layerColors && (
        <ProductsGrid data={candleDetailData.layerColors} title="Слои" />
      )}
      {candleDetailData?.smells && <ProductsGrid data={candleDetailData.smells} title="Запахи" />}
      {candleDetailData?.wicks && <ProductsGrid data={candleDetailData.wicks} title="Фитили" />}
    </>
  );
};

export default CandleDetailsPage;
