import { FC, useEffect, useState } from 'react';

import CandleSelectionPanel from '../../modules/constructor/CandleSelectionPanel';
import { CandleTypeWithCandles } from '../../typesV2/CandleTypeWithCandles';

import Style from './ConstructorCandlesPage.module.css';

import { ConstructorApi } from '../../services/ConstructorApi';

const ConstructorCandlesPage: FC = () => {
  const [candleTypeWithCandles, setCandleTypeWithCandles] = useState<CandleTypeWithCandles[]>();

  useEffect(() => {
    async function fetchData() {
      try {
        const candles = await ConstructorApi.getCandles();
        setCandleTypeWithCandles(candles);
      } catch (error) {
        console.error('Произошла ошибка при загрузке данных:', error);
      }
    }

    fetchData();
  }, []);

  return (
    <>
      <div className={Style.container}>
        <div className={Style.leftPanel}></div>
        <div className={Style.imagePanel}></div>
        <div className={Style.rightPanel}>
          {candleTypeWithCandles && <CandleSelectionPanel data={candleTypeWithCandles} />}
        </div>
      </div>
    </>
  );
};

export default ConstructorCandlesPage;
