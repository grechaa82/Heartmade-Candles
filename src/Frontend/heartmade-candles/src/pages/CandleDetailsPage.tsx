import { FC, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import MainInfoCandles from '../modules/MainInfoCandles';
import { CandleDetail } from '../types/CandleDetail';
import ProductsGrid from '../modules/ProductsGrid';
import TagsGrid from '../modules/TagsGrid';

type CandleDetailsParams = {
  id: string;
};

const CandleDetailsPage: FC = () => {
  const { id } = useParams<CandleDetailsParams>();
  const [candleDetailData, setCandleDetailData] = useState<CandleDetail | undefined>(undefined);

  useEffect(() => {
    async function fetchCandle() {
      try {
        const response = await fetch(`http://localhost:5000/api/admin/candles/${id}`);
        if (!response.ok) {
          throw new Error('Ошибка получения данных');
        }
        const data = await response.json();
        setCandleDetailData(data);
      } catch (e) {
        console.log(e);
      }
    }
    fetchCandle();
  }, [id]);

  return (
    <div>
      <div className="candles">
        {candleDetailData && <MainInfoCandles candleData={candleDetailData.candle} />}
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
    </div>
  );
};

export default CandleDetailsPage;
