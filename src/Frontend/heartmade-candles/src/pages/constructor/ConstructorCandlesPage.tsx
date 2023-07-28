import { FC, useEffect, useState } from 'react';

import Product from '../../components/constructor/Product';
import { Candle } from '../../types/Candle';

import { CandlesApi } from '../../services/CandlesApi';

const ConstructorCandlesPage: FC = () => {
  const [candles, setCandles] = useState<Candle[]>();

  useEffect(() => {
    async function fetchData() {
      try {
        const candles = await CandlesApi.getAll();
        setCandles(candles);
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
      }
    }

    fetchData();
  }, []);

  return (
    <>
      <div>ConstructorPage</div>
      {candles && candles.map((item: Candle) => <Product key={item.id} product={item} />)}
    </>
  );
};

export default ConstructorCandlesPage;
