import { FC } from 'react';

import { TypeCandle } from '../../types/TypeCandle';

import Style from './CandleFilter.module.css';

export interface CandleFilterProps {
  typeCandles?: TypeCandle[];
  selectedTypeCandle?: TypeCandle;
  onChange?: (typeCandles: TypeCandle) => void;
}

const CandleFilter: FC<CandleFilterProps> = ({
  typeCandles,
  selectedTypeCandle,
  onChange,
}) => {
  return (
    <div className={Style.filterBlock}>
      {typeCandles?.map((typeCandle, index) => (
        <button
          key={index}
          onClick={() => onChange(typeCandle)}
          className={`${Style.filterBtn} ${
            typeCandle.title === selectedTypeCandle.title ? Style.selected : ''
          }`}
        >
          {typeCandle.title}
        </button>
      ))}
    </div>
  );
};

export default CandleFilter;
