import { FC, useState, useEffect } from 'react';
import CandlesGrid from '../modules/ProductsGrid';
import { Candle } from '../types/Candle';
import { NumberOfLayer } from '../types/NumberOfLayer';
import { TypeCandle } from '../types/TypeCandle';
import TagsGrid from '../modules/TagsGrid';

import Style from './CandlePage.module.css';

export interface CandlePageProps {}

const CandlePage: FC<CandlePageProps> = () => {
  const [typeCandlesData, setTypeCandlesData] = useState<TypeCandle[]>([]);
  const [numberOfLayersData, setNumberOfLayersData] = useState<NumberOfLayer[]>([]);
  const [candlesData, setCandlesData] = useState<Candle[]>([]);

  useEffect(() => {
    async function fetchTypeCandles() {
      try {
        const response = await fetch(`http://localhost:5000/api/admin/typeCandles/`);
        const data = await response.json();
        setTypeCandlesData(data);
      } catch (error) {
        console.error('Произошла ошибка при загрузке типов свечей:', error);
      }
    }

    async function fetchNumberOfLayers() {
      try {
        const response = await fetch(`http://localhost:5000/api/admin/numberOfLayers/`);
        const data = await response.json();
        setNumberOfLayersData(data);
      } catch (error) {
        console.error('Произошла ошибка при загрузке количества слоев:', error);
      }
    }

    async function fetchCandles() {
      try {
        const response = await fetch(`http://localhost:5000/api/admin/candles/`);
        const data = await response.json();
        setCandlesData(data);
      } catch (error) {
        console.error('Произошла ошибка при загрузке свечей:', error);
      }
    }

    fetchTypeCandles();
    fetchNumberOfLayers();
    fetchCandles();
  }, []);

  return (
    <>
      <TagsGrid data={typeCandlesData} title="Типы свечей" />
      <TagsGrid data={numberOfLayersData} title="Количество слоев" />
      <CandlesGrid data={candlesData} title="Свечи" />
    </>
  );
};

export default CandlePage;
