import React, { FC } from 'react';
import Style from './CandlesGrid.module.css';
import BlockProduct from '../components/ProductBlock';
import ButtonWithIcon from '../components/ButtonWithIcon';
import IconPlusLarge from '../UI/IconPlusLarge';

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

export interface CandlesProps {
  candlesData: CandleData[];
}

const CandlesGrid: React.FC<CandlesProps> = ({ candlesData }) => {
  return (
    <div className={Style.candlesGrid}>
      <h1>Свечи</h1>
      <div className={Style.grid}>
        {candlesData.map((candle: CandleData) => (
          <BlockProduct candleData={candle} />
        ))}
        <ButtonWithIcon
          icon={IconPlusLarge}
          text="Добавить"
          onClick={() => console.log('Кнопка "Добавить" была нажата')}
          color="#2E67EA"
        />
      </div>
    </div>
  );
};

export default CandlesGrid;
