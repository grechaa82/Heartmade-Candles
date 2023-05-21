import React, { useState, useEffect } from 'react';

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
    <h1>Candles</h1>
    <ul>
      {candlesData.map((candle: CandleData) => (
        <li key={candle.id.toString()}>
          <p>{candle.title}</p>
          <p>{candle.price} руб</p>
        </li>
      ))}
    </ul>
  </div>
  );
};

export default CandlePage;
