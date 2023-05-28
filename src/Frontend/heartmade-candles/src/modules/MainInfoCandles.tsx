import { FC, useState, useEffect } from 'react';
import ButtonDropdown from '../components/ButtonDropdown';
import CheckboxBlock from '../components/CheckboxBlock';
import Textarea from '../components/Textarea';
import Style from './MainInfoCandles.module.css';
import { Candle } from '../types/Candle';
import { TypeCandle } from '../types/TypeCandle';

export interface MainInfoCandlesProps {
  candleData: Candle;
}

const MainInfoCandles: FC<MainInfoCandlesProps> = ({ candleData }) => {
  const [typeCandlesData, setTypeCandlesData] = useState<TypeCandle[]>([]);
  const typeCandlesArray = typeCandlesData.map((candle) => candle.title);

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
    fetchTypeCandles();
  }, []);

  return (
    <div className={Style.candleInfo}>
      <div className={Style.image}></div>
      <form className={`${Style.gridContainer} ${Style.formForCandle}`}>
        <div className={`${Style.formItem} ${Style.itemTitle}`}>
          <Textarea text={candleData.title} label="Название" limitation={{ limit: 48 }} />
        </div>
        <div className={`${Style.formItem} ${Style.itemPrice}`}>
          <Textarea text={candleData.price.toString()} label="Стоимость" />
        </div>
        <div className={`${Style.formItem} ${Style.itemWeightGrams}`}>
          <Textarea text={candleData.weightGrams.toString()} label="Вес в граммах" />
        </div>
        <div className={`${Style.formItem} ${Style.itemType}`}>
          <ButtonDropdown
            text={candleData.typeCandle.title}
            options={typeCandlesArray}
            onClick={() => console.log('text')}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemActive}`}>
          <CheckboxBlock text="Активна" checked={candleData.isActive} />
        </div>
        <div className={`${Style.formItem} ${Style.itemDescription}`}>
          <Textarea
            text={candleData.description}
            label="Описание"
            height={175}
            limitation={{ limit: 256 }}
          />
        </div>
      </form>
    </div>
  );
};

export default MainInfoCandles;
