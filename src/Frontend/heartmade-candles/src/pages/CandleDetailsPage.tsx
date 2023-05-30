import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import MainInfoCandles, { FetchTypeCandles } from '../modules/MainInfoCandles';
import { CandleDetail } from '../types/CandleDetail';
import ProductsGrid from '../modules/ProductsGrid';
import TagsGrid from '../modules/TagsGrid';
import { getCandleById, getTypeCandles } from '../Api';

type CandleDetailsParams = {
  id: string;
};

const CandleDetailsPage: FC = () => {
  const { id } = useParams<CandleDetailsParams>();
  const [candleDetailData, setCandleDetailData] = useState<CandleDetail | undefined>(undefined);

  const fetchTypeCandles: FetchTypeCandles = async () => {
    try {
      const data = await getTypeCandles();
      return data;
    } catch (error) {
      console.error('Произошла ошибка при загрузке типов свечей:', error);
      return [];
    }
  };

  useEffect(() => {
    async function fetchCandle() {
      try {
        if (id) {
          const data = await getCandleById(id);
          setCandleDetailData(data);
        }
      } catch (e) {
        console.log(e);
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
