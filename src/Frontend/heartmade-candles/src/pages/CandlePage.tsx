import React, { useState, useEffect } from 'react';
import BlockProduct from '../components/ProductBlock';
import ButtonWithIcon from '../components/ButtonWithIcon';
import IconPlusLarge from '../UI/IconPlusLarge';
import CandlesGrid from '../modules/CandlesGrid';

import Style from './CandlePage.module.css';

export interface CandleData {
  id: number;
  title: string;
  description: string;
  price: number;
  weightGrams: number;
  imageURL: string;
  isActive: boolean;
  typeCandle: number;
  createdAt: string;
}

export interface CandlePageProps {}

const CandlePage: React.FC<CandlePageProps> = () => {
  const [candlesData, setCandlesData] = useState<CandleData[]>([]);

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
      <CandlesGrid candlesData={candlesData} />
    </div>
  );
};

export default CandlePage;
