import React, { useState, useEffect } from 'react';
import CandlesGrid from '../modules/ProductsGrid';
import { Candle } from '../types/Candle';

import Style from './CandlePage.module.css';

export interface CandlePageProps {}

const CandlePage: React.FC<CandlePageProps> = () => {
  const [candlesData, setCandlesData] = useState<Candle[]>([]);

  useEffect(() => {
    async function fetchCandles() {
      const response = await fetch(`http://localhost:5000/api/admin/candles/`);
      const data = await response.json();
      setCandlesData(data);
    }
    fetchCandles();
  }, []);

  return (
    <div>
      <CandlesGrid data={candlesData} title="Свечи" />
    </div>
  );
};

export default CandlePage;
