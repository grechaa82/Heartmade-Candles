import React from 'react';
import Button from '../components/Button';
import ButtonDropdown from '../components/ButtonDropdown';
import CheckboxBlock from '../components/CheckboxBlock';
import Textarea from '../components/Textarea';
import Style from './MainInfoCandles.module.css';

export interface CandleData {
  id: number;
  title: string;
  description: string;
  weightGrams: number;
  imageURL: string;
  isActive: boolean;
  typeCandle: number;
  createdAt: string;
}

export interface MainInfoCandlesProps {
  candleData: CandleData;
}

const MainInfoCandles: React.FC<MainInfoCandlesProps> = ({ candleData }) => {
  return (
    <div className={Style.candleInfo}>
      <div className={Style.image}></div>
      <form className={`${Style.gridContainer} ${Style.formForCandle}`}>
        <div className={`${Style.formItem} ${Style.itemTitle}`}>
          <Textarea text={candleData.title} label="Название" limitation={{ limit: 48 }} />
        </div>
        <div className={`${Style.formItem} ${Style.itemPrice}`}>
          <Textarea text={candleData.weightGrams.toString()} label="Стоимость" />
        </div>
        <div className={`${Style.formItem} ${Style.itemWeightGrams}`}>
          <Textarea text={candleData.weightGrams.toString()} label="Вес в граммах" />
        </div>
        <div className={`${Style.formItem} ${Style.itemType}`}>
          <ButtonDropdown
            text={candleData.typeCandle.toString()}
            options={['Type 1', 'Type 2', 'Type 3', 'Type 4']}
            onClick={() => console.log('text')}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemActive}`}>
          <CheckboxBlock text="Активна" />
        </div>
        <div className={`${Style.formItem} ${Style.itemDescription}`}>
          <Textarea
            text={candleData.description}
            label="Описание"
            height={205}
            limitation={{ limit: 256 }}
          />
        </div>
      </form>
    </div>
  );
};

export default MainInfoCandles;
